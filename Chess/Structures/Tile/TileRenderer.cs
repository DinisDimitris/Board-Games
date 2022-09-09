using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Common;

namespace Structures.Tiles
{
    public class TileRenderer
    {
        public readonly uint[] Indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        public int VertexBufferObject;

        public int VertexArrayObject;

        private int ElementBufferObject;
        private Vector2 _screenSize;

        private Shader _shader;
        public TileRenderer(Vector2 screenSize, Shader shader)
        {
            _screenSize = screenSize;
            _shader = shader;
        }
        public void LoadVertexBuffers()
        {
            var vertices = new float[]
            {
                _screenSize.X / 8.0f,  _screenSize.Y / 8.0f, 0.0f, // top right
                _screenSize.X / 8.0f, -_screenSize.Y / 8.0f, 0.0f, // bottom right
                -_screenSize.X / 8.0f, -_screenSize.Y / 8.0f, 0.0f, // bottom left
                -_screenSize.X / 8.0f,  _screenSize.Y / 8.0f, 0.0f, // top left
            };

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            VertexArrayObject = GL.GenVertexArray();
            BindVertexArray();

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);
        }

        public Tile[,] RenderBoard()
        {
            BindVertexArray();

            var board = new Tile[8, 8];

            var position = Matrix4.Identity;

            Vector4 color;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    position = Matrix4.CreateTranslation(x, y, 0);

                    //whites
                    if ((y % 2 == 0 && x % 2 == 0) | (y % 2 == 1 && x % 2 == 1))
                    {
                        color = new Vector4(1, 1, 1,1);
                    }
                    else
                    {
                        color = new Vector4(0, 0, 0, 0);
                    }

                    board[x, y] = new Tile(position, color);

                    _shader.SetVector3("offset", new Vector3(x * _screenSize.X / 8, y * _screenSize.Y / 8, 1));
                    _shader.SetVector4("tileColour", board[x, y].Color);

                    GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
                }
            }

            return board;
        }

        public void ResizeBoard(Vector2 Size)
        {
            _screenSize = Size;

            var vertices = new float[]
            {
                Size.X / 8.0f,  Size.Y / 8.0f, 0.0f, // top right
                Size.X / 8.0f, -Size.Y / 8.0f, 0.0f, // bottom right
                -Size.X / 8.0f, -Size.Y / 8.0f, 0.0f, // bottom left
                -Size.X / 8.0f,  Size.Y / 8.0f, 0.0f, // top left
            };

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }

        public void BindVertexArray()
        {
            GL.BindVertexArray(VertexArrayObject);
        }
    }

}