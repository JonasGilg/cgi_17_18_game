using System;
using Engine;
using Engine.Render;
using OpenTK;

namespace Game.Components {
	public class GravityMovement : MoveComponent {
		private readonly double distance;
		private const double SPEED = 1.0;
		private double currentAngle;
		private readonly Vector3d axis;
		private readonly Vector3d direction;

		public GravityMovement(GameObject gameObject, Vector3d rotationAxis, Vector3d direction, double startAngle, double distance) : base(gameObject) {
			LinearVelocity = Vector3d.Zero;
			currentAngle = startAngle;
			this.distance = distance;
			axis = rotationAxis;
			this.direction = direction;
		}

		public override void Update() {
			ComputeGravitationalMovement();
			base.Update();
		}

		private void ComputeGravitationalMovement() {
			GameObject.TransformComponent.Position.X = Math.Cos(currentAngle) * distance;
			GameObject.TransformComponent.Position.Z = Math.Sin(currentAngle) * distance;

			GameObject.TransformComponent.Position = Quaterniond.FromAxisAngle(axis, currentAngle).Rotate(direction * distance); // + GameObject.TransformComponent.Parent.Position;

			currentAngle += SPEED * Time.DeltaTimeUpdate;

			if (currentAngle > MathHelper.TwoPi)
				currentAngle -= MathHelper.TwoPi;
		}
	}
}