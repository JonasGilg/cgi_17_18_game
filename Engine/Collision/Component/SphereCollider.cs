using Engine.Model;

namespace Engine {
	public class SphereCollider : CollisionComponent {
		public SphereCollider(GameObject gameObject, Model3D model, Collisionhandler collisionFunction) : base(gameObject,
			model, collisionFunction) { }

		public override bool IsColliding(CollisionComponent otherCollider) {
			switch (otherCollider) {
				case SphereCollider sphere:
					var veclen = (GameObject.TransformComponent.WorldPosition
					              - sphere.GameObject.TransformComponent.WorldPosition).Length;

					return veclen < (GameObject.Radius + sphere.GameObject.Radius);
				case BoxCollider box:
					return false;
				default:
					return false;
			}
		}
	}
}