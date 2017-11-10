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

			SkyBoxTexture = TextureManager.LoadCubemap(new[] {
				"data/textures/skybox/skybox_right1.png",
				"data/textures/skybox/skybox_left2.png",
				"data/textures/skybox/skybox_top3.png",
				"data/textures/skybox/skybox_bottom4.png",
				"data/textures/skybox/skybox_front5.png",
				"data/textures/skybox/skybox_back6.png"
			});

			GL.LinkProgram(Programm);

			ViewProjectionLocation = GL.GetUniformLocation(Programm, "viewProjection");
		}

		public static void Draw() {
			RenderTiming.Start();

			GL.Disable(EnableCap.DepthTest);
			GL.DepthMask(false);

			GL.UseProgram(Programm);

			var perspectiveProjection =
				(DisplayCamera.Transformation.ClearTranslation() * DisplayCamera.PerspectiveProjection).ToFloat();
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