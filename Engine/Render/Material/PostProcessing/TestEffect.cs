using System;
using Engine.Output;
using Engine.Render;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public class TestEffect : PostProcessingMaterial {
		private readonly int program;
		private readonly int fboTexture;

		private readonly float[] fboVertices = {
			-1, -1,
			1, -1,
			-1, 1,
			1, 1
		};

		private readonly int vboFboVertices;
		private readonly int attributeVCoordPostproc;
		private readonly int uniformFboTexture;
		private readonly int uniformOffset;

		public TestEffect() {
			program = ShaderLoader.LoadShader("Render/Material/PostProcessing/test_effect_VS.glsl",
				"Render/Material/PostProcessing/test_effect_FS.glsl");

			GL.BindAttribLocation(program, 0, "v_coord");
			attributeVCoordPostproc = GL.GetAttribLocation(program, "v_coord");
			GL.LinkProgram(program);

			uniformFboTexture = GL.GetUniformLocation(program, "fbo_texture");
			uniformOffset = GL.GetUniformLocation(program, "offset");

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.GenTextures(1, out fboTexture);
			GL.BindTexture(TextureTarget.Texture2D, fboTexture);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Display.DisplayWidth, Display.DisplayHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			GL.GenRenderbuffers(1, out int rboDepth);
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rboDepth);
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent16, Display.DisplayWidth, Display.DisplayHeight);
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

			GL.GenFramebuffers(1, out int fbo);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, fboTexture, 0);
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, rboDepth);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

			GL.GenBuffers(1, out vboFboVertices);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vboFboVertices);
			GL.BufferData(BufferTarget.ArrayBuffer, fboVertices.Length, fboVertices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public override void Draw() {
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			GL.ClearColor(new Color4(0, 0, 0, 1));
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.UseProgram(program);
			GL.BindTexture(TextureTarget.Texture2D, fboTexture);
			GL.Uniform1(uniformFboTexture, 0);
			GL.Uniform1(uniformOffset, (float) Time.TotalTime);
			GL.EnableVertexAttribArray(attributeVCoordPostproc);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vboFboVertices);
			GL.VertexAttribPointer(attributeVCoordPostproc, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
			GL.DisableVertexAttribArray(attributeVCoordPostproc);
		}
	}
}