using Engine;
using Engine.Component;
using Engine.Render;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.Components {
	public abstract class MoveInputComponent : Component {
		protected const double MOVEMENT_MULTIPLIER = 2000;
		protected const double ROTATION_MULTIPLIER = 5;
		protected readonly TransformComponent TransformComponent;
		protected readonly MoveComponent MoveComponent;
		
		protected void SetVelocityInObjectSpace(Vector3d velocity) {
			var newVelocity = TransformComponent.Orientation.Rotate(velocity);
			MoveComponent.LinearVelocity = newVelocity;
		}

		protected void AddLinearVelocityInObjectSpace(Vector3d velocity, double multiplier) {
			var newVelocity = TransformComponent.Orientation.Rotate(velocity);
			MoveComponent.LinearVelocity += newVelocity * multiplier;
		}

		protected Vector3d GetLinearVelocityInObjectSpace()
			=> TransformComponent.Orientation.Inverted().Rotate(MoveComponent.LinearVelocity);

		protected void AddAngularVelocityInObjectSpace(Vector3d velocity, double multiplier) {
			var newAngularVelocity = TransformComponent.Orientation.Rotate(velocity);
			MoveComponent.AngularVelocity += newAngularVelocity * multiplier;
		}

		protected Vector3d GetAngularVelocityInObjectSpace()
			=> TransformComponent.Orientation.Inverted().Rotate(MoveComponent.AngularVelocity);
		
		protected MoveInputComponent(GameObject gameObject, TransformComponent transformComponent,
			MoveComponent moveComponent) : base(gameObject) {
			TransformComponent = transformComponent;
			MoveComponent = moveComponent;
		}

		public override void Update() {
			if (Keyboard.Pressed(Key.C)) {
				MoveComponent.LinearVelocity = Vector3d.Zero;
				MoveComponent.AngularVelocity = Vector3d.Zero;
			}

			if (Keyboard.Down(Key.B)) {
				if (MoveComponent.LinearVelocity.Length > 0.05) {
					MoveComponent.LinearVelocity *= 1 - Time.DeltaTimeUpdate;
				}
				else {
					MoveComponent.LinearVelocity = Vector3d.Zero;
				}

				if (MoveComponent.AngularVelocity.Length > 0.05) {
					MoveComponent.AngularVelocity *= 1 - Time.DeltaTimeUpdate;
				}
				else {
					MoveComponent.AngularVelocity = Vector3d.Zero;
				}
			}
		}
	}
}