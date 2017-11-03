using OpenTK;
using OpenTK.Input;

namespace Engine {
	public class MoveComponent : Component {
		public Vector3 LinearVelocity = Vector3.Zero;
		public Vector3 AngularVelocity = Vector3.Zero;

		public MoveComponent(GameObject gameObject) : base(gameObject) { }

		public override void Update() {
			ApplyLinearVelocity();
			ApplyAngularVelocity();
		}

		private void ApplyLinearVelocity() {
			GameObject.TransformComponent.Position += LinearVelocity * 0.1f;//Time.deltaTime;
		}

		private void ApplyAngularVelocity() {
			var angularChange = AngularVelocity * 0.1f;//Time.deltaTime;
			var rotationAxis = angularChange == Vector3.Zero ? Vector3.One : angularChange.Normalized();
			var rotationAngle = angularChange.Length;
			GameObject.TransformComponent.Orientation = Quaternion.FromAxisAngle(rotationAxis, rotationAngle) * GameObject.TransformComponent.Orientation;
		}
	}
}