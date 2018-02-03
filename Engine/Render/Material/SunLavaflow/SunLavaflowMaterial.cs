using System;
using Engine.Model;
using Engine.Render;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public class SunLavaflowMaterial : BaseMaterial {
		private readonly int modelviewProjectionMatrixLocation;

		public SunLavaflowMaterial() {
			CreateShaderProgram("Render/Material/SunLavaflow/SunLavaflow_VS.glsl",
				"Render/Material/SunLavaflow/SunLavaflow_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			GL.LinkProgram(Program);

			modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");
		}

		protected override void PreDraw() => GL.UseProgram(Program);
		protected override void PostDraw() => GL.BindVertexArray(0);

		protected override void Draw(Model3D model, MaterialSettings materialSettings) {
			GL.BindVertexArray(model.VAO);
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.ColorTexture);

			var modelviewProjection = (model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection).ToFloat();
			GL.UniformMatrix4(modelviewProjectionMatrixLocation, false, ref modelviewProjection);

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}
	}
}