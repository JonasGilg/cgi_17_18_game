using System.Collections.Generic;
using System.Linq;
using Engine.Component;
using Engine.Material;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Render {
	public static class RenderEngine {
		private static readonly IOctree<RenderComponent> RENDER_OCTREE;
		private static readonly ISet<RenderComponent> DYNAMIC_OBJECTS;

		public static IBLData IBLData;

		static RenderEngine() {
			RENDER_OCTREE = new RecusiveOctree<RenderComponent>();
			DYNAMIC_OBJECTS = new HashSet<RenderComponent>();
		}

		public static void RegisterStaticRenderComponent(RenderComponent renderComponent)
			=> RENDER_OCTREE.Insert(renderComponent);

		public static void RegisterDynamicRenderComponent(RenderComponent renderComponent)
			=> DYNAMIC_OBJECTS.Add(renderComponent);

		public static void UnregisterStaticRenderComponent(RenderComponent renderComponent)
			=> RENDER_OCTREE.Remove(renderComponent);

		public static void UnregisterDynamicRenderComponent(RenderComponent renderComponent)
			=> DYNAMIC_OBJECTS.Remove(renderComponent);

		public static void Draw() {
			foreach (var renderComponent in RENDER_OCTREE.Items()) {
				if (DisplayCamera.IsSphereInFrustum(renderComponent.GetBoundingSphere()) != Intersect.OUTSIDE) {
					renderComponent.Material.RegisterForDraw(renderComponent);
				}
			}

			foreach (var renderComponent in DYNAMIC_OBJECTS) {
				if (DisplayCamera.IsSphereInFrustum(renderComponent.GetBoundingSphere()) != Intersect.OUTSIDE) {
					renderComponent.Material.RegisterForDraw(renderComponent);
				}
			}

			CascadedShadowMapping.StartShadowMapping();
			for (var i = 0; i < 3; i++) {
				CascadedShadowMapping.SetDepthTextureTarget(i);
				ShadowComponent.SHADOW_MATERIAL.DrawAll();
			}

			CascadedShadowMapping.EndShadowMapping();

			DeferredRendering.StartGBufferRendering();

			MaterialManager.DrawAll();

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			GL.ClearColor(new Color4(0, 0, 0, 0));
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			DeferredRendering.DrawFullscreenIBL(IBLData);
			DeferredRendering.CopyDepthToMainScreen();

			Skybox.Skybox.Draw();

			DeferredRendering.PingPongBlurGlowAndDraw();
		}

		private static ISet<RenderComponent> GetInFrustum() {
			ISet<RenderComponent> inside = new HashSet<RenderComponent>();
			GetInFrustumRecursive(ref inside, RENDER_OCTREE);
			return inside;
		}

		private static void GetInFrustumRecursive(ref ISet<RenderComponent> inside, IOctree<RenderComponent> tree) {
			for (var i = 0; i < tree.Children().Length; i++) {
				var child = tree.Children()[i];
				if (child != null) {
					var intersect = DisplayCamera.IsSphereInFrustum(child.BoundingSphere());

					if (intersect == Intersect.INSIDE || child.IsLeaf()) {
						for (var j = 0; j < child.Items().Count; j++) {
							inside.Add(child.Items().ElementAt(j));
						}
					}
					
					else if (intersect == Intersect.OVERLAP) {
						GetInFrustumRecursive(ref inside, child);
					}
				}
			}
		}
	}
}