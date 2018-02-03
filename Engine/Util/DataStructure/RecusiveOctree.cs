using System;
using System.Collections.Generic;
using Engine.Render;
using OpenTK;
using AABB = Engine.AxisAlignedBoundingBox;

namespace Engine {
	public class RecusiveOctree<T> : IOctree<T> where T : IOctreeItem<T> {
		private const double DEFAULT_UNIVERSE_SIZE = 150_000;
		private const double DEFAULT_CELL_SIZE = 100;

		private readonly IOctree<T> parent;
		private readonly IOctree<T>[] children;
		private readonly ISet<T> items;

		private readonly int treeMaxDepth;
		private readonly int treeCurrDepth;

		private AABB aabb;

		public RecusiveOctree(double treeSize = DEFAULT_UNIVERSE_SIZE, double cellSize = DEFAULT_CELL_SIZE) {
			treeMaxDepth = (int) Math.Ceiling(Math.Log(treeSize / cellSize, 2));
			treeCurrDepth = 0;

			aabb = AxisAlignedBoundingBox.FromCenterAndDimension(Vector3d.Zero, new Vector3d(treeSize / 2));
			parent = null;
			children = new IOctree<T>[8];
			items = new HashSet<T>();
		}

		private RecusiveOctree(AABB aabb, int treeCurrDepth, int treeMaxDepth, IOctree<T> parent) {
			this.treeCurrDepth = treeCurrDepth;
			this.treeMaxDepth = treeMaxDepth;

			this.aabb = aabb;
			this.parent = parent;
			children = new IOctree<T>[8];
			items = new HashSet<T>();
		}

		public void Insert(T item) {
			items.Add(item);

			if (treeCurrDepth < treeMaxDepth) {
				for (var i = 0; i < children.Length; i++) {
					var offset = aabb.HalfDimension;
					var childMin = aabb.Min;
					var childMax = aabb.Center;

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
						if (children[i] == null) children[i] = new RecusiveOctree<T>(childAABB, treeCurrDepth + 1, treeMaxDepth, this);
						children[i].Insert(item);
					}
				}
			}
		}

		/// <summary>
		/// Recursive removal from the octree.
		/// </summary>
		/// <param name="item"></param>
		public void Remove(T item) {
			if (items.Remove(item)) {
				for (var i = 0; i < children.Length; i++) {
					var child = children[i];
					if (child != null) {
						child.Remove(item);
						if (child.Items().Count == 0)
							children[i] = null;
					}
				}
			}
		}

		public ISet<T> Items() => items;
		public IOctree<T> Parent() => parent;
		public IOctree<T>[] Children() => children;
		public bool IsLeaf() => treeCurrDepth == treeMaxDepth;
		public AxisAlignedBoundingBox BoundingBox() => aabb;

		public Sphere BoundingSphere() => new Sphere(aabb.Center, aabb.Max.LengthFast);
	}
}