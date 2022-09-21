using System.IO;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Common
{
    public class Texture
    {
        private int _handle;
        public Texture(int Handle, string path)
        {
            _handle = GL.GenTexture();
            Use();

            // stb_image loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            // This will correct that, making the texture display properly.
            StbImage.stbi_set_flip_vertically_on_load(1);

            // Load the image.
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        }

        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }
    }
}