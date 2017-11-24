using System;
using Engine.Model;
using Engine.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public class SunLavaflowMaterial : BaseMaterial {
		private readonly int modelviewProjectionMatrixLocation;
		private readonly int timeLocation;

		public SunLavaflowMaterial() {
			// Shader-Programm wird aus den externen Files generiert...
			CreateShaderProgram("Render/Material/SunLavaflow/SunLavaflow_VS.glsl",
				"Render/Material/SunLavaflow/SunLavaflow_FS.glsl");

			// GL.BindAttribLocation, gibt an welcher Index in unserer Datenstruktur welchem "in" Parameter auf unserem Shader zugeordnet wird
			// folgende Befehle müssen aufgerufen werden...
			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			// ...bevor das Shader-Programm "gelinkt" wird.
			GL.LinkProgram(Program);

			// Die Stelle an der im Shader der per "uniform" der Input-Paremeter "modelview_projection_matrix" definiert wird, wird ermittelt.
			modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");

			timeLocation = GL.GetUniformLocation(Program, "time");
		}

		protected override void PreDraw() {
			GL.UseProgram(Program);
			
			GL.Uniform1(timeLocation, (float) Time.TotalTime);
		}

		protected override void PostDraw() {
			GL.BindVertexArray(0);
		}

		protected override void Draw(Model3D model, MaterialSettings materialSettings) {
			GL.BindVertexArray(model.VAO);
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.ColorTexture);

			var modelviewProjection = (model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection).ToFloat();
			GL.UniformMatrix4(modelviewProjectionMatrixLocation, false, ref modelviewProjection);

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}
	}
}