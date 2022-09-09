using System;
using Common;
using Structures.Tiles;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Engine.Board
{
    public class Renderer : GameWindow
    {
        private TileRenderer _tileRenderer;
        private Shader _shader;
        private Matrix4 _view;
        private Matrix4 _projection;

        private Tile[,] _board;

        public Renderer(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            _tileRenderer = new TileRenderer(Size, _shader);

            _tileRenderer.LoadVertexBuffers();

            _view = Matrix4.CreateTranslation(Size.X / 8, Size.Y / 8, -0.0005f);

            _projection = Matrix4.CreateOrthographicOffCenter(0.0f, Size.X, 0.0f, Size.Y, -0.1f, 1.0f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();

            _shader.SetMatrix4("view", _view);
            _shader.SetMatrix4("projection", _projection);

            _board = _tileRenderer.RenderBoard();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keyboardInput = KeyboardState;

            if (keyboardInput.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            var mousePosition = MouseState.Position;

            var flippedY = -1 * mousePosition.Y + Size.Y;

            var tileUnit = new Vector2(Size.X / 8.0f, Size.Y / 8.0f);

            var pos = new Vector2((int)(mousePosition.X / tileUnit.X + 1), (int)(flippedY / tileUnit.Y + 1));

            if (pos.X >= 1 && pos.X <= 8 && pos.Y >= 1 && pos.Y <= 8)
            {
                if (MouseState.IsButtonPressed(MouseButton.Left))
                {
                    Console.WriteLine(pos.X + " " + pos.Y);
                }
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            _tileRenderer.ResizeBoard(Size);

            _view = Matrix4.CreateTranslation(Size.X / 8, Size.Y / 8, -0.0005f);

            _projection = Matrix4.CreateOrthographicOffCenter(0.0f, Size.X, 0.0f, Size.Y, -0.1f, 1.0f);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}