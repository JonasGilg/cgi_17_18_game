using System;
using Engine.Local;
using Engine.Model;
using Engine.Postprocessing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Engine.Render {
	public static class DeferredRendering {
		public static int GColorRoughnessBuffer;
		public static int GPositionBuffer;
		public static int GNormalBuffer;
		public static int GMetalnessShadowBuffer;
		public static int GGlowBuffer;

		private static Model3D fullscreenQuad;
		private static Model3D pointLightObject;

		private static int gFramebufferName;
		private static int width;
		private static int height;

		private static SimpleLighting simpleLighting;

		private static ImageBasedLighting ibl;

		private static BlurHorizontalMaterial horizontalBlurMaterial;
		private static BlurVerticalMaterial verticalBlurMaterial;

		private static PointLight pointLight;

		private static int pingPongFbo0;
		private static int pingPongBuffer0;

		private static int pingPongFbo1;
		private static int pingPongBuffer1;

		public static void Init(int screenWidth, int screenHeight) {
			width = screenWidth;
			height = screenHeight;

			simpleLighting = new SimpleLighting();
			ibl = new ImageBasedLighting();
			pointLight = new PointLight();

			horizontalBlurMaterial = new BlurHorizontalMaterial();
			verticalBlurMaterial = new BlurVerticalMaterial();

			// the fullscreen quad object
			fullscreenQuad = new Model3D();
			fullscreenQuad.AddTriangle(new Vector3(1, -1, 0), new Vector3(-1, -1, 0), new Vector3(1, 1, 0), new Vector2(1, 0), new Vector2(0, 0), new Vector2(1, 1));
			fullscreenQuad.AddTriangle(new Vector3(-1, -1, 0), new Vector3(1, 1, 0), new Vector3(-1, 1, 0), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1));
			fullscreenQuad.CreateVAO();

			//point light object
			pointLightObject = ModelLoaderObject3D.Load("data/objects/sphere.obj", 1.0f, true);

			gFramebufferName = 0;
			GL.GenFramebuffers(1, out gFramebufferName);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, gFramebufferName);

			GL.GenTextures(1, out GColorRoughnessBuffer);
			GL.BindTexture(TextureTarget.Texture2D, GColorRoughnessBuffer);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, screenWidth, screenHeight, 0, PixelFormat.Rgba, PixelType.UnsignedInt8888, IntPtr.Zero);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, GColorRoughnessBuffer, 0);

			GL.GenTextures(1, out GPositionBuffer);
			GL.BindTexture(TextureTarget.Texture2D, GPositionBuffer);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb32f, screenWidth, screenHeight, 0, PixelFormat.Rgb, PixelType.Float, IntPtr.Zero);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, GPositionBuffer, 0);

			GL.GenTextures(1, out GNormalBuffer);
			GL.BindTexture(TextureTarget.Texture2D, GNormalBuffer);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f, screenWidth, screenHeight, 0, PixelFormat.Rgb, PixelType.Float, IntPtr.Zero);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment2, GNormalBuffer, 0);

			GL.GenTextures(1, out GMetalnessShadowBuffer);
			GL.BindTexture(TextureTarget.Texture2D, GMetalnessShadowBuffer);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f, screenWidth, screenHeight, 0, PixelFormat.Rg, PixelType.UnsignedByte, IntPtr.Zero);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment3, GMetalnessShadowBuffer, 0);

			GL.GenTextures(1, out GGlowBuffer);
			GL.BindTexture(TextureTarget.Texture2D, GGlowBuffer);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, screenWidth, screenHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment4, GGlowBuffer, 0);


			DrawBuffersEnum[] drawEnum = {DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1, DrawBuffersEnum.ColorAttachment2, DrawBuffersEnum.ColorAttachment3, DrawBuffersEnum.ColorAttachment4};
			GL.DrawBuffers(5, drawEnum);

			GL.GenRenderbuffers(1, out int depthrenderbuffer);
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthrenderbuffer);
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, screenWidth, screenHeight);
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthrenderbuffer);


			// Ping-Pong Buffer 0
			GL.GenFramebuffers(1, out pingPongFbo0);
			GL.GenTextures(1, out pingPongBuffer0);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, pingPongFbo0);
			GL.BindTexture(TextureTarget.Texture2D, pingPongBuffer0);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, screenWidth, screenHeight, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, pingPongBuffer0, 0);

			// Ping-Pong Buffer 1
			GL.GenFramebuffers(1, out pingPongFbo1);
			GL.GenTextures(1, out pingPongBuffer1);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, pingPongFbo1);
			GL.BindTexture(TextureTarget.Texture2D, pingPongBuffer1);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, screenWidth, screenHeight, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, pingPongBuffer1, 0);


			var eCode = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
			if (eCode != FramebufferErrorCode.FramebufferComplete) {
				Console.WriteLine("GBuffer init wrong" + eCode.ToString());
			}
			else {
				Console.WriteLine("GBuffer init Correct");
			}
		}

		public static void StartGBufferRendering() {
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, gFramebufferName);
			DrawBuffersEnum[] drawEnum = {DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1, DrawBuffersEnum.ColorAttachment2, DrawBuffersEnum.ColorAttachment3, DrawBuffersEnum.ColorAttachment4};
			GL.DrawBuffers(5, drawEnum);

			GL.ClearColor(new Color4(0, 0, 0, 0));
			GL.DepthMask(true);
			GL.Enable(EnableCap.DepthTest);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Viewport(0, 0, width, height);
		}

		public static void DrawFullscreenIBL(IBLData iblData) {
			GL.Disable(EnableCap.DepthTest);
			GL.Disable(EnableCap.CullFace);

			GL.Viewport(0, 0, width, height);

			ibl.Draw(fullscreenQuad, GColorRoughnessBuffer, GNormalBuffer, GPositionBuffer, GMetalnessShadowBuffer, GGlowBuffer, iblData);

			GL.Enable(EnableCap.CullFace);
		}

		public static void CopyDepthToMainScreen() {
			// copy depth to main screen
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, gFramebufferName);
			GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
			GL.BlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.DepthBufferBit, BlitFramebufferFilter.Nearest);
			
			// reeset render target to main screen
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

			// enable z-buffer again
			GL.Enable(EnableCap.DepthTest);
		}

		public static void DrawDebugFullscreen(int mode) {
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

			GL.ClearColor(new Color4(1, 0, 0, 0));
			GL.Disable(EnableCap.DepthTest);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Disable(EnableCap.CullFace);

			GL.Viewport(0, 0, width, height);

			switch (mode) {
				case 0:
					simpleLighting.Draw(fullscreenQuad, GColorRoughnessBuffer);
					break;
				case 1:
					simpleLighting.Draw(fullscreenQuad, GNormalBuffer);
					break;
				case 2:
					simpleLighting.Draw(fullscreenQuad, GPositionBuffer);
					break;
				case 3:
					simpleLighting.Draw(fullscreenQuad, GMetalnessShadowBuffer);
					break;
				default:
					simpleLighting.Draw(fullscreenQuad, GGlowBuffer);
					break;
			}

			GL.Enable(EnableCap.CullFace);
		}

		public static void DrawPointLight(Vector3d pos, Vector3 color, float radius) {
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			pointLight.Draw(pointLightObject, pos, radius, color, width, height);
		}

		public static void PingPongBlurGlowAndDraw() {
			GL.Disable(EnableCap.CullFace);

			for (var i = 0; i < 4; i++) {
				GL.BindFramebuffer(FramebufferTarget.Framebuffer, pingPongFbo0);
				horizontalBlurMaterial.Draw(fullscreenQuad, i == 0 ? GGlowBuffer : pingPongBuffer1);

				GL.BindFramebuffer(FramebufferTarget.Framebuffer, pingPongFbo1);
				verticalBlurMaterial.Draw(fullscreenQuad, pingPongBuffer0);
			}

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			horizontalBlurMaterial.Draw(fullscreenQuad, pingPongBuffer1);
			GL.Disable(EnableCap.Blend);

			GL.Enable(EnableCap.CullFace);
		}
	}
}