using System;
using OpenTK.Graphics.OpenGL;
using Engine.Model;

namespace Engine.Material {
	public class SimpleReflectionMaterial : BaseMaterial {
		private readonly int _modelviewProjectionMatrixLocation;
		private readonly int _modelviewMatrixLocation;

		public SimpleReflectionMaterial() {
			// shader-programm is loaded
			CreateShaderProgram("Render/Material/Simplereflection/SimpleReflection_VS.glsl",
				"Render/Material/Simplereflection/SimpleReflection_FS.glsl");

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

		public override void Draw(Model3D model, int textureId, float shininess = 0f, int normalmap = -1) {
			// set the texture
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			// using the Vertex-Array-Object of out object
			GL.BindVertexArray(model.VAO);

			// using our shader
			GL.UseProgram(Program);


			// The matrix which we give as "modelview_projection_matrix" is assembled:
			// object-transformation * camera-transformation * perspective projection of the camera
			// on the shader each vertex-position is multiplied by this matrix. The result is the final position on the screen
			var modelviewProjection =
				model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection;

			// Matrix is passed to the shader
			GL.UniformMatrix4(_modelviewProjectionMatrixLocation, false, ref modelviewProjection);

			// The "modelView-matrix is assembled together
			var modelviewMatrix = model.Transformation * DisplayCamera.Transformation;

			// ... and also passed to the shader
			GL.UniformMatrix4(_modelviewMatrixLocation, false, ref modelviewMatrix);


			// the object is drawn
			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
			
			GL.BindVertexArray(0);
		}
	}
}