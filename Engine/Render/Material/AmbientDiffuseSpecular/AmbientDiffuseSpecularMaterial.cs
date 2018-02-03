using System;
using Engine.Model;
using Engine.Render;
using OpenTK;
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
			CreateShaderProgram("Render/Material/AmbientDiffuseSpecular/AmbientDiffuseSpecular_VS.glsl",
				"Render/Material/AmbientDiffuseSpecular/AmbientDiffuseSpecular_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			GL.LinkProgram(Program);

			modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");
			modelMatrixLocation = GL.GetUniformLocation(Program, "model_matrix");
			materialShininessLocation = GL.GetUniformLocation(Program, "specular_shininess");

			lightOriginLocation = GL.GetUniformLocation(Program, "light_origin");
			lightAmbientLocation = GL.GetUniformLocation(Program, "light_ambient_color");
			lightDiffuseLocation = GL.GetUniformLocation(Program, "light_diffuse_color");
			lightSpecularLocation = GL.GetUniformLocation(Program, "light_specular_color");

			cameraPositionLocation = GL.GetUniformLocation(Program, "camera_position");
		}

		protected override void PreDraw() {
			GL.UseProgram(Program);

			GL.Uniform3(lightOriginLocation, Light.LightOrigin.ToFloat());
			GL.Uniform4(lightAmbientLocation, Light.LightAmbient);
			GL.Uniform4(lightDiffuseLocation, Light.LightDiffuse);
			GL.Uniform4(lightSpecularLocation, Light.LightSpecular);
			
			GL.Uniform4(cameraPositionLocation, new Vector4(DisplayCamera.Position.ToFloat(), 1));
		}

		protected override void PostDraw() => GL.BindVertexArray(0);

		protected override void Draw(Model3D model, MaterialSettings materialSettings) {
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.ColorTexture);
			GL.BindVertexArray(model.VAO);

			var modelViewProjection = (model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection).ToFloat();
			GL.UniformMatrix4(modelviewProjectionMatrixLocation, false, ref modelViewProjection);

			var modelMatrix = model.Transformation.ToFloat();
			GL.UniformMatrix4(modelMatrixLocation, false, ref modelMatrix);
			GL.Uniform1(materialShininessLocation, (float) materialSettings.Shininess);

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}
	}
}