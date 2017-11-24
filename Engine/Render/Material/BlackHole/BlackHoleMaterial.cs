using System;
using Engine.Model;
using Engine.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public class BlackHoleMaterial : BaseMaterial {
		public int modelviewProjectionMatrixLocation;

		public int materialPositionLocation;
		public int materialRatioLocation;
		public int materialRadiusLocation;
		public int materialDistanceLocation;

		public float ratio;

		//radius of the effect of the black hole
		public float radius = 50.0f;

		public BlackHoleMaterial() {
			CreateShaderProgram("Render/Material/BlackHole/BlackHole_VS.glsl",
				"Render/Material/BlackHole/BlackHole_FS.glsl");


			// GL.BindAttribLocation, gibt an welcher Index in unserer Datenstruktur welchem "in" Parameter auf unserem Shader zugeordnet wird
			// folgende Befehle müssen aufgerufen werden...
			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_uv");

			// ...bevor das Shader-Programm "gelinkt" wird.
			GL.LinkProgram(Program);

			// Die Stelle an der im Shader der per "uniform" der Input-Paremeter "modelview_projection_matrix" definiert wird, wird ermittelt.
			modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");

			materialPositionLocation = GL.GetUniformLocation(Program, "position");
			materialRatioLocation = GL.GetUniformLocation(Program, "ratio");
			materialRadiusLocation = GL.GetUniformLocation(Program, "radius");
			materialDistanceLocation = GL.GetUniformLocation(Program, "distance");
		}

		protected override void PreDraw() {
			GL.UseProgram(Program);

			GL.Uniform2(materialPositionLocation, new Vector2(0.5f, 0.5f));
			GL.Uniform1(materialRatioLocation, 0.5625f);
			GL.Uniform1(materialRadiusLocation, 200.0f);
		}

		protected override void PostDraw() {
			GL.BindVertexArray(0);
		}

		protected override void Draw(Model3D model, MaterialSettings materialSettings) {
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.ColorTexture);
			GL.BindVertexArray(model.VAO);

			var modelviewProjection = (model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection).ToFloat();
			GL.UniformMatrix4(modelviewProjectionMatrixLocation, false, ref modelviewProjection);
			GL.Uniform1(materialDistanceLocation, Math3D.Distance(model.Transformation.ExtractTranslation().ToFloat(), DisplayCamera.Position.ToFloat()));

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}
	}
}