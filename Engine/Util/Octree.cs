using AABB = Engine.AxisAlignedBoundingBox;

namespace Engine {
	public class Octree<T> where T : IOctreeItem {
		
	}

	public interface IOctreeItem {
		AABB GetAABB();
	}
}