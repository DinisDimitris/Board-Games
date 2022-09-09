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
    public class Window : GameWindow
    {
        private TileRenderer _tileRenderer;
        private Tile[,] _board;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _tileRenderer = new TileRenderer(Size);

            _tileRenderer.LoadVertexBuffers();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            _tileRenderer.LoadShader();
            _tileRenderer.SetView(Matrix4.CreateTranslation(Size.X / 8, Size.Y / 8, -0.0005f));
            _tileRenderer.SetProjection(Matrix4.CreateOrthographicOffCenter(0.0f, Size.X, 0.0f, Size.Y, -0.1f, 1.0f));

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

            var mousePositionOnGameScreen = new Vector2((int)(mousePosition.X / tileUnit.X), (int)(flippedY / tileUnit.Y));

            var x = (int)mousePositionOnGameScreen.X;
            var y = (int)mousePositionOnGameScreen.Y;

            if (x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                if (MouseState.IsButtonPressed(MouseButton.Left))
                {
                    _board[x,y].Color = new Vector4(1,1,1,1);
                    _tileRenderer.SetTileColour(_board[x,y].Color);

                    Console.WriteLine(mousePositionOnGameScreen.X + " " + mousePositionOnGameScreen.Y);
                }
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            _tileRenderer.ResizeBoard(Size);

            _tileRenderer.SetView(Matrix4.CreateTranslation(Size.X / 8, Size.Y / 8, -0.0005f));
            _tileRenderer.SetProjection(Matrix4.CreateOrthographicOffCenter(0.0f, Size.X, 0.0f, Size.Y, -0.1f, 1.0f));

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}