using System;
using Engine.Model;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public class NormalMappingMaterial : BaseMaterial {
		private readonly int _modelMatrixLocation;
		private readonly int _modelviewProjectionMatrixLocation;

		private readonly int _lightDirectionLocation;
		private readonly int _lightAmbientLocation;
		private readonly int _lightDiffuseLocation;
		private readonly int _lightSpecularLocation;
		private readonly int _cameraPositionLocation;
		private readonly int _materialShininessLocation;

		private readonly int _colorTextureLocation;
		private readonly int _normalTextureLocation;

		public int normalTextureId;
		public float shininess;

		public NormalMappingMaterial() {
			// Shader-Programm wird aus den externen Files generiert...
			CreateShaderProgram("cgimin/engine/material/normalmapping/NormalMapping_VS.glsl",
				"cgimin/engine/material/normalmapping/NormalMapping_FS.glsl");

			// GL.BindAttribLocation, gibt an welcher Index in unserer Datenstruktur welchem "in" Parameter auf unserem Shader zugeordnet wird
			// folgende Befehle müssen aufgerufen werden...
			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");
			GL.BindAttribLocation(Program, 3, "in_tangent");
			GL.BindAttribLocation(Program, 4, "in_bitangent");

			// ...bevor das Shader-Programm "gelinkt" wird.
			GL.LinkProgram(Program);

			// Die Stelle an der im Shader der per "uniform" der Input-Paremeter "modelview_projection_matrix" definiert wird, wird ermittelt.
			_modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");

			// Die Stelle für die den "model_matrix" - Parameter wird ermittelt.
			_modelMatrixLocation = GL.GetUniformLocation(Program, "model_matrix");

			// Die Stelle fur den "specular_shininess" - Parameter
			_materialShininessLocation = GL.GetUniformLocation(Program, "specular_shininess");

			// Die Stellen im Fragemant-Shader für Licht-parameter ermitteln.
			_lightDirectionLocation = GL.GetUniformLocation(Program, "light_direction");
			_lightAmbientLocation = GL.GetUniformLocation(Program, "light_ambient_color");
			_lightDiffuseLocation = GL.GetUniformLocation(Program, "light_diffuse_color");
			_lightSpecularLocation = GL.GetUniformLocation(Program, "light_specular_color");
			_cameraPositionLocation = GL.GetUniformLocation(Program, "camera_position");
			_colorTextureLocation = GL.GetUniformLocation(Program, "color_texture");
			_normalTextureLocation = GL.GetUniformLocation(Program, "normalmap_texture");
		}


		public override void Draw() {
			// Das Vertex-Array-Objekt unseres Objekts wird benutzt
			GL.BindVertexArray(model3D.Vao);

			// Unser Shader Programm wird benutzt
			GL.UseProgram(Program);

			// Farb-Textur wird "gebunden"
			GL.Uniform1(_colorTextureLocation, 0);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			// Normalmap-Textur wird "gebunden"
			GL.Uniform1(_normalTextureLocation, 1);
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, normalTextureId);

			// Die Matrix, welche wir als "modelview_projection_matrix" übergeben, wird zusammengebaut:
			// Objekt-Transformation * Kamera-Transformation * Perspektivische Projektion der kamera.
			// Auf dem Shader wird jede Vertex-Position mit dieser Matrix multipliziert. Resultat ist die Position auf dem Screen.
			var modelViewProjection = model3D.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection;

			// Die ModelViewProjection Matrix wird dem Shader als Parameter übergeben
			GL.UniformMatrix4(_modelviewProjectionMatrixLocation, false, ref modelViewProjection);

			// Die Model-Matrix wird dem Shader übergeben, zur transformation der Normalen
			// und der Berechnung des Winkels Betrachter / Objektpunkt 
			var model = model3D.Transformation;
			GL.UniformMatrix4(_modelMatrixLocation, false, ref model);

			// Die Licht Parameter werden übergeben
			GL.Uniform3(_lightDirectionLocation, Light.LightDirection);
			GL.Uniform4(_lightAmbientLocation, Light.LightAmbient);
			GL.Uniform4(_lightDiffuseLocation, Light.LightDiffuse);
			GL.Uniform4(_lightSpecularLocation, Light.LightSpecular);

			// Shininess
			GL.Uniform1(_materialShininessLocation, shininess);

			// Positions Parameter
			GL.Uniform4(_cameraPositionLocation, new Vector4(DisplayCamera.Position.X, DisplayCamera.Position.Y, DisplayCamera.Position.Z, 1));

			// Das Objekt wird gezeichnet
			GL.DrawElements(PrimitiveType.Triangles, model3D.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

			// Active Textur wieder auf 0, um andere Materialien nicht durcheinander zu bringen
			GL.ActiveTexture(TextureUnit.Texture0);
		}
	}
}