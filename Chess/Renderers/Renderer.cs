using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Common;
using Structures;

namespace Renderers
{
    public sealed class Renderer
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
        public Renderer(Vector2 screenSize)
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

        public void UseShader()
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
            foreach (var tile in board)
            { _vertices = new float[]
                {
                    _screenSize.X / 8.0f,  _screenSize.Y / 8.0f, 0.0f, // top right
                    _screenSize.X / 8.0f, -_screenSize.Y / 8.0f, 0.0f, // bottom right
                    -_screenSize.X / 8.0f, -_screenSize.Y / 8.0f, 0.0f, // bottom left
                    -_screenSize.X / 8.0f,  _screenSize.Y / 8.0f, 0.0f, // top left
                };

                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
                _shader.SetVector3("offset", new Vector3(tile.Identity.X * _screenSize.X / 8, tile.Identity.Y * _screenSize.Y / 8, 1));
                SetProjection(Matrix4.CreateOrthographicOffCenter(0.0f, _screenSize.X, 0.0f, _screenSize.Y, -0.1f, 1.0f));
                SetView(Matrix4.CreateTranslation(_screenSize.X / 8, _screenSize.Y / 8, -0.0005f));
                _shader.SetVector4("tileColour", tile.Color);

                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

                if (tile.Piece != null)
                {
                    _vertices = new float[]
                    {
                        (_screenSize.X - 100.0f)/ 8.0f,  (_screenSize.Y - 100.0f)/ 8.0f, 0.0f, // top right
                        (_screenSize.X - 100.0f)/ 8.0f, (-_screenSize.Y + 100.0f)/ 8.0f, 0.0f, // bottom right
                        (-_screenSize.X + 100.0f) / 8.0f, (-_screenSize.Y + 100.0f) / 8.0f, 0.0f, // bottom left
                        (-_screenSize.X + 100.0f) / 8.0f,  (_screenSize.Y - 100.0f)/ 8.0f, 0.0f, // top left
                    };

                    GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

                    SetView(Matrix4.CreateTranslation((_screenSize.X - 100.0f)/ 8 , (_screenSize.Y- 100.0f) / 8, -0.0005f));
                    SetProjection(Matrix4.CreateOrthographicOffCenter(0.0f, (_screenSize.X + 100.0f), 0.0f, (_screenSize.Y + 100.0f), -0.1f, 1.0f));
                    _shader.SetVector3("offset", new Vector3(tile.Piece.Identity.X * _screenSize.X / 8, tile.Piece.Identity.Y * _screenSize.Y / 8, 1));
                    _shader.SetVector4("tileColour", tile.Piece.Color);

                    GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
                }
            }
        }

        public void ResizeBoard(Vector2 Size)
        {
            _screenSize = Size;
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