using Engine;
using Engine.Component;
using Engine.Util;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.Components {
	public class ArcadeMoveInputComponent : MoveInputComponent {
		public ArcadeMoveInputComponent(GameObject gameObject, TransformComponent transformComponent,
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
				TransformComponent.Position += translateLeft * MOVEMENT_MULTIPLIER;
			}

			if (Keyboard.Down(Key.E)) {
				var translateRight = TransformComponent.Orientation.Rotate(new Vector3d(0.0, 0.0, Time.DeltaTimeUpdate));
				TransformComponent.Position += translateRight * MOVEMENT_MULTIPLIER;
			}

			if (Keyboard.Down(Key.Space)) {
				var translateUp = TransformComponent.Orientation.Rotate(new Vector3d(0.0, Time.DeltaTimeUpdate, 0.0));
				TransformComponent.Position += translateUp * MOVEMENT_MULTIPLIER;
			}

			if (Keyboard.Down(Key.X)) {
				var translateDown = TransformComponent.Orientation.Rotate(new Vector3d(0.0, -Time.DeltaTimeUpdate, 0.0));
				TransformComponent.Position += translateDown * MOVEMENT_MULTIPLIER;
			}

			if (Keyboard.Down(Key.A)) {
				var yawLeft = Quaterniond.FromAxisAngle(Vector3d.UnitY, Time.DeltaTimeUpdate);
				TransformComponent.Orientation *= yawLeft;
				var length = MoveComponent.LinearVelocity.Length;
				MoveComponent.LinearVelocity = TransformComponent.Orientation.Rotate(Vector3d.UnitX) * length;
			}

			if (Keyboard.Down(Key.D)) {
				var yawRight = Quaterniond.FromAxisAngle(Vector3d.UnitY, -Time.DeltaTimeUpdate);
				TransformComponent.Orientation *= yawRight;
				var length = MoveComponent.LinearVelocity.Length;
				MoveComponent.LinearVelocity = TransformComponent.Orientation.Rotate(Vector3d.UnitX) * length;
			}

			if (Keyboard.Down(Key.Up)) {
				var pitchDown = Quaterniond.FromAxisAngle(Vector3d.UnitZ, -Time.DeltaTimeUpdate);
				TransformComponent.Orientation *= pitchDown;
				var length = MoveComponent.LinearVelocity.Length;
				MoveComponent.LinearVelocity = TransformComponent.Orientation.Rotate(Vector3d.UnitX) * length;
			}

			if (Keyboard.Down(Key.Down)) {
				var pitchUp = Quaterniond.FromAxisAngle(Vector3d.UnitZ, Time.DeltaTimeUpdate);
				TransformComponent.Orientation *= pitchUp;
				var length = MoveComponent.LinearVelocity.Length;
				MoveComponent.LinearVelocity = TransformComponent.Orientation.Rotate(Vector3d.UnitX) * length;
			}

			if (Keyboard.Down(Key.Left)) {
				TransformComponent.Orientation *= Quaterniond.FromEulerAngles(0, 0, -Time.DeltaTimeUpdate);
			}

			if (Keyboard.Down(Key.Right)) {
				TransformComponent.Orientation *= Quaterniond.FromEulerAngles(0, 0, Time.DeltaTimeUpdate);
			}
		}
	}
}