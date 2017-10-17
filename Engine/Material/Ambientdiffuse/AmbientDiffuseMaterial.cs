using System;
using Engine.Model;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material.Ambientdiffuse {
	public class AmbientDiffuseMaterial : BaseMaterial {
		private readonly int _modelMatrixLocation;
		private readonly int _modelviewProjectionMatrixLocation;

		private readonly int _lightDirectionLocation;
		private readonly int _lightAmbientLocation;
		private readonly int _lightDiffuseLocation;

		public AmbientDiffuseMaterial() {
			// shader-programm is loaded
			CreateShaderProgram("CGI/Engine/Material/Ambientdiffuse/AmbientDiffuse_VS.glsl",
				"CGI/Engine/Material/Ambientdiffuse/AmbientDiffuse_FS.glsl");

			// GL.BindAttribLocation, defines which index of the data-structure is assigned to which "in" parameter 
			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			// ...has to be done before the final "link" of the shader-program
			GL.LinkProgram(Program);

			// the location of the "uniform"-paramter "modelview_projection_matrix" on the shader is saved to modelviewProjectionMatrixLocation
			_modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");

			// the location of the "uniform"-paramter for the model matrix on the shader is saved to modelviewMatrixLocation
			_modelMatrixLocation = GL.GetUniformLocation(Program, "model_matrix");

			// the location of the "uniform"-paramters of the light parameters
			_lightDirectionLocation = GL.GetUniformLocation(Program, "light_direction");
			_lightAmbientLocation = GL.GetUniformLocation(Program, "light_ambient_color");
			_lightDiffuseLocation = GL.GetUniformLocation(Program, "light_diffuse_color");
		}

		public void Draw(Model3D object3D, int textureId) {
			// set the texture
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			// using the Vertex-Array-Object of out object
			GL.BindVertexArray(object3D.Vao);

			// using our shader
			GL.UseProgram(Program);

			// The matrix which we give as "modelview_projection_matrix" is assembled:
			// object-transformation * camera-transformation * perspective projection of the camera
			// on the shader each vertex-position is multiplied by this matrix. The result is the final position on the screen
			var modelViewProjection =
				object3D.Transformation * Camera.Camera.Transformation * Camera.Camera.PerspectiveProjection;

			// modelViewProjection is passed to the shader
			GL.UniformMatrix4(_modelviewProjectionMatrixLocation, false, ref modelViewProjection);

			// The model matrix (just the transformation of the object) is also given to the shader. We want to multiply our normals with this matrix, to have them in world space.
			var model = object3D.Transformation;
			GL.UniformMatrix4(_modelMatrixLocation, false, ref model);

			// Die Licht Parameter werden übergeben
			GL.Uniform3(_lightDirectionLocation, Light.Light.LightDirection);
			GL.Uniform4(_lightAmbientLocation, Light.Light.LightAmbient);
			GL.Uniform4(_lightDiffuseLocation, Light.Light.LightDiffuse);

			// Das Objekt wird gezeichnet
			GL.DrawElements(PrimitiveType.Triangles, object3D.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}
	}
}