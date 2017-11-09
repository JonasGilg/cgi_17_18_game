using Engine;
using Engine.Component;
using Engine.Util;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.Components {
	public abstract class MoveInputComponent : Component {
		protected readonly TransformComponent TransformComponent;
		protected readonly MoveComponent MoveComponent;
		
		public MoveInputComponent(GameObject gameObject, TransformComponent transformComponent, MoveComponent moveComponent) : base(gameObject) {
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