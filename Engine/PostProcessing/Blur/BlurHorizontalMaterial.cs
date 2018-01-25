using System;
using Engine.Model;
using OpenTK.Graphics.OpenGL;
using Engine.Util;

namespace Engine.Postprocessing {
	public class BlurHorizontalMaterial : BasePostprocessing {
		private readonly int samplerLocation;

		public BlurHorizontalMaterial() {
			Program = ShaderCompiler.CreateShaderProgram("PostProcessing/Blur/BlurHorizontal_VS.glsl", "PostProcessing/Blur/BlurHorizontal_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			GL.LinkProgram(Program);

			samplerLocation = GL.GetUniformLocation(Program, "sampler");

			//shiftLocation  = GL.GetUniformLocation(Program, "shift");
			//targetLocation = GL.GetUniformLocation(Program, "target");
		}

		public void Draw(Model3D model, int textureID) {
			// Textur wird "gebunden"
			//GL.BindTexture(TextureTarget.Texture2D, textureID);

			GL.BindVertexArray(model.VAO);

			GL.UseProgram(Program);

			GL.Uniform1(samplerLocation, 0);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, textureID);

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindVertexArray(0);
		}
	}
}