using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using TearEngine.Engine.Objects;
using System.Security.Cryptography;

namespace TearEngine.Engine.Tools
{
    internal class Textures
    {
        static Dictionary<int, int> TextureCache = new Dictionary<int, int>();
        SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
        public static int CreateTexture(Bitmap bitmap)
        {
            int hash = bitmap.GetHashCode();

            if (TextureCache.ContainsKey(hash))
            {
                return TextureCache[hash];
            }
            else
            {
                GL.GenTextures(1, out int textureID);
                GL.BindTexture(TextureTarget.Texture2D, textureID);

                BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bitmap.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                TextureCache.Add(hash, textureID);
                return textureID;
            }
        }

        public static void RemoveTexture(int textureId)
        {
            GL.DeleteTexture(textureId);
        }
    }
}
