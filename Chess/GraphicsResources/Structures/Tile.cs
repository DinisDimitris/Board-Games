using OpenTK.Mathematics;

namespace Structures
{
    public class Tile
    {
        public Matrix4 _identity;
        public Tile(Matrix4 identity)
        {
            _identity = identity;
        }
    }

}