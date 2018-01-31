using Engine;
using Engine.Component;
using Engine.Render;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.Components {
	public class RLSpaceMovementComponent : MoveInputComponent {
		public RLSpaceMovementComponent(GameObject gameObject, TransformComponent transformComponent,
			MoveComponent moveComponent) : base(gameObject, transformComponent, moveComponent) { }

		public override void Update() {
			base.Update();

			if (Keyboard.Down(Key.W)) {
				var translateForward = TransformComponent.Orientation.Rotate(new Vector3d(Time.DeltaTimeUpdate, 0.0, 0.0));
				MoveComponent.LinearVelocity += translateForward * MOVEMENT_MULTIPLIER;
			}

			if (Keyboard.Down(Key.S)) {
				var translateBack = TransformComponent.Orientation.Rotate(new Vector3d(-Time.DeltaTimeUpdate, 0.0, 0.0));
				MoveComponent.LinearVelocity += translateBack * MOVEMENT_MULTIPLIER;
			}

			if (Keyboard.Down(Key.Q)) {
				var translateLeft = TransformComponent.Orientation.Rotate(new Vector3d(0.0, 0.0, -Time.DeltaTimeUpdate));
				MoveComponent.LinearVelocity += translateLeft * MOVEMENT_MULTIPLIER;
			}

			if (Keyboard.Down(Key.E)) {
				var translateRight = TransformComponent.Orientation.Rotate(new Vector3d(0.0, 0.0, Time.DeltaTimeUpdate));
				MoveComponent.LinearVelocity += translateRight * MOVEMENT_MULTIPLIER;
			}

			if (Keyboard.Down(Key.Space)) {
				var translateUp = TransformComponent.Orientation.Rotate(new Vector3d(0.0, Time.DeltaTimeUpdate, 0.0));
				MoveComponent.LinearVelocity += translateUp * MOVEMENT_MULTIPLIER;
			}

			if (Keyboard.Down(Key.X)) {
				var translateDown = TransformComponent.Orientation.Rotate(new Vector3d(0.0, -Time.DeltaTimeUpdate, 0.0));
				MoveComponent.LinearVelocity += translateDown * MOVEMENT_MULTIPLIER;
			}

			MoveComponent.LinearVelocity = Vector3d.Lerp(MoveComponent.LinearVelocity, Vector3d.Zero, Time.DeltaTimeUpdate);
			
			if (Keyboard.Down(Key.A)) {
				var yawLeft = TransformComponent.Orientation.Rotate(new Vector3d(0.0, Time.DeltaTimeUpdate, 0.0));
				MoveComponent.AngularVelocity += yawLeft * ROTATION_MULTIPLIER;
			}

			if (Keyboard.Down(Key.D)) {
				var yawRight = TransformComponent.Orientation.Rotate(new Vector3d(0.0, -Time.DeltaTimeUpdate, 0.0));
				MoveComponent.AngularVelocity += yawRight * ROTATION_MULTIPLIER;
			}

			if (Keyboard.Down(Key.Up)) {
				var forwardUp = TransformComponent.Orientation.Rotate(new Vector3d(0.0, 0.0, -Time.DeltaTimeUpdate));
				MoveComponent.AngularVelocity += forwardUp * ROTATION_MULTIPLIER * 2;
			}

			if (Keyboard.Down(Key.Down)) {
				var pitchDown = TransformComponent.Orientation.Rotate(new Vector3d(0.0, 0.0, Time.DeltaTimeUpdate));
				MoveComponent.AngularVelocity += pitchDown * ROTATION_MULTIPLIER * 2;
			}

			if (Keyboard.Down(Key.Left)) {
				var rollLeft = TransformComponent.Orientation.Rotate(new Vector3d(-Time.DeltaTimeUpdate, 0.0, 0.0));
				MoveComponent.AngularVelocity += rollLeft * ROTATION_MULTIPLIER * 2;
			}

			if (Keyboard.Down(Key.Right)) {
				var rollRight = TransformComponent.Orientation.Rotate(new Vector3d(Time.DeltaTimeUpdate, 0.0, 0.0));
				MoveComponent.AngularVelocity += rollRight * ROTATION_MULTIPLIER * 2;
			}

			MoveComponent.AngularVelocity = Vector3d.Lerp(MoveComponent.AngularVelocity, Vector3d.Zero, Time.DeltaTimeUpdate);
		}
	}
}