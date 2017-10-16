using System;
using CGI.Engine.Material;
using OpenTK.Graphics.OpenGL;
using CGI.Engine.Object3D;

namespace CGI.Engine.Material.Simplereflection {
	public class SimpleReflectionMaterial : BaseMaterial {
		private readonly int _modelviewProjectionMatrixLocation;
		private readonly int _modelviewMatrixLocation;

		public SimpleReflectionMaterial() {
			// shader-programm is loaded
			CreateShaderProgram("CGI/Engine/Material/Simplereflection/SimpleReflection_VS.glsl",
				"CGI/Engine/Material/Simplereflection/SimpleReflection_FS.glsl");

			// GL.BindAttribLocation, defines which index of the data-structure is assigned to which "in" parameter 
			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			// ...has to be done before the final "link" of the shader-program
			GL.LinkProgram(Program);

			// the location of the "uniform"-paramter "modelview_projection_matrix" on the shader is saved to modelviewProjectionMatrixLocation
			_modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");

			// the location of the "uniform"-paramter "modelview_matrix" on the shader is saved to modelviewMatrixLocation
			_modelviewMatrixLocation = GL.GetUniformLocation(Program, "modelview_matrix");
		}

		public void Draw(BaseObject3D object3D, int textureId) {
			// set the texture
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			// using the Vertex-Array-Object of out object
			GL.BindVertexArray(object3D.Vao);

			// using our shader
			GL.UseProgram(Program);


			// The matrix which we give as "modelview_projection_matrix" is assembled:
			// object-transformation * camera-transformation * perspective projection of the camera
			// on the shader each vertex-position is multiplied by this matrix. The result is the final position on the screen
			var modelviewProjection =
				object3D.Transformation * Camera.Camera.Transformation * Camera.Camera.PerspectiveProjection;

			// Matrix is passed to the shader
			GL.UniformMatrix4(_modelviewProjectionMatrixLocation, false, ref modelviewProjection);

			// The "modelView-matrix is assembled together
			var modelviewMatrix = object3D.Transformation * Camera.Camera.Transformation;

			// ... and also passed to the shader
			GL.UniformMatrix4(_modelviewMatrixLocation, false, ref modelviewMatrix);


			// the object is drawn
			GL.DrawElements(PrimitiveType.Triangles, object3D.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}
	}
}