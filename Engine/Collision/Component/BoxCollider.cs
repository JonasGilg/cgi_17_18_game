using Engine.Model;
using OpenTK;

namespace Engine {
	class BoxCollider : CollisionComponent {
		public Vector3d Center = Vector3d.Zero;
		public Vector3d Size = Vector3d.One;

		public BoxCollider(GameObject gameObject, Model3D model, Collisionhandler collisionFunction) : base(gameObject, model,
			collisionFunction) { }

		public override bool IsColliding(CollisionComponent otherCollider) {
			switch (otherCollider) {
				case SphereCollider sphere:
					return false;
				case BoxCollider box:
					return false;
				default:
					return false;
			}
		}
	}
}