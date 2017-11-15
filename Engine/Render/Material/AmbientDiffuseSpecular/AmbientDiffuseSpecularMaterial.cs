using System;
using Engine.Model;
using Engine.Util;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public class AmbientDiffuseSpecularMaterial : BaseMaterial {
		private readonly int modelMatrixLocation;
		private readonly int modelviewProjectionMatrixLocation;

		private readonly int lightOriginLocation;
		private readonly int lightAmbientLocation;
		private readonly int lightDiffuseLocation;
		private readonly int lightSpecularLocation;
		private readonly int cameraPositionLocation;
		private readonly int materialShininessLocation;

		public AmbientDiffuseSpecularMaterial() {
			// shader-programm is loaded
			CreateShaderProgram("Render/Material/AmbientDiffuseSpecular/AmbientDiffuseSpecular_VS.glsl",
				"Render/Material/AmbientDiffuseSpecular/AmbientDiffuseSpecular_FS.glsl");

			// GL.BindAttribLocation, defines which index of the data-structure is assigned to which "in" parameter 
			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			// ...has to be done before the final "link" of the shader-program
			GL.LinkProgram(Program);

			// the location of the "uniform"-paramter "modelview_projection_matrix" on the shader is saved to modelviewProjectionMatrixLocation
			modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");

			// the location of the "uniform"-paramter for the model matrix on the shader is saved to modelviewMatrixLocation
			modelMatrixLocation = GL.GetUniformLocation(Program, "model_matrix");

			// the location of the of the "shininess" parameter
			materialShininessLocation = GL.GetUniformLocation(Program, "specular_shininess");

			// the location of the "uniform"-paramters of the light parameters
			lightOriginLocation = GL.GetUniformLocation(Program, "light_origin");
			lightAmbientLocation = GL.GetUniformLocation(Program, "light_ambient_color");
			lightDiffuseLocation = GL.GetUniformLocation(Program, "light_diffuse_color");
			lightSpecularLocation = GL.GetUniformLocation(Program, "light_specular_color");

			// we have to pass also the camera position
			cameraPositionLocation = GL.GetUniformLocation(Program, "camera_position");
		}

		public override void Draw(Model3D model, int textureId, float shininess = 0, int normalmap = -1) {
			// set the texture
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			// using the Vertex-Array-Object of out object
			GL.BindVertexArray(model.VAO);

			// using our shader
			GL.UseProgram(Program);

			// The matrix which we give as "modelview_projection_matrix" is assembled:
			// object-transformation * camera-transformation * perspective projection of the camera
			// on the shader each vertex-position is multiplied by this matrix. The result is the final position on the scree
			var modelViewProjection = (model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection)
				.ToFloat();

			// modelViewProjection is passed to the shader
			GL.UniformMatrix4(modelviewProjectionMatrixLocation, false, ref modelViewProjection);

			// The model matrix (just the transformation of the object) is also given to the shader. We want to multiply our normals with this matrix, to have them in world space.
			var modelMatrix = model.Transformation.ToFloat();
			GL.UniformMatrix4(modelMatrixLocation, false, ref modelMatrix);

			// Die Licht Parameter werden übergeben
			GL.Uniform3(lightOriginLocation, (float) Light.LightOrigin.X, (float) Light.LightOrigin.Y,
				(float) Light.LightOrigin.Z);
			GL.Uniform4(lightAmbientLocation, Light.LightAmbient);
			GL.Uniform4(lightDiffuseLocation, Light.LightDiffuse);
			GL.Uniform4(lightSpecularLocation, Light.LightSpecular);

			// Shininess
			GL.Uniform1(materialShininessLocation, shininess);

			// Pass positions of the camera to calculate the view direction
			GL.Uniform4(cameraPositionLocation, (float) DisplayCamera.Position.X, (float) DisplayCamera.Position.Y,
				(float) DisplayCamera.Position.Z, 1);

			// The object is drawn
			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindVertexArray(0);
		}
	}
}