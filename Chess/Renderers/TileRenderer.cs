using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Common;
using Structures;

namespace Renderers
{
    public class TileRenderer
    {
        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        public float[] _vertices;
        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private int _elementBufferObject;
        private Vector2 _screenSize;

        private Shader _shader;
        public TileRenderer(Vector2 screenSize)
        {
            _screenSize = screenSize;
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag"); ;
        }
        public void LoadVertexBuffers()
        {
            _vertices = new float[]
            {
                _screenSize.X / 8.0f,  _screenSize.Y / 8.0f, 0.0f, // top right
                _screenSize.X / 8.0f, -_screenSize.Y / 8.0f, 0.0f, // bottom right
                -_screenSize.X / 8.0f, -_screenSize.Y / 8.0f, 0.0f, // bottom left
                -_screenSize.X / 8.0f,  _screenSize.Y / 8.0f, 0.0f, // top left
            };

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexArrayObject = GL.GenVertexArray();
            BindVertexArray();

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        }

        public void LoadShader()
        {
            _shader.Use();
        }

        public void SetView(Matrix4 view)
        {
            _shader.SetMatrix4("view", view);
        }

        public void SetProjection(Matrix4 projection)
        {
            _shader.SetMatrix4("projection", projection);
        }

        public void RenderBoard(Tile[,] board)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    _shader.SetVector3("offset", new Vector3(x * _screenSize.X / 8, y * _screenSize.Y / 8, 1));
                    _shader.SetVector4("tileColour", board[x, y].Color);


                    GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
                }
            }
        }

        public void ResizeBoard(Vector2 Size)
        {
            _screenSize = Size;

            _vertices = new float[]
            {
                Size.X / 8.0f,  Size.Y / 8.0f, 0.0f, // top right
                Size.X / 8.0f, -Size.Y / 8.0f, 0.0f, // bottom right
                -Size.X / 8.0f, -Size.Y / 8.0f, 0.0f, // bottom left
                -Size.X / 8.0f,  Size.Y / 8.0f, 0.0f, // top left
            };

            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
        }

        public void SetTileColours(Vector4 colour)
        {
            _shader.Use();

            _shader.SetVector4("tileColour", colour);

            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void BindVertexArray()
        {
            GL.BindVertexArray(_vertexArrayObject);
        }
    }

}