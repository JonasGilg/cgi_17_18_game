using System;
using Engine;
using Engine.Render;
using OpenTK;
using OpenTK.Input;
using Mouse = Engine.Input.Mouse;

namespace Game.Components {
	public class ThirdPersonSpringCameraComponent : CameraComponent {
		private readonly Vector3d offset;
		private double zoom;
		private Vector2d rotation;
		private Quaterniond rot;

		private readonly MoveComponent moveComponent;

		public ThirdPersonSpringCameraComponent(MoveComponent moveComponent, Vector3d offset, GameObject gameObject) : base(gameObject) {
			this.offset = offset;
			zoom = 1.0;
			rotation = Vector2d.Zero;
			this.moveComponent = moveComponent;
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
			
			rot = Quaterniond.Slerp(rot, Quaterniond.FromEulerAngles(rotation.Y, rotation.X, 0) * GameObject.TransformComponent.Orientation, Time.DeltaTimeUpdate * 10);

			var eyePosition = rot.Rotate(offset * (zoom + moveComponent.LinearVelocity.LengthFast / 2000)) + GameObject.TransformComponent.Position;
			
			SetLookAt(eyePosition, GameObject.TransformComponent.Position, GameObject.TransformComponent.Orientation.Rotate(Vector3d.UnitY));
		}
	}
}