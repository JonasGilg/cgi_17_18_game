using System.Collections.Generic;
using System.Linq;
using Engine.Component;
using Engine.Material;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Render {
	public static class RenderEngine {
		private static readonly TimingStats OCTREE_CULLING = new TimingStats("\tOctree Culling");
		private static readonly TimingStats FRUSTUM_CULLING = new TimingStats("\tFrustum Culling");
		private static readonly TimingStats SHADOW_MAPPING = new TimingStats("\tShadow Mapping");
		private static readonly TimingStats MATERIAL_RENDER = new TimingStats("\tMaterial Render");
		private static readonly TimingStats IBL_RENDER = new TimingStats("\tIBL Render");
		private static readonly TimingStats GLOW_DRAW = new TimingStats("\tGlow Draw");

		private static readonly IOctree<RenderComponent> RENDER_OCTREE;
		private static readonly ISet<RenderComponent> DYNAMIC_OBJECTS;

		public static IBLData IBLData;

		static RenderEngine() {
			RENDER_OCTREE = new RecusiveOctree<RenderComponent>();
			DYNAMIC_OBJECTS = new HashSet<RenderComponent>();

			TimingRegistry.AddRenderTiming(OCTREE_CULLING);
			TimingRegistry.AddRenderTiming(FRUSTUM_CULLING);
			TimingRegistry.AddRenderTiming(SHADOW_MAPPING);
			TimingRegistry.AddRenderTiming(MATERIAL_RENDER);
			TimingRegistry.AddRenderTiming(IBL_RENDER);
			TimingRegistry.AddRenderTiming(GLOW_DRAW);
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
			OCTREE_CULLING.Start();
			foreach (var renderComponent in RENDER_OCTREE.Items()) {
				if (DisplayCamera.IsSphereInFrustum(renderComponent.GetBoundingSphere()) != Intersect.OUTSIDE) {
					renderComponent.Material.RegisterForDraw(renderComponent);
				}
			}

			OCTREE_CULLING.Stop();

			FRUSTUM_CULLING.Start();
			foreach (var renderComponent in DYNAMIC_OBJECTS) {
				if (DisplayCamera.IsSphereInFrustum(renderComponent.GetBoundingSphere()) != Intersect.OUTSIDE) {
					renderComponent.Material.RegisterForDraw(renderComponent);
				}
			}

			FRUSTUM_CULLING.Stop();

			SHADOW_MAPPING.Start();
			CascadedShadowMapping.StartShadowMapping();
			for (var i = 0; i < 3; i++) {
				CascadedShadowMapping.SetDepthTextureTarget(i);
				ShadowComponent.SHADOW_MATERIAL.DrawAll();
			}

			CascadedShadowMapping.EndShadowMapping();
			SHADOW_MAPPING.Stop();

			DeferredRendering.StartGBufferRendering();

			MATERIAL_RENDER.Start();
			MaterialManager.DrawAll();
			MATERIAL_RENDER.Stop();

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			GL.ClearColor(new Color4(0, 0, 0, 0));
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			IBL_RENDER.Start();
			DeferredRendering.DrawFullscreenIBL(IBLData);
			IBL_RENDER.Stop();
			DeferredRendering.CopyDepthToMainScreen();

			Skybox.Skybox.Draw();

			GLOW_DRAW.Start();
			DeferredRendering.PingPongBlurGlowAndDraw();
			GLOW_DRAW.Stop();
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