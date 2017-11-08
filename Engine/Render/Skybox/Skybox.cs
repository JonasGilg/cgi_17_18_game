using System;
using System.Drawing;
using System.Drawing.Imaging;
using Engine.Model;
using Engine.Texture;
using Engine.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Engine.Render.Skybox {
	public static class Skybox {
		private static readonly int Programm;
		private static readonly int SkyBoxTexture;
		private static readonly int ViewProjectionLocation;

		private static readonly Model3D SkyboxModel;

		private static readonly TimingStats RenderTiming = new TimingStats("Skybox");

		static Skybox() {
			TimingRegistry.AddRenderTiming(RenderTiming);
			
			SkyboxModel = ModelLoaderObject3D.Load("data/objects/Skybox.obj");
			Programm = ShaderLoader.LoadShader("Render/Skybox/Skybox_VS.glsl", "Render/Skybox/Skybox_FS.glsl");

			GL.ActiveTexture(TextureUnit.Texture0);
			SkyBoxTexture = GL.GenTexture();
			GL.BindTexture(TextureTarget.TextureCubeMap, SkyBoxTexture);

			LoadCubeMapSide("data/textures/skybox/skybox_right1.png", TextureTarget.TextureCubeMapPositiveX);
			LoadCubeMapSide("data/textures/skybox/skybox_left2.png", TextureTarget.TextureCubeMapNegativeX);
			LoadCubeMapSide("data/textures/skybox/skybox_top3.png", TextureTarget.TextureCubeMapPositiveY);
			LoadCubeMapSide("data/textures/skybox/skybox_bottom4.png", TextureTarget.TextureCubeMapNegativeY);
			LoadCubeMapSide("data/textures/skybox/skybox_front5.png", TextureTarget.TextureCubeMapPositiveZ);
			LoadCubeMapSide("data/textures/skybox/skybox_back6.png", TextureTarget.TextureCubeMapNegativeZ);
			
			GL.LinkProgram(Programm);
			
			ViewProjectionLocation = GL.GetUniformLocation(Programm, "viewProjection");
		}

		private static void LoadCubeMapSide(string path, TextureTarget sideTarget) {
			var bmp = new Bitmap(path);

			var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);

			GL.TexImage2D(sideTarget, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0, PixelFormat.Bgra,
				PixelType.UnsignedByte, bmpData.Scan0);
			
			bmp.UnlockBits(bmpData);
			bmp.Dispose();

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}

		public static void Draw() {
			RenderTiming.Start();
			
			GL.Disable(EnableCap.DepthTest);
			GL.DepthMask(false);

			GL.UseProgram(Programm);

			var perspectiveProjection = (DisplayCamera.Transformation.ClearTranslation() * DisplayCamera.PerspectiveProjection).ToFloat();
			GL.UniformMatrix4(ViewProjectionLocation, false, ref perspectiveProjection);
			
			GL.BindVertexArray(SkyboxModel.VAO);
			GL.BindTexture(TextureTarget.TextureCubeMap, SkyBoxTexture);

			GL.DepthFunc(DepthFunction.Lequal);
			GL.DrawElements(PrimitiveType.Triangles, SkyboxModel.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
			GL.DepthFunc(DepthFunction.Less);

			GL.DepthMask(true);
			GL.Enable(EnableCap.DepthTest);
			
			RenderTiming.Stop();
		}
	}
}