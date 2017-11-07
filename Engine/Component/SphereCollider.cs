using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Component {
	class SphereCollider : CollisionComponent {

		public SphereCollider(GameObject gameObject, Collisionhandler collisionFunction) : base(gameObject, collisionFunction) {

		}

		public override bool IsColliding() {
			return false;
		}
	}
}
