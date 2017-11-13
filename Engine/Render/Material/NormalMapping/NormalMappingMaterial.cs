using System;
using Engine.Model;
using Engine.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public class NormalMappingMaterial : BaseMaterial {
		private readonly int _viewMatrixLocation;
		private readonly int _modelMatrixLocation;
		private readonly int _modelView3x3Location;
		private readonly int _modelviewProjectionMatrixLocation;

		private readonly int _lightDirectionLocation;
		private readonly int _lightAmbientLocation;
		private readonly int _lightDiffuseLocation;
		private readonly int _lightSpecularLocation;
		private readonly int _cameraPositionLocation;
		private readonly int _materialShininessLocation;

		private readonly int _colorTextureLocation;
		private readonly int _normalTextureLocation;

		public NormalMappingMaterial() {
			CreateShaderProgram("Render/Material/NormalMapping/NormalMapping_VS.glsl",
				"Render/Material/NormalMapping/NormalMapping_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");
			GL.BindAttribLocation(Program, 3, "in_tangent");
			GL.BindAttribLocation(Program, 4, "in_bitangent");

			GL.LinkProgram(Program);

			_modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");
			_modelMatrixLocation = GL.GetUniformLocation(Program, "model_matrix");
			_viewMatrixLocation = GL.GetUniformLocation(Program, "view_matrix");

			_materialShininessLocation = GL.GetUniformLocation(Program, "specular_shininess");

			_lightDirectionLocation = GL.GetUniformLocation(Program, "light_origin");
			_lightAmbientLocation = GL.GetUniformLocation(Program, "light_ambient_color");
			_lightDiffuseLocation = GL.GetUniformLocation(Program, "light_diffuse_color");
			_lightSpecularLocation = GL.GetUniformLocation(Program, "light_specular_color");
			_cameraPositionLocation = GL.GetUniformLocation(Program, "camera_position");
			_colorTextureLocation = GL.GetUniformLocation(Program, "color_texture");
			_normalTextureLocation = GL.GetUniformLocation(Program, "normalmap_texture");
			_viewMatrixLocation = GL.GetUniformLocation(Program, "view_matrix");
			_modelView3x3Location = GL.GetUniformLocation(Program, "model_view_3x3_matrix");
		}


		public override void Draw(Model3D model, int textureId, float shininess = 0f, int normalmap = -1) {
			GL.BindVertexArray(model.VAO);

			GL.UseProgram(Program);

			GL.Uniform1(_colorTextureLocation, 0);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			GL.Uniform1(_normalTextureLocation, 1);
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, normalmap);

			var modelViewProjection = (model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection).ToFloat();
			GL.UniformMatrix4(_modelviewProjectionMatrixLocation, false, ref modelViewProjection);

			var modelMatrix = model.Transformation.ToFloat();
			GL.UniformMatrix4(_modelMatrixLocation, false, ref modelMatrix);

			var viewMatrix = DisplayCamera.Transformation.ToFloat();
			GL.UniformMatrix4(_viewMatrixLocation, false, ref viewMatrix);
			
			var modelView3x3 = new Matrix3d(DisplayCamera.Transformation * model.Transformation).ToFloat();
			GL.UniformMatrix3(_modelView3x3Location, false, ref modelView3x3);

			GL.Uniform3(_lightDirectionLocation, Light.LightOrigin.ToFloat());
			GL.Uniform4(_lightAmbientLocation, Light.LightAmbient);
			GL.Uniform4(_lightDiffuseLocation, Light.LightDiffuse);
			GL.Uniform4(_lightSpecularLocation, Light.LightSpecular);

			GL.Uniform1(_materialShininessLocation, shininess);

			GL.Uniform4(_cameraPositionLocation, (float) DisplayCamera.Position.X, (float) DisplayCamera.Position.Y, (float) DisplayCamera.Position.Z, 1);

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.ActiveTexture(TextureUnit.Texture0);
			
			GL.BindVertexArray(0);
		}
	}
}