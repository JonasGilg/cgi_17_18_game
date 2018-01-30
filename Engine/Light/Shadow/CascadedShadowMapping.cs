using System;
using System.Collections.Generic;
using Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine {
	public static class CascadedShadowMapping {
		private const int NUM_TEXTURES = 3;

		public struct Cascade {
			public int DepthTexture;
			public Matrix4d DepthBias;
			public Matrix4d ShadowTransformation;
			public Matrix4d ShadowProjection;

			public float CascadeXySize;
			public float CascadeZSize;
			public float CascadeDistance;
			public float BorderDistance;
		}

		public static Cascade[] Cascades { get; private set; }
		
		public static CameraComponent LightCamera;

		private static int framebufferName;

		private static Vector3d cameraStartPosition;
		private static Vector3d nearNormal;

		private static float dist1;
		private static float dist2;
		private static float dist3;
		private static float minZdist;
		private static List<int> textureSizes;

		public static Vector3d LightDirection;

		public static void Init(int textureDimension1, int textureDimension2, int textureDimension3, float distance1, float distance2, float distance3, float minZdistance) {
			LightCamera = new CameraComponent(null);
			dist1 = distance1;
			dist2 = distance2;
			dist3 = distance3;
			minZdist = minZdistance;

			Cascades = new Cascade[NUM_TEXTURES];

			textureSizes = new List<int> {textureDimension1, textureDimension2, textureDimension3};

			framebufferName = 0;
			GL.GenFramebuffers(1, out framebufferName);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebufferName);

			var depthTextures = new int[NUM_TEXTURES];
			GL.GenTextures(NUM_TEXTURES, depthTextures);

			for (var i = 0; i < NUM_TEXTURES; i++) {
				GL.BindTexture(TextureTarget.Texture2D, depthTextures[i]);
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent16, textureSizes[i], textureSizes[i], 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareFunc, (int) All.Lequal);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int) TextureCompareMode.CompareRToTexture);

				Cascades[i].DepthTexture = depthTextures[i];
			}

			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, Cascades[0].DepthTexture, 0);
			GL.DrawBuffer(DrawBufferMode.None);

			SetLightDirection(new Vector3d(1, -1, 1));
		}

		public static void StartShadowMapping() {
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebufferName);
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			cameraStartPosition = DisplayCamera.Position;
			nearNormal = DisplayCamera.Planes[0].Normal;

			const float dist0 = 0.0f;

			Cascades[0].BorderDistance = dist1;
			Cascades[1].BorderDistance = dist2;
			Cascades[2].BorderDistance = dist3;

			Cascades[0].CascadeDistance = (dist0 + dist1) * 0.5f;
			Cascades[1].CascadeDistance = (dist1 + dist2) * 0.5f;
			Cascades[2].CascadeDistance = (dist2 + dist3) * 0.5f;

			//float angleDist = (float)Math.Tan(Camera.Fov / 2.0 * Math.PI / 180.0f);
			var angleDist = (float) Math.Tan(90 / 2.0 * Math.PI / 180.0f);

			var xDist = angleDist * dist1;
			Cascades[0].CascadeXySize = (float) Math.Sqrt(xDist * xDist + xDist * xDist + (Cascades[0].CascadeDistance - dist1) * (Cascades[0].CascadeDistance - dist1));
			Cascades[0].CascadeZSize = Cascades[0].CascadeXySize;
			if (Cascades[0].CascadeZSize < minZdist) Cascades[0].CascadeZSize = minZdist;

			xDist = angleDist * dist2;
			Cascades[1].CascadeXySize = (float) Math.Sqrt(xDist * xDist + xDist * xDist + (Cascades[1].CascadeDistance - dist2) * (Cascades[1].CascadeDistance - dist2));
			Cascades[1].CascadeZSize = Cascades[1].CascadeXySize;
			if (Cascades[1].CascadeZSize < minZdist) Cascades[1].CascadeZSize = minZdist;

			xDist = angleDist * dist3;
			Cascades[2].CascadeXySize = (float) Math.Sqrt(xDist * xDist + xDist * xDist + (Cascades[2].CascadeDistance - dist3) * (Cascades[2].CascadeDistance - dist3));
			Cascades[2].CascadeZSize = Cascades[2].CascadeXySize;
			if (Cascades[2].CascadeZSize < minZdist) Cascades[2].CascadeZSize = minZdist;
		}

		public static void SetDepthTextureTarget(int target) {
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, Cascades[target].DepthTexture, 0);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Matrix4d.CreateOrthographicOffCenter(-Cascades[target].CascadeXySize, Cascades[target].CascadeXySize, -Cascades[target].CascadeXySize, Cascades[target].CascadeXySize, -Cascades[target].CascadeZSize, Cascades[target].CascadeZSize, out var ddProjection);
			LightCamera.PerspectiveProjection = ddProjection;

			GL.Viewport(0, 0, textureSizes[target], textureSizes[target]);

			var textCamPosition = cameraStartPosition + nearNormal * Cascades[target].CascadeDistance;
			LightCamera.LookAtMatrix = Matrix4d.LookAt(textCamPosition, textCamPosition - LightDirection, new Vector3d(0, 1, 0));
			LightCamera.CreateViewFrustumPlanes(LightCamera.LookAtMatrix * LightCamera.PerspectiveProjection);

			Cascades[target].ShadowTransformation = LightCamera.LookAtMatrix;
			Cascades[target].ShadowProjection = ddProjection;

			Cascades[target].DepthBias = Matrix4.CreateScale(0.5f, 0.5f, 0.5f).ToDouble();
			Cascades[target].DepthBias *= Matrix4d.CreateTranslation(Cascades[target].CascadeXySize * 0.5f, Cascades[target].CascadeXySize * 0.5f, -Cascades[target].CascadeZSize * 0.5f);
		}

		public static void EndShadowMapping() {
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Front);
		}

		public static void SetLightDirection(Vector3d direction) {
			LightDirection = direction.Normalized();
		}
	}
}