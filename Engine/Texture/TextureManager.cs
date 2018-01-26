using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
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

			var bmpData = new BitmapData[faces.Length];

			Parallel.For(0, faces.Length, i => {
				var bmp = new Bitmap(faces[i]);
				bmpData[i] = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
					PixelFormat.Format32bppArgb);
				bmp.Dispose();
			});

			for (var i = 0; i < faces.Length; i++) {
				GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, bmpData[i].Width, bmpData[i].Height,
					0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData[i].Scan0);
			}

			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Linear);

			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int) TextureWrapMode.ClampToEdge);

			GL.BindTexture(TextureTarget.TextureCubeMap, 0);

			return textureID;
		}

		public static int LoadIBLSpecularMap(string baseName, string fileType) {
			var textureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.TextureCubeMap, textureID);

			const int numTextures = 9 * 6;
			
			var bmpData = new BitmapData[numTextures];

			Parallel.For(0, numTextures, l => {
				var i = l / 6;
				var o = l % 6;

				var fileName = baseName + "_m0" + i.ToString() + "_c0" + o.ToString() + "." + fileType;
				var bmp = new Bitmap(fileName);
				bmpData[l] = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			});

			for (var l = 0; l < numTextures; l++) {
				var i = l / 6;
				var o = l % 6;
				GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + o, i, PixelInternalFormat.Rgba, bmpData[l].Width, bmpData[l].Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData[l].Scan0);
			}

			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.LinearMipmapLinear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Linear);

			//glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
			//glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int) TextureWrapMode.ClampToEdge);

			GL.BindTexture(TextureTarget.TextureCubeMap, 0);

			return textureID;
		}


		public static int LoadIBLIrradianceMap(string baseName, string fileType) {
			var textureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.TextureCubeMap, textureID);

			var bmpData = new BitmapData[6];

			Parallel.For(0, 6, i => {
				var fileName = baseName + "_c0" + i.ToString() + "." + fileType;
				var bmp = new Bitmap(fileName);
				bmpData[i] = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			});

			for (var i = 0; i < 6; i++) {
				GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, bmpData[i].Width, bmpData[i].Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData[i].Scan0);
			}

			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Linear);

			//glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
			//glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int) TextureWrapMode.ClampToEdge);

			GL.BindTexture(TextureTarget.TextureCubeMap, 0);

			return textureID;
		}
	}
}