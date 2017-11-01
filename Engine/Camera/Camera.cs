using OpenTK;

namespace Engine {
	public class Camera : GameObject {

		public Camera() { }

		public Matrix4 LookAtMatrix { get; private set; }
		
		public void SetLookAt(Vector3 target, Vector3 up) {
			LookAtMatrix = Matrix4.LookAt(TransformComponent.Position, target, up);
		}
	}
}