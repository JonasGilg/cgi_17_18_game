using Engine.Component;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using Engine.Model;

namespace Engine {
	public delegate void Collisionhandler(Collision.Collision col);
	public abstract class CollisionComponent : Component.Component {
		public Model3D model3D;
		public abstract Collision.BoundingBox boundingBox {
			get;
		}
		public Collisionhandler onCollision;
		public CollisionComponent(GameObject gameObject, Model3D model, Collisionhandler collisionFunction) : base(gameObject) {
			model3D = model;
			onCollision = collisionFunction;
		}

		public override void Update() {
			
		}

		public bool IsColliding(CollisionComponent otherCollider) => boundingBox.Intersects(otherCollider.boundingBox);
		
	}
}
