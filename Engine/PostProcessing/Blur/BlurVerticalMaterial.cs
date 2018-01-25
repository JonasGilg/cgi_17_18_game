using System;
using Engine.Model;
using OpenTK.Graphics.OpenGL;
using Engine.Util;

namespace Engine.Postprocessing {
	public class BlurVerticalMaterial : BasePostprocessing {
		private readonly int samplerLocation;

		public BlurVerticalMaterial() {
			Program = ShaderCompiler.CreateShaderProgram("PostProcessing/Blur/BlurVertical_VS.glsl", "PostProcessing/Blur/BlurVertical_FS.glsl");

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