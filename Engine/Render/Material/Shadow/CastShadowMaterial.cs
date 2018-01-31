using System;
using Engine.Model;
using Engine.Render;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material.Shadow {
	public class CastShadowMaterial : BaseMaterial {
		private readonly int depthMVPLocation;

		public CastShadowMaterial() {
			CreateShaderProgram("Render/Material/Shadow/CastShadow_VS.glsl",
				"Render/Material/Shadow/CastShadow_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");

			GL.LinkProgram(Program);

			depthMVPLocation = GL.GetUniformLocation(Program, "depthMVP");
		}

		protected override void Draw(Model3D model, MaterialSettings materialSettings) {
			var depthMVPShadow = (model.Transformation * CascadedShadowMapping.LightCamera.LookAtMatrix * CascadedShadowMapping.LightCamera.PerspectiveProjection).ToFloat();
			GL.UniformMatrix4(depthMVPLocation, false, ref depthMVPShadow);

			GL.BindVertexArray(model.VAO);

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindVertexArray(0);
		}

		protected override void PreDraw() => GL.UseProgram(Program);

		protected override void PostDraw() => GL.ActiveTexture(TextureUnit.Texture0);
	}
}