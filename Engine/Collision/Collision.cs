using OpenTK;

namespace Engine.Collision {
	public class Collision {
		public GameObject otherGameObject;
		public CollisionComponent OtherCollisonComponent;
		public Vector3d PointOfContact = Vector3d.Zero;
	}
}