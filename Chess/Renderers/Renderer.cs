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
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        }
        public void LoadVertexBuffers()
        {
            _vertices = new float[]
            {
                _screenSize.X / 8.0f,  _screenSize.Y / 8.0f, 0.0f, 0.80f, 1.0f, // top right
                _screenSize.X / 8.0f, -_screenSize.Y / 8.0f, 0.0f, 0.80f, 0.0f, // bottom right
                -_screenSize.X / 8.0f, -_screenSize.Y / 8.0f, 0.0f, 0.0f, 0.0f, // bottom left
                -_screenSize.X / 8.0f,  _screenSize.Y / 8.0f, 0.0f, 0.0f, 1.0f, // top left
            };

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexArrayObject = GL.GenVertexArray();
            BindVertexArray();

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

        public void UseShader()
        {
            _shader.Use();
        }

        public void UseTexture()
        {
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
            _vertices = new float[]
            {
                _screenSize.X / 8.0f,  _screenSize.Y / 8.0f, 0.0f, 1.0f, 1.0f, // top right
                _screenSize.X / 8.0f, -_screenSize.Y / 8.0f, 0.0f, 1.0f, 0.0f, // bottom right
                -_screenSize.X / 8.0f, -_screenSize.Y / 8.0f, 0.0f, 0.0f, 0.0f, // bottom left
                -_screenSize.X / 8.0f,  _screenSize.Y / 8.0f, 0.0f, 0.0f, 1.0f, // top left
            };

            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            foreach (var tile in board)
            {
                _shader.SetVector3("offset", new Vector3(tile.Identity.X * _screenSize.X / 8, tile.Identity.Y * _screenSize.Y / 8, 1));
                _shader.SetVector4("tileColour", tile.Color);

                int activeTextureLocation = GL.GetUniformLocation(_shader.Handle, "activeTexture");
                GL.Uniform1(activeTextureLocation, 0);

                // Load and bind texture for the current tile
                int textureHandle = Texture.LoadFromFile(tile.TexturePath); // Assuming each tile has a TexturePath property
                GL.BindTexture(TextureTarget.Texture2D, textureHandle);

                // Draw the tile
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
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

        public void SetTexture(){}

        public void BindVertexArray()
        {
            GL.BindVertexArray(_vertexArrayObject);
        }
    }
}