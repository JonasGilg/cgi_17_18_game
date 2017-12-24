using System;
using Engine;
using Engine.Render;
using OpenTK;

namespace Game.Components {
	public class GravityMovement : MoveComponent {
		private const double DISTANCE = 4000;
		private const double SPEED = 1.0 / 5.0;
		public double currentAngle;

		public GravityMovement(GameObject gameObject, double currentAngle) : base(gameObject) {
			LinearVelocity = Vector3d.Zero;
			this.currentAngle = currentAngle;
		}

		public override void Update() {
			ComputeGravitationalMovement();
			base.Update();
		}

		private void ComputeGravitationalMovement() {
			GameObject.TransformComponent.Position.X = Math.Cos(currentAngle) * DISTANCE;
			GameObject.TransformComponent.Position.Z = Math.Sin(currentAngle) * DISTANCE;

			currentAngle += SPEED * Time.DeltaTimeUpdate;

			if (currentAngle > 2 * Math.PI)
				currentAngle -= 2 * Math.PI;
		}
	}
}