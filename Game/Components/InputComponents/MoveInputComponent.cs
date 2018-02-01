using Engine;
using Engine.Component;
using Engine.Render;
using Engine.Util;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.Components {
	public abstract class MoveInputComponent : Component {
		protected const double FORWARD_SPEED = 2000;
		protected const double SIDEWAY_SPEED = 500;
		protected const double ROTATION_MULTIPLIER = 5;
		protected readonly TransformComponent TransformComponent;
		public readonly MoveComponent MoveComponent;
		
		protected void SetLinearVelocityInObjectSpace(Vector3d velocity) {
			var newVelocity = TransformComponent.Orientation.Rotate(velocity);
			MoveComponent.LinearVelocity = newVelocity;
		}

		protected void SetLinearVelocityInObjectSpace(double x, double y, double z) => SetLinearVelocityInObjectSpace(new Vector3d(x, y, z));

		protected void AddLinearVelocityInObjectSpace(Vector3d velocity) {
			var newVelocity = TransformComponent.Orientation.Rotate(velocity);
			MoveComponent.LinearVelocity += newVelocity;
		}

		protected void AddLinearVelocityInObjectSpace(double x, double y, double z) => AddLinearVelocityInObjectSpace(new Vector3d(x, y, z));

		public Vector3d GetLinearVelocityInObjectSpace()
			=> TransformComponent.Orientation.Inverted().Rotate(MoveComponent.LinearVelocity);

		protected void AddAngularVelocityInObjectSpace(Vector3d velocity) {
			var newAngularVelocity = TransformComponent.Orientation.Rotate(velocity);
			MoveComponent.AngularVelocity += newAngularVelocity;
		}

		protected void AddAngularVelocityInObjectSpace(double x, double y, double z) => AddAngularVelocityInObjectSpace(new Vector3d(x, y, z));

		public Vector3d GetAngularVelocityInObjectSpace()
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

			if (Keyboard.Pressed(Key.V)) {
				var pos = GameObject.TransformComponent.Position;
				var formatCoord = "" + (int)pos.X + "," + (int)pos.Y + "," + (int)pos.Z;
				IO.PrintAsync(formatCoord);
			}
		}
	}
}