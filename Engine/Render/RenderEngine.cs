using System;
using System.Collections.Generic;
using Engine.Component;
using Engine.Material;

namespace Engine.Render {
	public static class RenderEngine {
		private static readonly Octree<RenderComponent> RENDER_OCTREE;

		static RenderEngine() {
			RENDER_OCTREE = new Octree<RenderComponent>();
		}

		public static void RegisterRenderComponent(RenderComponent renderComponent) 
			=> RENDER_OCTREE.AddItem(renderComponent);

		public static void UnregisterRenderComponent(RenderComponent renderComponent)
			=> RENDER_OCTREE.RemoveItem(renderComponent);

		public static void Draw() {
			var inView = /* GetInFrustum();*/ RENDER_OCTREE.Items;

			var counter = 0;
			var objects = "";
			foreach (var renderComponent in inView) {
				if (DisplayCamera.IsAABBInFrustum(renderComponent.GetAABB()) != Intersect.OUTSIDE) {
					renderComponent.Material.RegisterForDraw(renderComponent);
					counter++;
					if (renderComponent.Material.GetType() == typeof(NormalMappingMaterial))
						objects += renderComponent.GetAABB();
					/*objects += " " + renderComponent.GameObject.ToString() + " ";*/
				}
			}
			Console.Out.WriteLine(counter+":"+objects);
			
			MaterialManager.DrawAll();
		}

		private static HashSet<RenderComponent> GetInFrustum() {
			var inside = new HashSet<RenderComponent>();
			GetInFrustumRecursive(inside, RENDER_OCTREE);
			return inside;
		}

		private static void GetInFrustumRecursive(ISet<RenderComponent> inside, Octree<RenderComponent> tree) {
			for (var i = 0; i < tree.Children.Length; i++) {
				var child = tree.Children[i];
				if (child != null) {
					var intersect = DisplayCamera.IsAABBInFrustum(child.AABB);

					if (intersect == Intersect.INSIDE || child.TreeCurrDepth == child.TreeMaxDepth) {
						for (var j = 0; j < child.Items.Count; j++) {
							inside.Add(child.Items[j]);
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