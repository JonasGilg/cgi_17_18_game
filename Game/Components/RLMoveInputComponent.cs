using System;
using Engine;
using Engine.Component;
using Engine.Input;
using Engine.Render;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;
using Mouse = Engine.Input.Mouse;

namespace Game.Components {
	public class RLSpaceMovementComponent : MoveInputComponent {
		public RLSpaceMovementComponent(GameObject gameObject, TransformComponent transformComponent,
			MoveComponent moveComponent) : base(gameObject, transformComponent, moveComponent) { }

		public override void Update() {
			base.Update();

			var sideSpeed = Time.DeltaTimeUpdate * SIDEWAY_SPEED;
			var linearVelocity = GetLinearVelocityInObjectSpace();

			var triggerState = Controller.RightTrigger - Controller.LeftTrigger;
			
			if(Math.Abs(triggerState) > double.Epsilon)
				AddLinearVelocityInObjectSpace(Time.DeltaTimeUpdate * FORWARD_SPEED * triggerState, 0, 0);
			else if (Keyboard.Down(Key.W))
				AddLinearVelocityInObjectSpace(Time.DeltaTimeUpdate * FORWARD_SPEED, 0, 0);
			else if (Keyboard.Down(Key.S))
				AddLinearVelocityInObjectSpace(-Time.DeltaTimeUpdate * FORWARD_SPEED, 0, 0);
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
			
			if(Math.Abs(Controller.RightStick.X) > 0.001)
				AddAngularVelocityInObjectSpace(0, -Time.DeltaTimeUpdate * Controller.RightStick.X * 5, 0);
			else if (Keyboard.Down(Key.A))
				AddAngularVelocityInObjectSpace(0, angularSpeed, 0);
			else if (Keyboard.Down(Key.D))
				AddAngularVelocityInObjectSpace(0, -angularSpeed, 0);
			else if (angularVelocity.Y > 0)
				AddAngularVelocityInObjectSpace(0, -angularSpeed, 0);
			else if (angularVelocity.Y < 0)
				AddAngularVelocityInObjectSpace(0, angularSpeed, 0);

			if(Math.Abs(Controller.LeftStick.Y) > 0.001)
				AddAngularVelocityInObjectSpace(0, 0, -Time.DeltaTimeUpdate * Controller.LeftStick.Y * 5);
			else if (Math.Abs(Mouse.CursorDelta.Y) > 0.001 && !Mouse.Down(MouseButton.Right))
				AddAngularVelocityInObjectSpace(0, 0, Time.DeltaTimeUpdate * Mouse.CursorDelta.Y);
			else if (Keyboard.Down(Key.Up))
				AddAngularVelocityInObjectSpace(0, 0, -angularSpeed);
			else if (Keyboard.Down(Key.Down))
				AddAngularVelocityInObjectSpace(0, 0, angularSpeed);
			else if (angularVelocity.Z < 0)
				AddAngularVelocityInObjectSpace(0, 0, angularSpeed);
			else if (angularVelocity.Z > 0)
				AddAngularVelocityInObjectSpace(0, 0, -angularSpeed);
			
			if(Math.Abs(Controller.LeftStick.X) > 0.001)
				AddAngularVelocityInObjectSpace(Time.DeltaTimeUpdate * Controller.LeftStick.X * 4, 0, 0);
			else if (Math.Abs(Mouse.CursorDelta.X) > 0.001 && !Mouse.Down(MouseButton.Right))
				AddAngularVelocityInObjectSpace(Time.DeltaTimeUpdate * Mouse.CursorDelta.X * 0.8, 0, 0);
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