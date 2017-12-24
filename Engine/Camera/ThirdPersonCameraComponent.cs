using System;
using Engine.Render;
using OpenTK;
using OpenTK.Input;
using Mouse = Engine.Input.Mouse;

namespace Engine.Component {
	public class ThirdPersonCameraComponent : CameraComponent {
		private readonly Vector3d offset;
		private double zoom;
		private Vector2d rotation;

		public ThirdPersonCameraComponent(Vector3d offset, GameObject gameObject) : base(gameObject) {
			this.offset = offset;
			zoom = 1.0;
			rotation = Vector2d.Zero;
		}

		public override void Update() {
			base.Update();

			if (Mouse.Down(MouseButton.Right)) {
				var factor = Time.DeltaTimeUpdate * 0.3;

				rotation.X += -Mouse.CursorDelta.X * factor;

				const double maxRotation = Math.PI / 2 - 0.2;
				const double minRotation = -maxRotation;
				var deltaY = -Mouse.CursorDelta.Y * factor;
				rotation.Y = Math1D.Clamp(rotation.Y + deltaY, minRotation, maxRotation);
			}
			else {
				rotation = Vector2d.Lerp(rotation, Vector2d.Zero, Time.DeltaTimeUpdate * 10);
			}

			zoom = Math1D.Clamp(zoom + Mouse.ScrollDelta / 10, 0.5, 2);

			var rot = Quaterniond.FromEulerAngles(rotation.Y, rotation.X, 0);

			var eyePosition = (GameObject.TransformComponent.Orientation * rot).Rotate(offset * zoom) +
			                  GameObject.TransformComponent.Position;

			SetLookAt(eyePosition, GameObject.TransformComponent.Position,
				GameObject.TransformComponent.Orientation.Rotate(Vector3d.UnitY));
		}
	}
}