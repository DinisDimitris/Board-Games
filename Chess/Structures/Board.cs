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
        public static Dictionary<int, List<Vector2>> GetLegalMoves(Tile[,] _board, Tile tile)
        {
            var texture = tile.Texture;
            var position = tile.Identity;

            int forwardDirection = 1; // Assuming positive y-direction is forward for White pieces
            if (texture.Contains("1"))
            {
                forwardDirection = -1; // Reverse direction for Black pieces
            }

            var legalMoves = new Dictionary<int, List<Vector2>>();
            if (texture.Contains("pawn"))
                legalMoves = GetPawnMoves(_board, position, forwardDirection);


            // Other pieces' legal moves
            if (texture.Contains("rook"))
            {
               // legalMoves.AddRange(GetRookMoves(_board, position));
            }
            else if (texture.Contains("knight"))
            {
                //legalMoves.AddRange(GetKnightMoves(_board, position));
            }
            // Implement logic for other pieces similarly

            return legalMoves;
        }


        private static Dictionary<int, List<Vector2>> GetPawnMoves(Tile[,] _board, Vector2 position, int forwardDirection){
            
            var nonAttackingMoves = new List<Vector2>(); 
            var attackingMoves = new List<Vector2>(); 

            int forwardX = (int)position.X;
            int forwardY = (int)position.Y + forwardDirection;

            // Forward movement
            if (IsInBoard(forwardX, forwardY) && _board[forwardX, forwardY].Texture == "")
            {
                nonAttackingMoves.Add(new Vector2(forwardX, forwardY));
            }

            // Diagonal capturing
            int leftDiagonalX = forwardX - 1;
            int rightDiagonalX = forwardX + 1;
            int diagonalY = forwardY;

            if (IsInBoard(leftDiagonalX, diagonalY))
            {
                if (_board[leftDiagonalX, diagonalY].Texture != "" && _board[leftDiagonalX, diagonalY].Texture.Contains("1") != forwardDirection.Equals(-1))
                {
                    attackingMoves.Add(new Vector2(leftDiagonalX, diagonalY));
                }
            }

            if (IsInBoard(rightDiagonalX, diagonalY))
            {
                if (_board[rightDiagonalX, diagonalY].Texture != "" && _board[rightDiagonalX, diagonalY].Texture.Contains("1") != forwardDirection.Equals(-1))
                {
                    attackingMoves.Add(new Vector2(rightDiagonalX, diagonalY));
                }
            }

            // Pawn's starting position double movement
            if (IsStartingPosition(position, forwardDirection))
            {
                int doubleForwardY = (int)position.Y + 2 * forwardDirection;
                if (IsInBoard(forwardX, doubleForwardY) && _board[forwardX, doubleForwardY].Texture == "")
                {
                    nonAttackingMoves.Add(new Vector2(forwardX, doubleForwardY));
                }
            }

            var dict = new Dictionary<int , List<Vector2>>{
                {0, nonAttackingMoves},
                {1, attackingMoves}
            };

            return dict;
        }

        private static List<Vector2> GetRookMoves(Tile[,] _board, Vector2 position)
        {
            List<Vector2> legalMoves = new List<Vector2>();
            // Implement rook legal moves logic
            return legalMoves;
        }

        private static List<Vector2> GetKnightMoves(Tile[,] _board, Vector2 position)
        {
            List<Vector2> legalMoves = new List<Vector2>();
            // Implement knight legal moves logic
            return legalMoves;
        }

        // Implement similar methods for other pieces like bishop, queen, and king

        private static bool IsInBoard(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }

        private static bool IsStartingPosition(Vector2 position, int forwardDirection)
        {
            return (forwardDirection == 1 && position.Y == 1) || (forwardDirection == -1 && position.Y == 6);
        }

    }
}