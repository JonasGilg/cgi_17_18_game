using System;
using Engine.Model;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public class SimpleTextureMaterial : BaseMaterial {
		private readonly int _modelviewProjectionMatrixLocation;

		public SimpleTextureMaterial() {
			// Shader-Programm wird aus den externen Files generiert...
			CreateShaderProgram("Render/Material/Simpletexture/Simple_VS.glsl",
				"Render/Material/Simpletexture/Simple_FS.glsl");

			// GL.BindAttribLocation, gibt an welcher Index in unserer Datenstruktur welchem "in" Parameter auf unserem Shader zugeordnet wird
			// folgende Befehle müssen aufgerufen werden...
			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			// ...bevor das Shader-Programm "gelinkt" wird.
			GL.LinkProgram(Program);

			// Die Stelle an der im Shader der per "uniform" der Input-Paremeter "modelview_projection_matrix" definiert wird, wird ermittelt.
			_modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");
		}

		public override void Draw(Model3D model, int textureId, float shininess = 0f, int normalmap = -1) {
			// Textur wird "gebunden"
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			// das Vertex-Array-Objekt unseres Objekts wird benutzt
			GL.BindVertexArray(model.Vao);

			// Unser Shader Programm wird benutzt
			GL.UseProgram(Program);

			// Die Matrix, welche wir als "modelview_projection_matrix" übergeben, wird zusammengebaut:
			// Objekt-Transformation * Kamera-Transformation * Perspektivische Projektion der kamera.
			// Auf dem Shader wird jede Vertex-Position mit dieser Matrix multipliziert. Resultat ist die Position auf dem Screen.
			var modelviewProjection =
				model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection;

			// Die Matrix wird dem Shader als Parameter übergeben
			GL.UniformMatrix4(_modelviewProjectionMatrixLocation, false, ref modelviewProjection);

			// Das Objekt wird gezeichnet
			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}
	}
}