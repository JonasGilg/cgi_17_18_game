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
				_rotation.Y += -Mouse.CursorDelta.X * Time.DeltaTime * 0.1f;
				_rotation.X += -Mouse.CursorDelta.Y * Time.DeltaTime * 0.1f;
			}
			else {
				_rotation = Vector2.Lerp(_rotation, Vector2.Zero, Time.DeltaTime * 5);
			}
			
			var rotation = Quaternion.FromEulerAngles(_rotation.X, _rotation.Y, 0f);
			var eyePosition = Math3D.Rotate(Offset, rotation);
			Math3D.Rotate(ref eyePosition, GameObject.TransformComponent.Orientation);

			eyePosition += GameObject.TransformComponent.Position;

			SetLookAt(eyePosition, GameObject.TransformComponent.Position, GameObject.TransformComponent.Orientation * Vector3.UnitY);
		}
	}
}