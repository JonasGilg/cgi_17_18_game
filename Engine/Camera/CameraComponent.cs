using OpenTK;
using OpenTK.Input;

namespace Engine {
	public class CameraComponent : Component {

		public CameraComponent(GameObject gameObject) : base(gameObject) { }

		public Vector3 Position { get; private set; }
		public Matrix4 LookAtMatrix { get; private set; }
		
		public void SetLookAt(Vector3 eye, Vector3 target, Vector3 up) {
			LookAtMatrix = Matrix4.LookAt(eye, target, up);
			Position = eye;
		}

		public override void Update() { }
	}
}