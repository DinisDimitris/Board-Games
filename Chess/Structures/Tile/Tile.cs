using OpenTK.Mathematics;

namespace Structures.Tiles
{
    public class Tile
    {
        public Matrix4 Identity;

        public Vector4 Color;
        public Tile(Matrix4 identity, Vector4 color)
        {
            Color = color;
            Identity = identity;
        }
    }

}