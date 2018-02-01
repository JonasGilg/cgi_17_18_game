using System;
using Engine;
using Engine.Component;
using Engine.Render;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;
using Mouse = Engine.Input.Mouse;

namespace Game.Components {
	public class RLSpaceMovementComponent : MoveInputComponent {
		public RLSpaceMovementComponent(GameObject gameObject, TransformComponent transformComponent,
			MoveComponent moveComponent) : base(gameObject, transformComponent, moveComponent) { }

		private Vector3d velocity = Vector3d.Zero;

		private void AddVelocity(double x, double y, double z) {
			velocity.X = Math1D.Clamp(velocity.X + x, -1, 1);
			velocity.Y = Math1D.Clamp(velocity.Y + y, -1, 1);
			velocity.Z = Math1D.Clamp(velocity.Z + z, -1, 1);
		}

		private Vector3d CalcSpeed(double speedMedian) => new Vector3d(
			Math.Cos(Math.PI * velocity.X + Math.PI) * speedMedian + speedMedian,
			Math.Cos(Math.PI * velocity.Y + Math.PI) * speedMedian + speedMedian,
			Math.Cos(Math.PI * velocity.Z + Math.PI) * speedMedian + speedMedian
		);

		public override void Update() {
			base.Update();

			var sideSpeed = Time.DeltaTimeUpdate * SIDEWAY_SPEED;
			var linearVelocity = GetLinearVelocityInObjectSpace();

			if (Keyboard.Down(Key.W))
				AddLinearVelocityInObjectSpace(Time.DeltaTimeUpdate * FORWARD_SPEED, 0, 0);
			else if (Keyboard.Down(Key.S))
				AddLinearVelocityInObjectSpace(-sideSpeed, 0, 0);
			else if (linearVelocity.X > sideSpeed)
				AddLinearVelocityInObjectSpace(-sideSpeed, 0, 0);
			else if (linearVelocity.X < -sideSpeed)
				AddLinearVelocityInObjectSpace(sideSpeed, 0, 0);
			else
				AddLinearVelocityInObjectSpace(-linearVelocity.X, 0, 0);

			if (Keyboard.Down(Key.Q))
				AddLinearVelocityInObjectSpace(0, 0, -sideSpeed);
			else if (Keyboard.Down(Key.E))
				AddLinearVelocityInObjectSpace(0, 0, sideSpeed);
			else if (linearVelocity.Z < -sideSpeed)
				AddLinearVelocityInObjectSpace(0, 0, sideSpeed);
			else if (linearVelocity.Z > sideSpeed)
				AddLinearVelocityInObjectSpace(0, 0, -sideSpeed);
			else
				AddLinearVelocityInObjectSpace(0, 0, -linearVelocity.Z);

			if (Keyboard.Down(Key.Space))
				AddLinearVelocityInObjectSpace(0, sideSpeed, 0);
			else if (Keyboard.Down(Key.X))
				AddLinearVelocityInObjectSpace(0, -sideSpeed, 0);
			else if (linearVelocity.Y > sideSpeed)
				AddLinearVelocityInObjectSpace(0, -sideSpeed, 0);
			else if (linearVelocity.Y < -sideSpeed)
				AddLinearVelocityInObjectSpace(0, sideSpeed, 0);
			else
				AddLinearVelocityInObjectSpace(0, -linearVelocity.Y, 0);

			MoveComponent.LinearVelocity = Vector3d.Lerp(MoveComponent.LinearVelocity, Vector3d.Zero, Time.DeltaTimeUpdate);

			var angularSpeed = Time.DeltaTimeUpdate * ROTATION_MULTIPLIER;
			var angularVelocity = GetAngularVelocityInObjectSpace();

			if (Keyboard.Down(Key.A))
				AddAngularVelocityInObjectSpace(0, angularSpeed, 0);
			else if (Keyboard.Down(Key.D))
				AddAngularVelocityInObjectSpace(0, -angularSpeed, 0);
			else if (angularVelocity.Y > 0)
				AddAngularVelocityInObjectSpace(0, -angularSpeed, 0);
			else if (angularVelocity.Y < 0)
				AddAngularVelocityInObjectSpace(0, angularSpeed, 0);

			if (Math.Abs(Mouse.CursorDelta.Y) > 0001)
				AddAngularVelocityInObjectSpace(0, 0, Time.DeltaTimeUpdate * Mouse.CursorDelta.Y);
			else if (Keyboard.Down(Key.Up))
				AddAngularVelocityInObjectSpace(0, 0, -angularSpeed);
			else if (Keyboard.Down(Key.Down))
				AddAngularVelocityInObjectSpace(0, 0, angularSpeed);
			else if (angularVelocity.Z < 0)
				AddAngularVelocityInObjectSpace(0, 0, angularSpeed);
			else if (angularVelocity.Z > 0)
				AddAngularVelocityInObjectSpace(0, 0, -angularSpeed);

			if (Math.Abs(Mouse.CursorDelta.X) > 0001)
				AddAngularVelocityInObjectSpace(Time.DeltaTimeUpdate * Mouse.CursorDelta.X, 0, 0);
			else if (Keyboard.Down(Key.Left))
				AddAngularVelocityInObjectSpace(-angularSpeed, 0, 0);
			else if (Keyboard.Down(Key.Right))
				AddAngularVelocityInObjectSpace(angularSpeed, 0, 0);
			else if (angularVelocity.X < 0)
				AddAngularVelocityInObjectSpace(angularSpeed, 0, 0);
			else if (angularVelocity.X > 0)
				AddAngularVelocityInObjectSpace(-angularSpeed, 0, 0);

			MoveComponent.AngularVelocity = Vector3d.Lerp(MoveComponent.AngularVelocity, Vector3d.Zero, Time.DeltaTimeUpdate);
		}
	}
}