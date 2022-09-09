using OpenTK.Mathematics;

namespace Structures
{
    public class Tile
    {
        public Vector2 Identity;

        public Vector4 Color;
        public Tile(Vector2 identity, Vector4 color)
        {
            Color = color;
            Identity = identity;
        }
    }

}