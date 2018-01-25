﻿﻿using System.Collections.Generic;
using System.Linq;
using Engine.Component;
using Engine.Material;

namespace Engine.Render {
	public static class RenderEngine {
		private static readonly IOctree<RenderComponent> RENDER_OCTREE;

		static RenderEngine() {
			RENDER_OCTREE = new RecusiveOctree<RenderComponent>();
		}

		public static void RegisterRenderComponent(RenderComponent renderComponent) 
			=> RENDER_OCTREE.Insert(renderComponent);

		public static void UnregisterRenderComponent(RenderComponent renderComponent)
			=> RENDER_OCTREE.Remove(renderComponent);

		public static void Draw() {
			var inView = RENDER_OCTREE.Items();

			foreach (var renderComponent in inView) {
				if (DisplayCamera.IsSphereInFrustum(renderComponent.GetBoundingSphere()) != Intersect.OUTSIDE) {
					renderComponent.Material.RegisterForDraw(renderComponent);
				}
			}
			
			MaterialManager.DrawAll();
		}

		private static HashSet<RenderComponent> GetInFrustum() {
			var inside = new HashSet<RenderComponent>();
			GetInFrustumRecursive(inside, RENDER_OCTREE);
			return inside;
		}

		private static void GetInFrustumRecursive(ISet<RenderComponent> inside, IOctree<RenderComponent> tree) {
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
						GetInFrustumRecursive(inside, child);
					}
				}
			}
		}
	}
}