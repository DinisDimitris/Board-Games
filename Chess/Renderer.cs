using RenderEngine.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Engine.Render
{
    //testing
    public class Renderer : GameWindow
    {
        private readonly float[] _vertices =
        {
             0.10f,  0.10f, 0.0f, // top right
             0.10f, -0.10f, 0.0f, // bottom right
            -0.10f, -0.10f, 0.0f, // bottom left
            -0.10f,  0.10f, 0.0f, // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3, 
            1, 2, 3 
        };

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        private Matrix4 _transform;

        private int _elementBufferObject;

        public Renderer(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            // We also upload data to the EBO the same way as we did with VBOs.
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            
            // The EBO has now been properly setup. Go to the Render function to see how we draw our rectangle now!
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            
            _transform = Matrix4.CreateTranslation(0.5f,0.0f,0.0f);

            //_shader.SetMatrix4("transform", _transform);

            _shader.Use();

            // Because ElementArrayObject is a property of the currently bound VAO,
            // the buffer you will find in the ElementArrayBuffer will change with the currently bound VAO.
            GL.BindVertexArray(_vertexArrayObject);

            // Then replace your call to DrawTriangles with one to DrawElements
            // Arguments:
            //   Primitive type to draw. Triangles in this case.
            //   How many indices should be drawn. Six in this case.
            //   Data type of the indices. The indices are an unsigned int, so we want that here too.
            //   Offset in the EBO. Set this to 0 because we want to draw the whole thing.
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}