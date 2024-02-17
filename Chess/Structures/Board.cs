using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using OpenTK.Mathematics;

namespace Structures
{
    public static class Board
    {
        public static Tile[,] GenerateBoard()
        {
            var board = new Tile[8, 8];

            var textCordsByTexturePath = GetStartingPositionTextures();

            Vector4 color;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var position = new Vector2(x, y);

                    //whites
                    if ((y % 2 == 0 && x % 2 == 0) | (y % 2 == 1 && x % 2 == 1))
                    {
                        color = new Vector4(1, 1, 1, 1);
                    }
                    else
                    {
                        color = new Vector4(0, 0, 0, 0);
                    }

                    string texture = "";
                    if (textCordsByTexturePath.TryGetValue(position, out string textPath))
                        texture = "Textures/" + textPath;

                    board[x, y] = new Tile(position, color, texture);
                }
            }

            return board;
        }

        public static Dictionary<Vector2, string> GetStartingPositionTextures()
        {
            Dictionary<Vector2, string> startingPositions = new Dictionary<Vector2, string>
            {
                {new Vector2(0, 0), "rook.png" }, // Rook
                {new Vector2(1, 0), "knight.png" }, // Knight
                {new Vector2(2, 0), "bishop.png" }, // Bishop
                {new Vector2(3, 0), "queen.png" }, // Queen
                {new Vector2(4, 0), "king.png" }, // King
                {new Vector2(5, 0), "bishop.png" }, // Bishop
                {new Vector2(6, 0), "knight.png" }, // Knight
                {new Vector2(7, 0), "rook.png" }, // Rook
                {new Vector2(0, 7), "rook1.png"},
                {new Vector2(1, 7), "knight1.png"},
                {new Vector2(2, 7), "bishop1.png"},
                {new Vector2(3, 7), "queen1.png"},
                {new Vector2(4, 7), "king1.png"},
                {new Vector2(5, 7), "bishop1.png"},
                {new Vector2(6, 7), "knight1.png"},
                {new Vector2(7, 7), "rook1.png"}
            };
            for (int i = 0; i < 8; i++) // White Pawns
            {
                startingPositions.Add(new Vector2(i, 1), "pawn.png");
            }

            for (int i = 0; i < 8; i++) // Black Pawns
            {
                startingPositions.Add(new Vector2(i, 6), "pawn1.png");
            }

            return startingPositions;
        }
        
        public static List<Vector2> GetLegalMoves(Tile[,] _board, Tile tile)
        {
            List<Vector2> legalMoves = new List<Vector2>();

            var texture = tile.Texture;
            var position = tile.Identity;

            int forwardDirection = 1; // Assuming positive y-direction is forward for White pieces
            if (texture.Contains("1"))
            {
                forwardDirection = -1; // Reverse direction for Black pieces
            }

            // Check if the tile in front of the pawn is empty
            int forwardX = (int)position.X;
            int forwardY = (int)position.Y + forwardDirection;
            if (forwardY >= 0 && forwardY < _board.GetLength(1) && _board[forwardX, forwardY].Texture == "")
            {
                legalMoves.Add(new Vector2(forwardX, forwardY));
            }

            if (IsStartingPosition(position, texture))
            {
                int doubleForwardY = (int)position.Y + 2 * forwardDirection;
                if (doubleForwardY >= 0 && doubleForwardY < _board.GetLength(1) && _board[forwardX, doubleForwardY].Texture == "")
                {
                    legalMoves.Add(new Vector2(forwardX, doubleForwardY));
                }
            }

            // TODO: Add logic for capturing opponent's pieces diagonally (if applicable)

            return legalMoves;
        }   

    private static bool IsStartingPosition(Vector2 position, string texture)
    {
        return (texture.Contains("pawn") && position.Y == 1) || (texture.Contains("pawn1") && position.Y == 6);
    }
    }
}