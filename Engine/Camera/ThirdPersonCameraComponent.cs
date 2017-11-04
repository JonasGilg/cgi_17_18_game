using System;
using Engine.Util;
using OpenTK;
using OpenTK.Input;
using Mouse = Engine.Input.Mouse;

namespace Engine {
	public class ThirdPersonCameraComponent : CameraComponent {
		public Vector3 Offset;
		private Vector2 _rotation;

		public ThirdPersonCameraComponent(Vector3 offset, GameObject gameObject) : base(gameObject) {
			Offset = offset;
			_rotation = Vector2.Zero;
		}

		public override void Update() {
			base.Update();

			if (Mouse.Down(MouseButton.Right)) {
				var factor = Time.DeltaTime * 0.1f;
				
				_rotation.X += -Mouse.CursorDelta.X * factor;

				const float maxRotation = (float) (Math.PI / 2 - 0.2f);
				const float minRotation = -maxRotation;
				var deltaY = -Mouse.CursorDelta.Y * factor;
				_rotation.Y = Math1D.Clamp(_rotation.Y + deltaY, minRotation, maxRotation);
			}
			else {
				_rotation = Vector2.Lerp(_rotation, Vector2.Zero, Time.DeltaTime * 5);
			}

			var rotation = Quaternion.FromEulerAngles(_rotation.Y, _rotation.X, 0f);
			var eyePosition = GameObject.TransformComponent.Orientation * rotation * Offset;

			eyePosition += GameObject.TransformComponent.Position;

			SetLookAt(eyePosition, GameObject.TransformComponent.Position, GameObject.TransformComponent.Orientation * Vector3.UnitY);
		}
	}
}