using System;
using Structures;
using Renderers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Window
{
    public sealed class Window : GameWindow
    {
        private static Renderer _renderer;
        private static bool _moveChosen = false;

        private static Vector4 _clickColor = new Vector4(0.9f, 0.6f, 0.4f, 1.0f);
        private static Vector4 _attackColor = new Vector4(1.0f, 0.0f, 0.0f, 0.1f);
        private static Tile _tempColorTile;
        private static List<Vector2> _tempMoveTiles;
        private static List<Vector2> _tempAttackTiles;
        private static Tile[,] _board;

        private const string DOT_TEXTURE = "Textures/dot.png";

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

            _tempColorTile = new Tile(new Vector2(1,1), new Vector4(1,1,1,1), "test.png");

            _tempMoveTiles = new List<Vector2>();

            _tempAttackTiles = new List<Vector2>();
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
                    var legalMoves = Board.GetLegalMoves(_board, _board[x,y]);

                    if (!_moveChosen && _board[x,y].Texture != "")
                    {
                        _tempColorTile.Identity = _board[x,y].Identity;
                        _tempColorTile.Color = _board[x, y].Color;

                        var nonAttackingMoves = legalMoves[0];
                        var attackingMoves = legalMoves[1];

                        foreach(var legalMove in nonAttackingMoves)
                        {
                            _board[(int)legalMove.X,(int)legalMove.Y].Texture = DOT_TEXTURE;
                            _tempMoveTiles.Add(legalMove);
                        }

                        foreach(var legalMove in attackingMoves)
                        {
                             _board[(int)legalMove.X,(int)legalMove.Y].Color = _attackColor;
                             _tempAttackTiles.Add(legalMove);

                        }
                        _board[x, y].Color = _clickColor;

                        _moveChosen = true;
                    }
                    else
                    {
                        foreach(var tempMoveTile in _tempMoveTiles)
                        {
                            _board[(int)tempMoveTile.X, (int)tempMoveTile.Y].Texture = "";
                        }

                        foreach(var tempAttackTile in _tempAttackTiles)
                        {
                            _board[(int)tempAttackTile.X, (int)tempAttackTile.Y].Color = _tempColorTile.Color;
                        }

                        var mouseHoveringPos = new Vector2(x,y);

                        if (_tempMoveTiles.Contains(mouseHoveringPos) || _tempAttackTiles.Contains(mouseHoveringPos) )
                        {
                            _board[(int)mouseHoveringPos.X, (int) mouseHoveringPos.Y].Texture = _board[(int)_tempColorTile.Identity.X, (int)_tempColorTile.Identity.Y].Texture;

                            _board[(int)_tempColorTile.Identity.X, (int)_tempColorTile.Identity.Y].Texture = "";
                        }

                        _board[(int)_tempColorTile.Identity.X, (int)_tempColorTile.Identity.Y].Color = _tempColorTile.Color;
                        _moveChosen = false;



                        _tempMoveTiles = new List<Vector2>();
                        _tempAttackTiles = new List<Vector2>();
                    }

                    _renderer.SetTileColours(_board[x, y].Color);
                    //Console.WriteLine(mousePositionOnGameScreen.X + " " + mousePositionOnGameScreen.Y);
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