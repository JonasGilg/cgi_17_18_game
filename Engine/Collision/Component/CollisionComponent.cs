using Engine.Collision;
using Engine.Model;

namespace Engine {
	public delegate void Collisionhandler(Collision.Collision col);

	public abstract class CollisionComponent : Component.Component {
		private readonly Model3D model3D;
		public PhysicsMaterial PhysicsMaterial;
		public readonly Collisionhandler OnCollision;

		protected CollisionComponent(GameObject gameObject, Model3D model, Collisionhandler collisionFunction) :
			base(gameObject) {
			model3D = model;

			OnCollision = collisionFunction;
		}

		public override void Update() { }

		public void Register() {
			World.RegisterCollisionComponent(this);
		}

		public void Unregister() {
			World.UnregisterCollisionComponent(this);
		}

		public abstract bool IsColliding(CollisionComponent otherCollider);
	}
}