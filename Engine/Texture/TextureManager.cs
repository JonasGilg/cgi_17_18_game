using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Engine.Texture {
	public static class TextureManager {
		public static int LoadTexture(string fullAssetPath) {
			var returnTextureId = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, returnTextureId);

			var bmp = new Bitmap(fullAssetPath);

			var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Nearest);

			bmp.UnlockBits(bmpData);
			bmp.Dispose();

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			GL.BindTexture(TextureTarget.TextureCubeMap, 0);

			return returnTextureId;
		}

		public static int LoadCubemap(string[] faces) {
			var textureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.TextureCubeMap, textureID);

			for (var i = 0; i < faces.Length; i++) {
				var bmp = new Bitmap(faces[i]);
				var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
					PixelFormat.Format32bppArgb);
				bmp.Dispose();

				GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height,
					0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
			}

			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Linear);

			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int) TextureWrapMode.ClampToEdge);

			GL.BindTexture(TextureTarget.TextureCubeMap, 0);

			return textureID;
		}
	}
}