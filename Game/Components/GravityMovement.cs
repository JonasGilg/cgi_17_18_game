using System;
using Engine;
using Engine.Render;
using OpenTK;

namespace Game.Components {
	public class GravityMovement : MoveComponent {
		public double DISTANCE = 4000;
		public double SPEED = 1.0;
		public double currentAngle;
		public Vector3d Axis;
		public Vector3d Direction;
		
		

		public GravityMovement(GameObject gameObject, Vector3d rotationAxis, Vector3d direction, double startAngle, double distance) : base(gameObject) {
			LinearVelocity = Vector3d.Zero;
			currentAngle = startAngle;
			DISTANCE = distance;
			Axis = rotationAxis;
			Direction = direction;
		}

		public override void Update() {
			ComputeGravitationalMovement();
			base.Update();
		}

		private void ComputeGravitationalMovement() {
			GameObject.TransformComponent.Position.X = Math.Cos(currentAngle) * DISTANCE;
			GameObject.TransformComponent.Position.Z = Math.Sin(currentAngle) * DISTANCE;

			GameObject.TransformComponent.Position = Quaterniond.FromAxisAngle(Axis, currentAngle).Rotate(Direction * DISTANCE);// + GameObject.TransformComponent.Parent.Position;

			currentAngle += SPEED * Time.DeltaTimeUpdate;

			if (currentAngle > MathHelper.TwoPi)
				currentAngle -= MathHelper.TwoPi;
		}
	}
}