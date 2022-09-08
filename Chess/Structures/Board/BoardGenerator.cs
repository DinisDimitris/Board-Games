using Structures.Tiles;
using OpenTK.Mathematics;

namespace Structures.Grid
{
    public static class GridGenerator
    {
        public static Tile[,] GenerateBySize(int rows, int columns)
        {

            var board = new Tile[rows, columns];

            var position = Matrix4.Identity;

            Vector4 color;

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    position = Matrix4.CreateTranslation(x, y, 0);
                    //whites
                    if ((y % 2 == 0 && x % 2 == 0) | (y % 2 == 1 && x % 2 == 1))
                    {
                        color = new Vector4(1, 1, 1,1);
                    }
                    else
                    {
                        color = new Vector4(0, 0, 0, 0);
                    }

                    board[x, y] = new Tile(position, color);
                }
            }

            return board;
        }
    }
}