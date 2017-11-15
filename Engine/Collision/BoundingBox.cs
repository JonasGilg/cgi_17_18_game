using OpenTK;

namespace Engine.Collision {
	public class BoundingBox {
		public Vector3d MinVec;
		public Vector3d MaxVec;

		public bool Intersects(BoundingBox bbox) {
			return false;
		}
	}
}