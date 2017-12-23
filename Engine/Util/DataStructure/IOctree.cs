using System.Collections.Generic;
using Engine.Util;
using AABB = Engine.AxisAlignedBoundingBox;

namespace Engine {
	public interface IOctree<T> {
		void Insert(T item);
		void Remove(T item);

		ISet<T> Items();
		
		IOctree<T> Parent();
		IOctree<T>[] Children();
		
		bool IsLeaf();
		
		AxisAlignedBoundingBox BoundingBox();
		Sphere BoundingSphere();
	}
	
	public interface IOctreeItem<T> {
		AABB GetAABB();
	}
}