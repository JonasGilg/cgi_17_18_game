using Engine.Util;
using OpenTK;
using OpenTK.Input;
using Mouse = Engine.Input.Mouse;

namespace Engine {
	public class ThirdPersonCameraComponent : CameraComponent {
		public Vector3 Offset;
		
		public ThirdPersonCameraComponent(Vector3 offset, GameObject gameObject) : base(gameObject) {
			Offset = offset;
		}
		
		public override void Update(double deltaTime) {
			base.Update(deltaTime);

			if (Mouse.Down(MouseButton.Right)) {
				Math3D.Rotate(ref Offset, Quaternion.FromAxisAngle(Vector3.UnitY, (float) (-Mouse.CursorDelta.X * deltaTime * 0.1)));
			}
			
			var eyePosition = Offset;
			Math3D.Rotate(ref eyePosition, GameObject.TransformComponent.Orientation);

			eyePosition += GameObject.TransformComponent.Position;

			SetLookAt(eyePosition, GameObject.TransformComponent.Position, GameObject.TransformComponent.Orientation * Vector3.UnitY);
		}
	}
}