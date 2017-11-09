using System;
using Engine.Util;
using Engine.Component;
using OpenTK;
using OpenTK.Input;
using Mouse = Engine.Input.Mouse;

namespace Engine.Component {
	public class ThirdPersonCameraComponent : CameraComponent {
		public Vector3d Offset;
		private Vector2d _rotation;

		public ThirdPersonCameraComponent(Vector3d offset, GameObject gameObject) : base(gameObject) {
			Offset = offset;
			_rotation = Vector2d.Zero;
		}

		public override void Update() {
			base.Update();

			if (Mouse.Down(MouseButton.Right)) {
				var factor = Time.DeltaTimeUpdate * 0.3;
				
				_rotation.X += -Mouse.CursorDelta.X * factor;

				const double maxRotation = Math.PI / 2 - 0.2;
				const double minRotation = -maxRotation;
				var deltaY = -Mouse.CursorDelta.Y * factor;
				_rotation.Y = Math1D.Clamp(_rotation.Y + deltaY, minRotation, maxRotation);
			}
			else {
				_rotation = Vector2d.Lerp(_rotation, Vector2d.Zero, Time.DeltaTimeUpdate * 10);
			}

			var rotation = Quaterniond.FromEulerAngles(_rotation.Y, _rotation.X, 0);
			
			var eyePosition = (GameObject.TransformComponent.Orientation * rotation).Rotate(Offset) + GameObject.TransformComponent.Position;

			SetLookAt(eyePosition, GameObject.TransformComponent.Position, GameObject.TransformComponent.Orientation.Rotate(Vector3d.UnitY));
		}
	}
}