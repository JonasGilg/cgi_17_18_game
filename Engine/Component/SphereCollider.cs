using Engine.Collision;
using Engine.Model;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Util;

namespace Engine {
	public class SphereCollider : CollisionComponent {
		public Vector3d center = Vector3d.Zero;
		public double radiusFactor = 1.0;
		public double radiusLen;
		
		public SphereCollider(GameObject gameObject, Model3D model, Collisionhandler collisionFunction) : base(gameObject, model, collisionFunction) {
			var tmpLen = 0.0; //((double)(this.model3D.Positions).Max(p => p.Length)) * radiusFactor;
			foreach(var pos in this.model3D.Positions) {
				var calculatedLen = (pos * this.GameObject.TransformComponent.Scale.ToFloat()).Length * radiusFactor;
				if (calculatedLen > tmpLen) tmpLen = calculatedLen;
			}
			radiusLen = tmpLen;
			System.Console.WriteLine(radiusLen);
		}

		public override bool IsColliding(CollisionComponent otherCollider) {
			switch (otherCollider) {
				case SphereCollider sphere:
					var veclen = (this.GameObject.TransformComponent.WorldPosition - sphere.GameObject.TransformComponent.WorldPosition).Length;
					//if(GameObject.GetType().Name.Contains("pace") ) System.Console.WriteLine(veclen);
					return veclen < (radiusLen + sphere.radiusLen);
				case BoxCollider box:
					return false;
				default:
					return false;
			}
		}

	}
}
