using System;
using Engine.Model;
using Engine.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public class NormalMappingMaterial : BaseMaterial {
		private readonly int viewMatrixLocation;
		private readonly int modelMatrixLocation;
		private readonly int modelView3X3Location;
		private readonly int modelviewProjectionMatrixLocation;

		private readonly int lightDirectionLocation;
		private readonly int lightAmbientLocation;
		private readonly int lightDiffuseLocation;
		private readonly int lightSpecularLocation;
		private readonly int cameraPositionLocation;
		private readonly int materialShininessLocation;

		private readonly int colorTextureLocation;
		private readonly int normalTextureLocation;

		public NormalMappingMaterial() {
			CreateShaderProgram("Render/Material/NormalMapping/NormalMapping_VS.glsl",
				"Render/Material/NormalMapping/NormalMapping_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");
			GL.BindAttribLocation(Program, 3, "in_tangent");
			GL.BindAttribLocation(Program, 4, "in_bitangent");

			GL.LinkProgram(Program);

			modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");
			modelMatrixLocation = GL.GetUniformLocation(Program, "model_matrix");
			viewMatrixLocation = GL.GetUniformLocation(Program, "view_matrix");

			materialShininessLocation = GL.GetUniformLocation(Program, "specular_shininess");

			lightDirectionLocation = GL.GetUniformLocation(Program, "light_origin");
			lightAmbientLocation = GL.GetUniformLocation(Program, "light_ambient_color");
			lightDiffuseLocation = GL.GetUniformLocation(Program, "light_diffuse_color");
			lightSpecularLocation = GL.GetUniformLocation(Program, "light_specular_color");
			cameraPositionLocation = GL.GetUniformLocation(Program, "camera_position");
			colorTextureLocation = GL.GetUniformLocation(Program, "color_texture");
			normalTextureLocation = GL.GetUniformLocation(Program, "normalmap_texture");
			viewMatrixLocation = GL.GetUniformLocation(Program, "view_matrix");
			modelView3X3Location = GL.GetUniformLocation(Program, "model_view_3x3_matrix");
		}


		public override void Draw(Model3D model, int textureId, float shininess = 0f, int normalmap = -1) {
			GL.BindVertexArray(model.VAO);

			GL.UseProgram(Program);

			GL.Uniform1(colorTextureLocation, 0);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			GL.Uniform1(normalTextureLocation, 1);
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, normalmap);

			var modelViewProjection = (model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection)
				.ToFloat();
			GL.UniformMatrix4(modelviewProjectionMatrixLocation, false, ref modelViewProjection);

			var modelMatrix = model.Transformation.ToFloat();
			GL.UniformMatrix4(modelMatrixLocation, false, ref modelMatrix);

			var viewMatrix = DisplayCamera.Transformation.ToFloat();
			GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);

			var modelView3x3 = new Matrix3d(DisplayCamera.Transformation * model.Transformation).ToFloat();
			GL.UniformMatrix3(modelView3X3Location, false, ref modelView3x3);

			GL.Uniform3(lightDirectionLocation, Light.LightOrigin.ToFloat());
			GL.Uniform4(lightAmbientLocation, Light.LightAmbient);
			GL.Uniform4(lightDiffuseLocation, Light.LightDiffuse);
			GL.Uniform4(lightSpecularLocation, Light.LightSpecular);

			GL.Uniform1(materialShininessLocation, shininess);

			GL.Uniform4(cameraPositionLocation, (float) DisplayCamera.Position.X, (float) DisplayCamera.Position.Y,
				(float) DisplayCamera.Position.Z, 1);

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.ActiveTexture(TextureUnit.Texture0);

			GL.BindVertexArray(0);
		}
	}
}