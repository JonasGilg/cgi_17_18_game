using System;
using Engine.Model;
using Engine.Util;
using OpenTK;

namespace Engine {
	public class SphereCollider : CollisionComponent {
		public Vector3d Center = Vector3d.Zero;
		private const double RadiusFactor = 1.0;
		private readonly double _radiusLen;

		public SphereCollider(GameObject gameObject, Model3D model, Collisionhandler collisionFunction) : base(gameObject,
			model, collisionFunction) {
			var maxLength = 0.0; //((double)(this.model3D.Positions).Max(p => p.Length)) * radiusFactor;
			foreach (var pos in Model3D.Positions) {
				var calculatedLen = pos.Length * RadiusFactor;
				if (calculatedLen > maxLength) maxLength = calculatedLen;
			}
			_radiusLen = maxLength;
		}

		public override bool IsColliding(CollisionComponent otherCollider) {
			switch (otherCollider) {
				case SphereCollider sphere:
					var veclen = (GameObject.TransformComponent.WorldPosition - sphere.GameObject.TransformComponent.WorldPosition)
						.Length;
					//if(GameObject.GetType().Name.Contains("pace") ) System.Console.WriteLine(veclen);

					if (veclen < (_radiusLen + sphere._radiusLen)) {
						Console.Out.WriteLine(GameObject.TransformComponent.WorldPosition);
						Console.Out.WriteLine(sphere.GameObject.TransformComponent.WorldPosition);
						Console.Out.WriteLine(veclen);
					}
					
					return veclen < (_radiusLen * GameObject.TransformComponent.Scale.Length + sphere._radiusLen * sphere.GameObject.TransformComponent.Scale.Length);
				case BoxCollider box:
					return false;
				default:
					return false;
			}
		}
	}
}