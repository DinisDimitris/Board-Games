using System;
using Structures;
using Renderers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Engine.Window
{
    public sealed class Window : GameWindow
    {
        private static Renderer _renderer;
        private static bool _moveChosen = false;

        private static Vector4 _clickColor = new Vector4(0.9f, 0.6f, 0.4f, 1.0f);
        private static Tile _tempTile;
        private static Tile[,] _board;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _renderer = new Renderer(Size);

            _renderer.LoadVertexBuffers();

            _board = Board.GenerateBoard();

            _board[5,5].Piece = new Piece(new Vector4(0,1,0,1));

            _tempTile = new Tile(new Vector2(1,1), new Vector4(1,1,1,1), "test.png");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            _renderer.UseShader();
            _renderer.SetView(Matrix4.CreateTranslation(Size.X / 8, Size.Y / 8, -0.0005f));
            _renderer.SetProjection(Matrix4.CreateOrthographicOffCenter(0.0f, Size.X, 0.0f, Size.Y, -0.1f, 1.0f));

            _renderer.RenderBoard(_board);

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
                    if (!_moveChosen)
                    {
                        _tempTile.Identity = _board[x,y].Identity;
                        _tempTile.Color = _board[x, y].Color;

                        _board[x, y].Color = _clickColor;

                        _moveChosen = true;
                    }
                    else
                    {
                        _board[(int)_tempTile.Identity.X, (int)_tempTile.Identity.Y].Color = _tempTile.Color;
                        _moveChosen = false;
                    }

                    _renderer.SetTileColours(_board[x, y].Color);
                    Console.WriteLine(mousePositionOnGameScreen.X + " " + mousePositionOnGameScreen.Y);
                }
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            _renderer.ResizeBoard(Size);

            _renderer.SetView(Matrix4.CreateTranslation(Size.X / 8, Size.Y / 8, -0.0005f));
            _renderer.SetProjection(Matrix4.CreateOrthographicOffCenter(0.0f, Size.X, 0.0f, Size.Y, -0.1f, 1.0f));

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}