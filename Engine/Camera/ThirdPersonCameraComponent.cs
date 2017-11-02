using Engine.Util;
using OpenTK;
using OpenTK.Input;

namespace Engine {
	public class ThirdPersonCameraComponent : CameraComponent {
		public Vector3 Offset;
		
		public ThirdPersonCameraComponent(Vector3 offset, GameObject gameObject) : base(gameObject) {
			Offset = offset;
		}
		
		public override void Update(double deltaTime, KeyboardDevice input) {
			base.Update(deltaTime, input);
			var eyePosition = Offset;
			Math3D.Rotate(ref eyePosition, GameObject.TransformComponent.Orientation);

			eyePosition += GameObject.TransformComponent.Position;

			SetLookAt(eyePosition, GameObject.TransformComponent.Position, GameObject.TransformComponent.Orientation * Vector3.UnitY);
		}
	}
}