using System;
using Engine.Model;
using Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Postprocessing {
	public class BlurFullscreenMaterial : BasePostprocessing {
		private readonly int shiftLocation;
		private readonly int targetLocation;

		public BlurFullscreenMaterial() {
			Program = ShaderLoader.LoadShader("PostProcessing/Blur/BlurFullscreen_VS.glsl", "PostProcessing/Blur/BlurFullscreen_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			GL.LinkProgram(Program);

			shiftLocation = GL.GetUniformLocation(Program, "shift");
			targetLocation = GL.GetUniformLocation(Program, "target");
		}


		public void Draw(Model3D model, int textureID, int target, float xShift, float yShift) {
			GL.BindTexture(TextureTarget.Texture2D, textureID);

			GL.BindVertexArray(model.VAO);

			GL.UseProgram(Program);

			GL.Uniform2(shiftLocation, new Vector2(xShift, yShift));
			GL.Uniform1(targetLocation, target);

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindVertexArray(0);
		}
	}
}