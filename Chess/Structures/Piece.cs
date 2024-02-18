using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Structures
{
    public static class PieceHelper
    {
        public static Dictionary<int, List<Vector2>> GetPawnMoves(Tile[,] _board, Vector2 position, int forwardDirection)
        {

            var nonAttackingMoves = new List<Vector2>();
            var attackingMoves = new List<Vector2>();

            int forwardX = (int)position.X;
            int forwardY = (int)position.Y + forwardDirection;

            if (IsInBoard(forwardX, forwardY) && _board[forwardX, forwardY].Texture == "")
            {
                nonAttackingMoves.Add(new Vector2(forwardX, forwardY));
            }

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
                var doubleForwardY = (int)position.Y + 2 * forwardDirection;
                var singleForwardY = (int)position.Y + 1 * forwardDirection;

                var blockedAhead = false;
                if (_board[forwardX, singleForwardY].Texture != "")
                {
                    blockedAhead = true;
                }

                if (_board[forwardX, doubleForwardY].Texture == "" && !blockedAhead)
                {
                    nonAttackingMoves.Add(new Vector2(forwardX, doubleForwardY));
                }
            }

            var dict = new Dictionary<int, List<Vector2>>{
                {0, nonAttackingMoves},
                {1, attackingMoves}
            };

            return dict;
        }
        public static Dictionary<int, List<Vector2>> GetRookMoves(Tile[,] _board, Vector2 position)
        {
            var legalMoves = new Dictionary<int, List<Vector2>>();

            legalMoves[0] = new List<Vector2>(); 
            legalMoves[1] = new List<Vector2>(); 

            // Check moves in all four directions: up, down, left, right
            CheckDirection(_board, position, 1, 0, legalMoves); // Up
            CheckDirection(_board, position, -1, 0, legalMoves); // Down
            CheckDirection(_board, position, 0, 1, legalMoves); // Right
            CheckDirection(_board, position, 0, -1, legalMoves); // Left

            return legalMoves;
        }

        public static Dictionary<int, List<Vector2>> GetBishopMoves(Tile[,] _board, Vector2 position)
        {
            var legalMoves = new Dictionary<int, List<Vector2>>();

            legalMoves[0] = new List<Vector2>(); 
            legalMoves[1] = new List<Vector2>(); 

            CheckDirection(_board, position, 1, 1, legalMoves); // Up-Right
            CheckDirection(_board, position, 1, -1, legalMoves); // Up-Left
            CheckDirection(_board, position, -1, 1, legalMoves); // Down-Right
            CheckDirection(_board, position, -1, -1, legalMoves); // Down-Left

            return legalMoves;
        }

        public static Dictionary<int, List<Vector2>> GetQueenMoves(Tile[,] _board, Vector2 position)
        {
            var legalMoves = new Dictionary<int, List<Vector2>>();

            legalMoves[0] = new List<Vector2>();
            legalMoves[1] = new List<Vector2>();

            // Combine rook and bishop moves
            legalMoves = CombineMoves(legalMoves, GetRookMoves(_board, position));
            legalMoves = CombineMoves(legalMoves, GetBishopMoves(_board, position));

            return legalMoves;
        }

        public static Dictionary<int, List<Vector2>> GetKingMoves(Tile[,] _board, Vector2 position)
        {
            var legalMoves = new Dictionary<int, List<Vector2>>();

            legalMoves[0] = new List<Vector2>();
            legalMoves[1] = new List<Vector2>();

            // Check all adjacent squares
            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newX = (int)position.X + dx[i];
                int newY = (int)position.Y + dy[i];

                if (IsInBoard(newX, newY))
                {
                    if (_board[newX, newY].Texture == "")
                    {
                        legalMoves[0].Add(new Vector2(newX, newY));
                    }
                    else
                    {
                        if (_board[newX, newY].Texture.Contains("1") != _board[(int)position.X, (int)position.Y].Texture.Contains("1"))
                        {
                            legalMoves[1].Add(new Vector2(newX, newY)); 
                        }
                    }
                }
            }

            return legalMoves;
        }

        private static void CheckDirection(Tile[,] _board, Vector2 position, int dx, int dy, Dictionary<int, List<Vector2>> legalMoves)
        {
            int x = (int)position.X + dx;
            int y = (int)position.Y + dy;

            while (IsInBoard(x, y))
            {
                if (_board[x, y].Texture == "")
                {
                    legalMoves[0].Add(new Vector2(x, y));
                }
                else
                {
                    if (_board[x, y].Texture.Contains("1") != _board[(int)position.X, (int)position.Y].Texture.Contains("1"))
                    {
                        legalMoves[1].Add(new Vector2(x, y)); 
                    }
                    break; // Stop further movement in this direction
                }

                x += dx;
                y += dy;
            }
        }

        private static Dictionary<int, List<Vector2>> CombineMoves(Dictionary<int, List<Vector2>> moves1, Dictionary<int, List<Vector2>> moves2)
        {
            Dictionary<int, List<Vector2>> combinedMoves = new Dictionary<int, List<Vector2>>();

            combinedMoves[0] = new List<Vector2>(moves1[0]);
            combinedMoves[1] = new List<Vector2>(moves1[1]);

            combinedMoves[0].AddRange(moves2[0]);
            combinedMoves[1].AddRange(moves2[1]);

            return combinedMoves;
        }

        public static Dictionary<int, List<Vector2>> GetKnightMoves(Tile[,] _board, Vector2 position)
        {
            var legalMoves = new Dictionary<int, List<Vector2>>();

            int[] dx = { 1, 1, 2, 2, -1, -1, -2, -2 };
            int[] dy = { 2, -2, 1, -1, 2, -2, 1, -1 };

            legalMoves[0] = new List<Vector2>(); 
            legalMoves[1] = new List<Vector2>(); 

            for (int i = 0; i < 8; i++)
            {
                int newX = (int)position.X + dx[i];
                int newY = (int)position.Y + dy[i];

                if (IsInBoard(newX, newY))
                {
                    if (_board[newX, newY].Texture == "")
                    {
                        legalMoves[0].Add(new Vector2(newX, newY));
                    }
                    else
                    {
                        if (_board[newX, newY].Texture.Contains("1") != _board[(int)position.X, (int)position.Y].Texture.Contains("1"))
                        {
                            legalMoves[1].Add(new Vector2(newX, newY)); 
                        }
                    }
                }
            }

            return legalMoves;
        }

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