using System;
using Common;
using Structures.Grid;
using Structures.Tiles;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Engine.Render

{
    public class Renderer : GameWindow
    {
        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;
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

            var vertices = new float[]
            {
                Size.X / 8.0f,  Size.Y / 8.0f, 0.0f, // top right
                Size.X / 8.0f, -Size.Y / 8.0f, 0.0f, // bottom right
                -Size.X / 8.0f, -Size.Y / 8.0f, 0.0f, // bottom left
                -Size.X / 8.0f,  Size.Y / 8.0f, 0.0f, // top left
            };

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            _view = Matrix4.CreateTranslation(Size.X / 8 , Size.Y / 8, -0.0005f);

            _projection = Matrix4.CreateOrthographicOffCenter(0.0f, Size.X, 0.0f, Size.Y , -0.1f, 1.0f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);


            _shader.Use();

            GL.BindVertexArray(_vertexArrayObject);
            _shader.SetMatrix4("view", _view);
            _shader.SetMatrix4("projection", _projection);

            var boardModel = GridGenerator.GenerateBySize(8, 8);

            RenderFromBoardModel(boardModel);

            SwapBuffers();
        }

        public void RenderFromBoardModel(Tile[,] board)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    _shader.SetVector3("offset", new Vector3(x * Size.X / 8,y * Size.Y / 8, 1));
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

            var tileUnit = new Vector2(Size.X / 8.0f, Size.Y / 8.0f);

            var mousePositionPerTile = new Vector2(MouseState.Position.X / tileUnit.X, flippedY / tileUnit.Y);
            Console.WriteLine((mousePositionPerTile.X + " " + mousePositionPerTile.Y));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            var vertices = new float[]
            {
                Size.X / 8.0f,  Size.Y / 8.0f, 0.0f, // top right
                Size.X / 8.0f, -Size.Y / 8.0f, 0.0f, // bottom right
                -Size.X / 8.0f, -Size.Y / 8.0f, 0.0f, // bottom left
                -Size.X / 8.0f,  Size.Y / 8.0f, 0.0f, // top left
            };

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);


            _view = Matrix4.CreateTranslation(Size.X / 8 , Size.Y / 8, -0.0005f);

            _projection = Matrix4.CreateOrthographicOffCenter(0.0f, Size.X, 0.0f, Size.Y , -0.1f, 1.0f);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}