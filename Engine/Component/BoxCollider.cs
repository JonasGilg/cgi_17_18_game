using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	class BoxCollider : CollisionComponent {
		public BoxCollider(GameObject gameObject, Collisionhandler collisionFunction) : base(gameObject, collisionFunction) {
		}

		public override bool IsColliding() {
			return false;
		}
	}
}
