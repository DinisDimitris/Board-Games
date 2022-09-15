using OpenTK.Mathematics;

namespace Structures
{
    public sealed class Piece
    {
        public Vector2 Identity;
        public Vector4 Color;
        public Piece(Vector2 identity, Vector4 color)
        {
            Color = color;
            Identity = identity;
        }
    }

}