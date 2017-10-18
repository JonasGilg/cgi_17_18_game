using OpenTK;

namespace Engine.Component {
	public class MoveComponent : Component {
		public Vector3 LinearVelocityWorld = Vector3.Zero;
		public Vector3 AngularVelocityWorld = Vector3.Zero;

		public MoveComponent(GameObject.GameObject gameObject) : base(gameObject) { }

		public void Update(double deltaTime) {
			ApplyLinearVelocity(deltaTime);
			ApplyAngularVelocity(deltaTime);
		}

		private void ApplyLinearVelocity(double deltaTime) {
			GameObject.Position += LinearVelocityWorld * (float) deltaTime;
		}

		private void ApplyAngularVelocity(double deltaTime) {
			var angularChange = AngularVelocityWorld * (float) deltaTime;
			var rotationAxis = angularChange == Vector3.Zero ? Vector3.One : angularChange.Normalized();
			var rotationAngle = angularChange.Length;
			GameObject.Orientation = Quaternion.FromAxisAngle(rotationAxis, rotationAngle) * GameObject.Orientation;
		}
	}
}