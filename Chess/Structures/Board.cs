using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Structures
{
    public static class Board
    {
        public static Tile[,] GenerateBoard()
        {
            var board = new Tile[8, 8];

            Vector4 color;

            string texture = "";

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var position = new Vector2(x, y);

                    //whites
                    if ((y % 2 == 0 && x % 2 == 0) | (y % 2 == 1 && x % 2 == 1))
                    {

                        color = new Vector4(1, 1, 1, 1);

                        texture = "Textures/bishop1.png";

                    }
                    else
                    {
                       texture = "";
                        color = new Vector4(0, 0, 0, 0);
                    }

                    board[x, y] = new Tile(position, color, texture);
                }
            }

            return board;
        }

        public static List<Vector2> GetStartingPositionTextures()
        {
            return new List<Vector2>();

        }
    }
}