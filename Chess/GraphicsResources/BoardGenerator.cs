using Structures;
using OpenTK.Mathematics;

namespace Board.Generator
{
    public static class BoardGenerator
    {

        public static Tile[,] GenerateBySize(int rows, int columns)
        {

            var board = new Tile[rows, columns];

            var position = Matrix4.Identity;

            Vector3 color;

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    position = Matrix4.CreateTranslation(x, y, 0);
                    //whites
                    if ((y % 2 == 0 && x % 2 == 0) | (y % 2 == 1 && x % 2 == 1))
                    {
                        color = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        color = new Vector3(0, 0, 0);
                    }

                    board[x, y] = new Tile(position, color);
                }
            }

            return board;
        }
    }
}