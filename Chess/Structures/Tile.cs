using OpenTK.Mathematics;

namespace Structures
{
    public sealed class Tile
    {
        public Vector2 Identity;
        public Vector4 Color;
        public string Texture { get; internal set; }

        public Tile(Vector2 identity, Vector4 color, string texture)
        {
            Color = color;
            Identity = identity;
            Texture = texture;
        }
    }

}