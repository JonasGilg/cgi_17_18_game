using Engine.Util;
using OpenTK;

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
			GameObject.TransformComponent.Position += LinearVelocity * Time.DeltaTime;
		}

		private void ApplyAngularVelocity() {
			var angularChange = AngularVelocity * Time.DeltaTime;
			var rotationAxis = angularChange == Vector3.Zero ? Vector3.One : angularChange.Normalized();
			var rotationAngle = angularChange.Length;
			GameObject.TransformComponent.Orientation = Quaternion.FromAxisAngle(rotationAxis, rotationAngle) * GameObject.TransformComponent.Orientation;
		}
	}
}