using System;
using Engine.Model;
using Engine.Texture;
using Engine.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine {
	public struct IBLData {
		public int IrradianceCubeTexture;
		public int SpecularCubeTexture;
	}
	
	public class ImageBasedLighting : BaseLighting {
		private readonly int gColorRoughnessLocation;
		private readonly int gNormalLocation;
		private readonly int gPositionLocation;
		private readonly int gMetalnessAndShadowLocation;
		private readonly int gGlowLocation;

		private readonly int iblSpcularCubeLocation;
		private readonly int iblIrradianceCubeLocation;

		private readonly int cameraPosLocation;

		private readonly int iblLookupTexture;
		private readonly int iblLookupTextureLocation;

		public ImageBasedLighting() {
			iblLookupTexture = TextureManager.LoadTexture("Light/IBL/ibl_brdf_lut.png");

			Program = ShaderCompiler.CreateShaderProgram("Light/IBL/IBL_VS.glsl", "Light/IBL/IBL_FS.glsl");

			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			GL.LinkProgram(Program);

			gColorRoughnessLocation = GL.GetUniformLocation(Program, "gColorRoughness");
			gNormalLocation = GL.GetUniformLocation(Program, "gNormal");
			gPositionLocation = GL.GetUniformLocation(Program, "gPosition");
			gMetalnessAndShadowLocation = GL.GetUniformLocation(Program, "gMetalAndShadow");
			gGlowLocation = GL.GetUniformLocation(Program, "gGlow");

			iblSpcularCubeLocation = GL.GetUniformLocation(Program, "iblSpecular");
			iblIrradianceCubeLocation = GL.GetUniformLocation(Program, "iblIrradiance");

			cameraPosLocation = GL.GetUniformLocation(Program, "camera_position");

			iblLookupTextureLocation = GL.GetUniformLocation(Program, "brdfLUT");
		}

		public void Draw(Model3D model, int colorRoughnessBufferTexID, int normalTexID, int positionTexID, int metalnessTexID, int glowTexID, IBLData iblData) {
			GL.BindVertexArray(model.VAO);

			GL.UseProgram(Program);

			GL.Uniform1(gColorRoughnessLocation, 0);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, colorRoughnessBufferTexID);

			GL.Uniform1(gNormalLocation, 1);
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, normalTexID);

			GL.Uniform1(gPositionLocation, 2);
			GL.ActiveTexture(TextureUnit.Texture2);
			GL.BindTexture(TextureTarget.Texture2D, positionTexID);

			GL.Uniform1(gMetalnessAndShadowLocation, 3);
			GL.ActiveTexture(TextureUnit.Texture3);
			GL.BindTexture(TextureTarget.Texture2D, metalnessTexID);

			GL.Uniform1(gGlowLocation, 4);
			GL.ActiveTexture(TextureUnit.Texture4);
			GL.BindTexture(TextureTarget.Texture2D, glowTexID);

			GL.Uniform1(iblLookupTextureLocation, 5);
			GL.ActiveTexture(TextureUnit.Texture5);
			GL.BindTexture(TextureTarget.Texture2D, iblLookupTexture);

			GL.Uniform1(iblSpcularCubeLocation, 6);
			GL.ActiveTexture(TextureUnit.Texture6);
			GL.BindTexture(TextureTarget.TextureCubeMap, iblData.SpecularCubeTexture);

			GL.Uniform1(iblIrradianceCubeLocation, 7);
			GL.ActiveTexture(TextureUnit.Texture7);
			GL.BindTexture(TextureTarget.TextureCubeMap, iblData.IrradianceCubeTexture);

			GL.Uniform4(cameraPosLocation, new Vector4((float) DisplayCamera.Position.X, (float) DisplayCamera.Position.Y, (float) DisplayCamera.Position.Z, 1));

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindVertexArray(0);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
	}
}