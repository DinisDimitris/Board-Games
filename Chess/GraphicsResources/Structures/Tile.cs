using OpenTK.Mathematics;

namespace Structures
{
    public class Tile
    {
        public Matrix4 Identity;

        public Vector3 Color;
        public Tile(Matrix4 identity, Vector3 color)
        {
            Color = color;
            Identity = identity;
        }
    }

}