using Engine.Model;

namespace Engine {
	public delegate void Collisionhandler(Collision.Collision col);

	public abstract class CollisionComponent : Component.Component {
		protected readonly Model3D Model3D;
		public readonly Collisionhandler OnCollision;

		protected CollisionComponent(GameObject gameObject, Model3D model, Collisionhandler collisionFunction) :
			base(gameObject) {
			Model3D = model;
			OnCollision = collisionFunction;
		}

		public override void Update() { }

		public abstract bool IsColliding(CollisionComponent otherCollider);
	}
}