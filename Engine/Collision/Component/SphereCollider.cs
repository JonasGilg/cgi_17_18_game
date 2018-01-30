using Engine.Collision;
using Engine.Collision.Component;
using Engine.Model;

namespace Engine {
	public class SphereCollider : CollisionComponent {
		public SphereCollider(GameObject gameObject,
			Model3D model,
			ExecuteFunction passiveCollisionFunctions = null,
			ExecuteFunction activeCollisionFunctions = null,
			
			PhysicsMaterial physicsMaterial = null)
			: base(gameObject, model,
				passiveCollisionFunctions,
				activeCollisionFunctions,
				physicsMaterial) {
			
		}

		public override bool IsColliding(CollisionComponent otherCollider) {
			switch (otherCollider) {
				case SphereCollider sphere:
					var veclen = (GameObject.TransformComponent.WorldPosition
					              - sphere.GameObject.TransformComponent.WorldPosition).Length;

					return veclen < (GameObject.Radius + sphere.GameObject.Radius);
				case BoxColliderColComponent box:
					return false;
				default:
					return false;
			}
		}
	}
}