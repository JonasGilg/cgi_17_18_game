using System.Collections.Generic;
using Engine.Util;

namespace Engine {
	public interface IOctree<T> {
		void Insert(T item);
		void Remove(T item);

		ICollection<T> Items();
		
		IOctree<T> Parent();
		IOctree<T>[] Children();
		
		bool IsLeaf();
		
		AxisAlignedBoundingBox BoundingBox();
		Sphere BoundingSphere();
	}
}