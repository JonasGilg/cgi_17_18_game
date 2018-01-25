using System;
using Engine.Model;
using Engine.Render;
using Engine.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Local {
	public class PointLight : BaseLighting {
		private readonly int modelviewProjectionMatrixLocation;

		private readonly int midPositionLocation;
		private readonly int radiusLocation;
		private readonly int colorLocation;
		private readonly int cameraPosLocation;

		private readonly int gNormalLocation;
		private readonly int gPositionLocation;
		private readonly int gColorAndRoughnessLocation;
		private readonly int gMetalnessAndShadowLocation;

		private readonly int screenWidthLocation;
		private readonly int screenHeightLocation;

		public PointLight() {
			Program = ShaderCompiler.CreateShaderProgram("Light/Local/PointLight_VS.glsl",
				"Light/Local/PointLight_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			GL.LinkProgram(Program);

			modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");
			midPositionLocation = GL.GetUniformLocation(Program, "midPosition");
			radiusLocation = GL.GetUniformLocation(Program, "radius");
			colorLocation = GL.GetUniformLocation(Program, "color");
			gNormalLocation = GL.GetUniformLocation(Program, "GNormal");
			gPositionLocation = GL.GetUniformLocation(Program, "GPosition");
			gColorAndRoughnessLocation = GL.GetUniformLocation(Program, "GColorAndRoughness");
			gMetalnessAndShadowLocation = GL.GetUniformLocation(Program, "GMetalness");

			screenWidthLocation = GL.GetUniformLocation(Program, "screenWidth");
			screenHeightLocation = GL.GetUniformLocation(Program, "screenHeight");

			cameraPosLocation = GL.GetUniformLocation(Program, "camera_position");
		}

		public void PrepareDraw(Model3D model) {
			GL.BindVertexArray(model.VAO);

			GL.UseProgram(Program);

			GL.Uniform1(gNormalLocation, 0);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, DeferredRendering.GNormalBuffer);

			GL.Uniform1(gPositionLocation, 1);
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, DeferredRendering.GPositionBuffer);

			GL.Uniform1(gColorAndRoughnessLocation, 2);
			GL.ActiveTexture(TextureUnit.Texture2);
			GL.BindTexture(TextureTarget.Texture2D, DeferredRendering.GColorRoughnessBuffer);

			GL.Uniform1(gMetalnessAndShadowLocation, 3);
			GL.ActiveTexture(TextureUnit.Texture3);
			GL.BindTexture(TextureTarget.Texture2D, DeferredRendering.GMetalnessShadowBuffer);
		}

		public void FinishDraw() {
			GL.BindVertexArray(0);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, 1);

			GL.ActiveTexture(TextureUnit.Texture2);
			GL.BindTexture(TextureTarget.Texture2D, 2);

			GL.ActiveTexture(TextureUnit.Texture3);
			GL.BindTexture(TextureTarget.Texture2D, 3);
		}

		public void Draw(Model3D model3D, Vector3d position, float radius, Vector3 color, int screenWidth, int screenHeight) {
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);


			GL.BindVertexArray(model3D.VAO);

			GL.UseProgram(Program);

			GL.Uniform1(gNormalLocation, 0);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, DeferredRendering.GNormalBuffer);

			GL.Uniform1(gPositionLocation, 1);
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, DeferredRendering.GPositionBuffer);

			GL.Uniform1(gColorAndRoughnessLocation, 2);
			GL.ActiveTexture(TextureUnit.Texture2);
			GL.BindTexture(TextureTarget.Texture2D, DeferredRendering.GColorRoughnessBuffer);

			GL.Uniform1(gMetalnessAndShadowLocation, 3);
			GL.ActiveTexture(TextureUnit.Texture3);
			GL.BindTexture(TextureTarget.Texture2D, DeferredRendering.GMetalnessShadowBuffer);

			var model = Matrix4d.Identity;
			model *= Matrix4.CreateScale(radius, radius, radius).ToDouble();
			model *= Matrix4d.CreateTranslation(position);

			var modelviewProjection = model * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection;

			GL.UniformMatrix4(modelviewProjectionMatrixLocation, false, ref modelviewProjection);

			var midPos = new Vector3d(0, 0, 0);
			midPos = Vector3d.TransformPosition(midPos, model);
			GL.Uniform3(midPositionLocation, midPos.ToFloat());

			GL.Uniform1(radiusLocation, radius);
			GL.Uniform3(colorLocation, color);

			GL.Uniform1(screenWidthLocation, (float) screenWidth);
			GL.Uniform1(screenHeightLocation, (float) screenHeight);

			GL.Uniform3(cameraPosLocation, new Vector3((float) DisplayCamera.Position.X, (float) DisplayCamera.Position.Y, (float) DisplayCamera.Position.Z));

			GL.DrawElements(PrimitiveType.Triangles, model3D.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindVertexArray(0);
		}
	}
}