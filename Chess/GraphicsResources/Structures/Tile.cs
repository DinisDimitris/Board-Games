using OpenTK.Mathematics;

namespace Structures
{
    public class Tile
    {
        public Matrix4 Identity;
        public Tile(Matrix4 identity)
        {
            Identity = identity;
        }
    }

}