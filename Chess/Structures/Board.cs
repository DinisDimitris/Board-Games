using System;
using System.Collections.Generic;
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
                {new Vector2(0, 0), "rook0.png" }, // Rook
                {new Vector2(1, 0), "knight0.png" }, // Knight
                {new Vector2(2, 0), "bishop0.png" }, // Bishop
                {new Vector2(3, 0), "queen0.png" }, // Queen
                {new Vector2(4, 0), "king0.png" }, // King
                {new Vector2(5, 0), "bishop0.png" }, // Bishop
                {new Vector2(6, 0), "knight0.png" }, // Knight
                {new Vector2(7, 0), "rook0.png" }, // Rook
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
                startingPositions.Add(new Vector2(i, 1), "pawn0.png");
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
                legalMoves = PieceHelper.GetPawnMoves(_board, position, forwardDirection);

            if (texture.Contains("rook"))
                legalMoves = PieceHelper.GetRookMoves(_board, position);

            if (texture.Contains("knight"))
                legalMoves = PieceHelper.GetKnightMoves(_board, position);

            if (texture.Contains("queen"))
                legalMoves = PieceHelper.GetQueenMoves(_board, position);

            if (texture.Contains("king"))
                legalMoves = PieceHelper.GetKingMoves(_board, position);

            if (texture.Contains("bishop"))
                legalMoves = PieceHelper.GetBishopMoves(_board, position);

            return legalMoves;
        }

        public static bool IsOpponentInCheck(Tile[,] board, string suffix)
        {
            var opponentKingTexture = suffix == "1" ? "king1" : "king0";

            var pieceMoveCheckCount = 0;

            // Check if any of the player's pieces can attack the opponent's king
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j].Texture != "" && !board[i, j].Texture.Contains(suffix))
                    {
                        var legalMoves = GetLegalMoves(board, board[i, j]);
                        var attackingMoves = legalMoves[1];

                        foreach (var move in attackingMoves)
                        {
                            Console.WriteLine($"attack by {board[i,j].Texture} at {move.X}, {move.Y} on piece : {board[(int)move.X, (int)move.Y].Texture}");
                            if (board[(int)move.X, (int)move.Y].Texture.Contains(opponentKingTexture)){
                                return true;
                            }
                            }
                        }
                        pieceMoveCheckCount += 1;
                    }
                }

                return false;
            }
            
        }
}