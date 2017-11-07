using Engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Collision;

namespace Engine {
	class BoxCollider : CollisionComponent {
		public override BoundingBox boundingBox {
			get {
				return null;
			}
		}
		public BoxCollider(GameObject gameObject, Model3D model, Collisionhandler collisionFunction) : base(gameObject, model, collisionFunction) {
		}

	}
}
