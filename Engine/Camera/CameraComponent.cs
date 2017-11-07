using OpenTK;
using Engine.Component;

namespace Engine{
	public class CameraComponent : Component.Component {
		
		public CameraComponent(GameObject gameObject) : base(gameObject) { }

		public Vector3d Position { get; private set; }
		public Matrix4d LookAtMatrix { get; private set; }
		
		public void SetLookAt(Vector3d eye, Vector3d target, Vector3d up) {
			LookAtMatrix = Matrix4d.LookAt(eye, target, up);
			Position = eye;
		}

		public override void Update() {
			
		}
	}
}