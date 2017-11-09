using Engine.Collision;
using Engine.Model;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	public class SphereCollider : CollisionComponent {
		public Vector3d center = Vector3d.Zero;
		public double radius = 0.5;
		
		public SphereCollider(GameObject gameObject, Model3D model, Collisionhandler collisionFunction) : base(gameObject, model, collisionFunction) {

		}

		public override bool IsColliding(CollisionComponent otherCollider) {
			switch (otherCollider) {
				case SphereCollider sphere:
					//this.GameObject.TransformComponent
					return false;
				case BoxCollider box:
					return false;
				default:
					return false;
			}
		}

	}
}
