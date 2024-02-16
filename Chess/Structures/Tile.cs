using OpenTK.Mathematics;

namespace Structures
{
    public sealed class Tile
    {
        public Vector2 Identity;
        public Vector4 Color;

        public Piece Piece {get; set;}
        public string TexturePath { get; internal set; }

        public Tile(Vector2 identity, Vector4 color, string texturePath)
        {
            Color = color;
            Identity = identity;
            TexturePath = texturePath;
        }
    }

}