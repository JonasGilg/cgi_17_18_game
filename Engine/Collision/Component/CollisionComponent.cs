using Engine.Collision;
using Engine.Model;

namespace Engine {
	public delegate void Collisionhandler(Collision.Collision col);

	public abstract class CollisionComponent : Component.Component {
		protected readonly Model3D Model3D;
		public PhysicsMaterial PhysicsMaterial;
		public readonly Collisionhandler OnCollision;

		protected CollisionComponent(GameObject gameObject, Model3D model, Collisionhandler collisionFunction) :
			base(gameObject) {
			Model3D = model;
		
			OnCollision = collisionFunction;
		}

		public override void Update() { }
		public void Register() {
			World.registerCollisionComponent(this);
		}

		public void Unregister() {
			World.unregisterCollisionComponent(this);
		}

		public abstract bool IsColliding(CollisionComponent otherCollider);
	}
}