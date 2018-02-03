using System;
using Engine.Model;
using Engine.Render;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material.Deferred {
	public class PBRMaterial : BaseMaterial {
		private readonly int modelMatrixLocation;
		private readonly int modelViewMatrixLocation;
		private readonly int modelviewProjectionMatrixLocation;

		private readonly int colorTextureLocation;
		private readonly int normalTextureLocation;
		private readonly int metalnessTextureLocation;
		private readonly int roughnessTextureLocation;
		private readonly int aoTextureLocation;
		private readonly int glowTextureLocation;

		private readonly int dist1Location;
		private readonly int dist2Location;
		private readonly int dist3Location;

		private readonly int depthBiasMvpLocation1;
		private readonly int depthBiasMvpLocation2;
		private readonly int depthBiasMvpLocation3;

		private readonly int shadowTextureLocation1;
		private readonly int shadowTextureLocation2;
		private readonly int shadowTextureLocation3;

		private readonly int lightDirectionLocation;

		public PBRMaterial() {
			CreateShaderProgram("Render/Material/Deferred/PBR_VS.glsl",
				"Render/Material/Deferred/PBR_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");
			GL.BindAttribLocation(Program, 3, "in_tangent");
			GL.BindAttribLocation(Program, 4, "in_bitangent");

			GL.LinkProgram(Program);

			modelMatrixLocation = GL.GetUniformLocation(Program, "model_matrix");
			modelViewMatrixLocation = GL.GetUniformLocation(Program, "modelview_matrix");
			modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");

			colorTextureLocation = GL.GetUniformLocation(Program, "color_texture");
			normalTextureLocation = GL.GetUniformLocation(Program, "normalmap_texture");
			metalnessTextureLocation = GL.GetUniformLocation(Program, "metalness_texture");
			roughnessTextureLocation = GL.GetUniformLocation(Program, "roughness_texture");
			aoTextureLocation = GL.GetUniformLocation(Program, "ao_texture");
			glowTextureLocation = GL.GetUniformLocation(Program, "glow_texture");

			dist1Location = GL.GetUniformLocation(Program, "dist1");
			dist2Location = GL.GetUniformLocation(Program, "dist2");
			dist3Location = GL.GetUniformLocation(Program, "dist3");

			depthBiasMvpLocation1 = GL.GetUniformLocation(Program, "DepthBiasMVP1");
			depthBiasMvpLocation2 = GL.GetUniformLocation(Program, "DepthBiasMVP2");
			depthBiasMvpLocation3 = GL.GetUniformLocation(Program, "DepthBiasMVP3");

			shadowTextureLocation1 = GL.GetUniformLocation(Program, "shadowmap_texture1");
			shadowTextureLocation2 = GL.GetUniformLocation(Program, "shadowmap_texture2");
			shadowTextureLocation3 = GL.GetUniformLocation(Program, "shadowmap_texture3");

			lightDirectionLocation = GL.GetUniformLocation(Program, "light_direction");
		}

		protected override void Draw(Model3D model3D, MaterialSettings materialSettings) {
			GL.BindVertexArray(model3D.VAO);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.ColorTexture);

			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.NormalTexture);

			GL.ActiveTexture(TextureUnit.Texture2);
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.MetalnessTexture);

			GL.ActiveTexture(TextureUnit.Texture3);
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.RoughnessTexture);

			GL.ActiveTexture(TextureUnit.Texture4);
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.AOTexture);

			GL.ActiveTexture(TextureUnit.Texture5);
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.GlowTexture);

			var modelViewProjection = (model3D.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection).ToFloat();
			GL.UniformMatrix4(modelviewProjectionMatrixLocation, false, ref modelViewProjection);

			var modelView = (model3D.Transformation * DisplayCamera.Transformation).ToFloat();
			GL.UniformMatrix4(modelViewMatrixLocation, false, ref modelView);

			var model = model3D.Transformation.ToFloat();
			GL.UniformMatrix4(modelMatrixLocation, false, ref model);

			GL.Uniform1(dist1Location, CascadedShadowMapping.Cascades[0].BorderDistance);
			GL.Uniform1(dist2Location, CascadedShadowMapping.Cascades[1].BorderDistance);
			GL.Uniform1(dist3Location, CascadedShadowMapping.Cascades[2].BorderDistance);

			var depthMVP = (model3D.Transformation * CascadedShadowMapping.Cascades[0].ShadowTransformation * CascadedShadowMapping.Cascades[0].DepthBias * CascadedShadowMapping.Cascades[0].ShadowProjection).ToFloat();
			GL.UniformMatrix4(depthBiasMvpLocation1, false, ref depthMVP);

			var depthMVP2 = (model3D.Transformation * CascadedShadowMapping.Cascades[1].ShadowTransformation * CascadedShadowMapping.Cascades[1].DepthBias * CascadedShadowMapping.Cascades[1].ShadowProjection).ToFloat();
			GL.UniformMatrix4(depthBiasMvpLocation2, false, ref depthMVP2);

			var depthMVP3 = (model3D.Transformation * CascadedShadowMapping.Cascades[2].ShadowTransformation * CascadedShadowMapping.Cascades[2].DepthBias * CascadedShadowMapping.Cascades[2].ShadowProjection).ToFloat();
			GL.UniformMatrix4(depthBiasMvpLocation3, false, ref depthMVP3);

			GL.Uniform3(lightDirectionLocation, CascadedShadowMapping.LightDirection.ToFloat());

			GL.DrawElements(PrimitiveType.Triangles, model3D.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindVertexArray(0);
		}

		protected override void PreDraw() {
			GL.UseProgram(Program);

			GL.Uniform1(colorTextureLocation, 0);
			GL.Uniform1(normalTextureLocation, 1);
			GL.Uniform1(metalnessTextureLocation, 2);
			GL.Uniform1(roughnessTextureLocation, 3);
			GL.Uniform1(aoTextureLocation, 4);
			GL.Uniform1(glowTextureLocation, 5);

			GL.Uniform1(shadowTextureLocation1, 6);
			GL.ActiveTexture(TextureUnit.Texture6);
			GL.BindTexture(TextureTarget.Texture2D, CascadedShadowMapping.Cascades[0].DepthTexture);

			GL.Uniform1(shadowTextureLocation2, 7);
			GL.ActiveTexture(TextureUnit.Texture7);
			GL.BindTexture(TextureTarget.Texture2D, CascadedShadowMapping.Cascades[1].DepthTexture);

			GL.Uniform1(shadowTextureLocation3, 8);
			GL.ActiveTexture(TextureUnit.Texture8);
			GL.BindTexture(TextureTarget.Texture2D, CascadedShadowMapping.Cascades[2].DepthTexture);
		}

		protected override void PostDraw() => GL.ActiveTexture(TextureUnit.Texture0);
	}
}