using System;
using Common;
using Board.Generator;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Engine.Render

{
    public class Renderer : GameWindow
    {
        private readonly float[] _vertices =
        {
             0.50f,  0.50f, 0.0f, // top right
             0.50f, -0.50f, 0.0f, // bottom right
            -0.50f, -0.50f, 0.0f, // bottom left
            -0.50f,  0.50f, 0.0f, // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        private Matrix4 _model;
        private Matrix4 _view;
        private Matrix4 _projection;

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

            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            _view = Matrix4.CreateTranslation(0.5f, 0.5f, -0.0005f);

            _projection = Matrix4.CreateOrthographicOffCenter(0.0f, 8.0f, 0.0f, 12.0f, -0.1f, 1.0f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            _model = Matrix4.Identity;

            _shader.Use();

            GL.BindVertexArray(_vertexArrayObject);

            _shader.SetMatrix4("model", _model);
            _shader.SetMatrix4("view", _view);
            _shader.SetMatrix4("projection", _projection);

            var boardModel = BoardGenerator.GenerateBySize(8, 8);

            RenderFromBoardModel(boardModel);

            SwapBuffers();
        }

        public void RenderFromBoardModel(Structures.Tile[,] board)
        {

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    _shader.SetMatrix4("model", board[x, y].Identity);
                    _shader.SetVector4("tileColour", board[x, y].Color);

                    GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
                }
            }
            }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            var flippedY = -1 * MouseState.Position.Y + Size.Y;

            var scaledX = MouseState.Position.X == 0 ? MouseState.Position.X: (MouseState.Position.X / 100);
            var scaledY = flippedY == 0 ? flippedY: flippedY / 100; 

                Console.WriteLine( scaledX + " " + scaledY );
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}