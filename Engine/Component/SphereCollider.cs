using Engine.Collision;
using Engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Component {
	class SphereCollider : CollisionComponent {
		public override BoundingBox boundingBox {
			get {
				return null;
			}
		}
		public SphereCollider(GameObject gameObject, Model3D model, Collisionhandler collisionFunction) : base(gameObject, model, collisionFunction) {

		}

	}
}
