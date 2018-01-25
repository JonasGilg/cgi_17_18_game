using System;
using Engine.Model;
using Engine.Util;
using OpenTK.Graphics.OpenGL;

namespace Engine {
	public class SimpleLighting : BaseLighting {
		public SimpleLighting() {
			Program = ShaderCompiler.CreateShaderProgram("Light/Simple/Simple_VS.glsl",
				"Light/Simple/Simple_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			GL.LinkProgram(Program);
		}


		public void Draw(Model3D model, int textureID) {
			GL.UseProgram(Program);

			GL.BindVertexArray(model.VAO);

			GL.BindTexture(TextureTarget.Texture2D, textureID);

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindVertexArray(0);
		}
	}
}