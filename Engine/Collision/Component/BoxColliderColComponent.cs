using Engine.Collision;
using Engine.Collision.Component;
using Engine.Model;
using OpenTK;

namespace Engine {
	class BoxColliderColComponent : CollisionComponent {
		public Vector3d Center = Vector3d.Zero;
		public Vector3d Size = Vector3d.One;

		public BoxColliderColComponent(GameObject gameObject,
			Model3D model, 
			ExecuteFunction passiveCollisionFunctions = null,
			ExecuteFunction activeCollisionFunctions = null,
			PhysicsMaterial physicsMaterial = null
			) 
				: base(gameObject, model,
					passiveCollisionFunctions, activeCollisionFunctions, physicsMaterial ) { }

		public override bool IsColliding(CollisionComponent otherCollider) {
			switch (otherCollider) {
				case SphereCollider sphere:
					return false;
				case BoxColliderColComponent box:
					return false;
				default:
					return false;
			}
		}
	}
}