﻿using Engine.Util;
using OpenTK;
using OpenTK.Input;
using Mouse = Engine.Input.Mouse;

namespace Engine {
	public class ThirdPersonCameraComponent : CameraComponent {
		public Vector3 Offset;
		private readonly Vector3 _originalOffset;
		
		public ThirdPersonCameraComponent(Vector3 offset, GameObject gameObject) : base(gameObject) {
			Offset = offset;
			_originalOffset = offset;
		}
		
		public override void Update(double deltaTime) {
			base.Update(deltaTime);

			if (Mouse.Down(MouseButton.Right)) {
				Math3D.Rotate(ref Offset,
					Quaternion.FromAxisAngle(Vector3.UnitY, (float) (-Mouse.CursorDelta.X * deltaTime * 0.1)));
			}
			else {
				Offset = Vector3.Lerp(Offset, _originalOffset, (float) deltaTime * 5);
			}
			
			var eyePosition = Offset;
			Math3D.Rotate(ref eyePosition, GameObject.TransformComponent.Orientation);

			eyePosition += GameObject.TransformComponent.Position;

			SetLookAt(eyePosition, GameObject.TransformComponent.Position, GameObject.TransformComponent.Orientation * Vector3.UnitY);
		}
	}
}