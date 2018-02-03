using System;
using Engine.Model;
using Engine.Render;
using Engine.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public class BlackHoleMaterial : BaseMaterial {
		private readonly int modelviewProjectionMatrixLocation;
		private readonly int modelMatrixLocation;
		private readonly int cameraPositionLocation;

		public BlackHoleMaterial() {
			CreateShaderProgram("Render/Material/BlackHole/BlackHole_VS.glsl",
				"Render/Material/BlackHole/BlackHole_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			GL.LinkProgram(Program);

			modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");
			modelMatrixLocation = GL.GetUniformLocation(Program, "model_matrix");
			cameraPositionLocation = GL.GetUniformLocation(Program, "camera_pos");
		}

		protected override void PreDraw() => GL.UseProgram(Program);

		protected override void PostDraw() => GL.BindVertexArray(0);

		protected override void Draw(Model3D model, MaterialSettings materialSettings) {
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.ColorTexture);
			GL.BindVertexArray(model.VAO);

			var modelviewProjection = (model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection).ToFloat();
			GL.UniformMatrix4(modelviewProjectionMatrixLocation, false, ref modelviewProjection);
			var modelmatrix = model.Transformation.ToFloat();
			GL.UniformMatrix4(modelMatrixLocation, false, ref modelmatrix);
			GL.Uniform3(cameraPositionLocation, DisplayCamera.Position.ToFloat());

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}
	}
}