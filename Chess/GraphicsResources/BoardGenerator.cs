using Structures;
using OpenTK.Mathematics;

namespace Board.Generator
{
    public static class BoardGenerator
    {

        public static Tile[,] GenerateTilesFrom(int rows, int columns)
        {

            var board = new Tile[rows, columns];

            var position = Matrix4.Identity;

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {

                    //whites
                    if ((y % 2 == 0 && x % 2 == 0) | (y % 2 == 1 && x % 2 == 1))
                    {

                        position = Matrix4.CreateTranslation(x, y, 0);
                        board[x, y] = new Tile(position);
                    }

                    else
                    {
                        position = Matrix4.CreateTranslation(x, y, 0);
                        board[x, y] = new Tile(position);
                    }
                }
            }

            return board;

        }
    }
}