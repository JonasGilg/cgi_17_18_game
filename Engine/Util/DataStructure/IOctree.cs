using System.Collections.Immutable;
using Engine.Util;
using AABB = Engine.AxisAlignedBoundingBox;

namespace Engine {
	public interface IOctree<T> {
		void Insert(T item);
		void Remove(T item);

		IImmutableSet<T> Items();
		
		IOctree<T> Parent();
		ImmutableArray<IOctree<T>> Children();
		
		bool IsLeaf();
		
		AxisAlignedBoundingBox BoundingBox();
		Sphere BoundingSphere();
	}
	
	public interface IOctreeItem<T> {
		AABB GetAABB();
	}
}