using System;
using System.Collections.Generic;
using OpenTK;
using AABB = Engine.AxisAlignedBoundingBox;

namespace Engine {
	public class Octree<T> where T : IOctreeItem {
		private const double UNIVERSE_SIZE = 1_000_000;
		private const double SMALLEST_CELL_SIZE = 10;

		public readonly Octree<T>[] Children;
		protected internal readonly List<T> Items;

		public readonly int TreeMaxDepth;
		public readonly int TreeCurrDepth;

		protected internal AABB AABB;

		public Octree(double treeSize = UNIVERSE_SIZE, double cellSize = SMALLEST_CELL_SIZE) {
			TreeMaxDepth = (int) Math.Ceiling(Math.Log(treeSize / cellSize, 2));
			TreeCurrDepth = 0;

			AABB = AxisAlignedBoundingBox.FromCenterAndDimension(Vector3d.Zero, new Vector3d(treeSize / 2));
			Children = new Octree<T>[8];
			Items = new List<T>();
		}

		public Octree(AABB aabb, int treeCurrDepth, int treeMaxDepth) {
			TreeCurrDepth = treeCurrDepth;
			TreeMaxDepth = treeMaxDepth;

			AABB = aabb;
			Children = new Octree<T>[8];
			Items = new List<T>();
		}

		public void AddItem(T item) {
			Items.Add(item);
			
			if (TreeCurrDepth < TreeMaxDepth) {
				for (var i = 0; i < Children.Length; i++) {
					var offset = AABB.HalfDimension;
					var childMin = AABB.Min;
					var childMax = AABB.Center;

					if (i % 2 == 1) {
						childMin.X += offset.X;
						childMax.X += offset.X;
					}
					if ((i / 2) % 2 == 1) {
						childMin.Y += offset.Y;
						childMax.Y += offset.Y;
					}
					if (i >= 4) {
						childMin.Z += offset.Z;
						childMax.Z += offset.Z;
					}

					var childAABB = AABB.FromMinMax(childMin, childMax);

					if (childAABB.Overlaps(item.GetAABB())) {
						if (Children[i] == null) Children[i] = new Octree<T>(childAABB, TreeCurrDepth + 1, TreeMaxDepth);
						Children[i].AddItem(item);
					}
				}
			}
		}

		public void RemoveItem(T item) {
			if (Items.Remove(item)) {
				for (var i = 0; i < Children.Length; i++) {
					Children[i]?.RemoveItem(item);
				}
			}
		}

		/*public void UpdateItem(T item) {
			for (int i = 0; i < Children.Length; i++) {
				var child = Children[i];
				if (child != null) {
					if (child.Items.Contains(item)) {
						if(child.)
					}
				}
			}
		}*/
	}

	public interface IOctreeItem {
		AABB GetAABB();
	}
}