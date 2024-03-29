﻿using System;
using Engine.Model;
using Engine.Texture;
using OpenTK.Graphics.OpenGL;

namespace Engine.Render.Skybox {
	public static class Skybox {
		private static readonly int PROGRAMM;
		private static readonly int SKY_BOX_TEXTURE;
		private static readonly int VIEW_PROJECTION_LOCATION;

		private static readonly Model3D SKYBOX_MODEL;

		private static readonly TimingStats RENDER_TIMING = new TimingStats("Skybox");

		static Skybox() {
			TimingRegistry.AddRenderTiming(RENDER_TIMING);

			SKYBOX_MODEL = ModelLoaderObject3D.Load("data/objects/Skybox.obj");
			PROGRAMM = ShaderLoader.LoadShader("Render/Skybox/Skybox_VS.glsl", "Render/Skybox/Skybox_FS.glsl");
			const string debugBox = "";
			SKY_BOX_TEXTURE = TextureManager.LoadCubemap(new[] {
				"data/textures/skybox/bright/" + debugBox + "skybox_bright_right1.png",
				"data/textures/skybox/bright/" + debugBox + "skybox_bright_left2.png",
				"data/textures/skybox/bright/" + debugBox + "skybox_bright_top3.png",
				"data/textures/skybox/bright/" + debugBox + "skybox_bright_bottom4.png",
				"data/textures/skybox/bright/" + debugBox + "skybox_bright_front5.png",
				"data/textures/skybox/bright/" + debugBox + "skybox_bright_back6.png"
			});

			GL.LinkProgram(PROGRAMM);

			VIEW_PROJECTION_LOCATION = GL.GetUniformLocation(PROGRAMM, "viewProjection");
		}

		public static void Draw() {
			RENDER_TIMING.Start();

			GL.UseProgram(PROGRAMM);

			var perspectiveProjection =
				(DisplayCamera.Transformation.ClearTranslation() * DisplayCamera.PerspectiveProjection).ToFloat();
			GL.UniformMatrix4(VIEW_PROJECTION_LOCATION, false, ref perspectiveProjection);

			GL.BindVertexArray(SKYBOX_MODEL.VAO);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.TextureCubeMap, SKY_BOX_TEXTURE);

			GL.DepthFunc(DepthFunction.Lequal);
			GL.DrawElements(PrimitiveType.Triangles, SKYBOX_MODEL.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
			GL.DepthFunc(DepthFunction.Less);

			GL.BindVertexArray(0);

			RENDER_TIMING.Stop();
		}
	}
}