using OpenTK;

namespace Engine.Component {
	public class MoveComponent : Component {
		public Vector3 LinearVelocityWorld = Vector3.Zero;
		public Vector3 AngularVelocityWorld = Vector3.Zero;

		public MoveComponent(GameObject.GameObject gameObject) : base(gameObject) { }

		public void AddLinearVelocityInObjectSpace(Vector3 velocityVector) {
			var localVector = (Matrix4.CreateTranslation(velocityVector) * GameObject.GetTransformationMatrix().ClearTranslation()).ExtractTranslation();
			LinearVelocityWorld += localVector;
		}

		public void AddLinearVelocityInWorldSpace(Vector3 velocityVector) {
			LinearVelocityWorld += velocityVector;
		}

		public void Update(double deltaTime) {
			GameObject.Position += LinearVelocityWorld * (float) deltaTime;

			var angularChange = AngularVelocityWorld * (float) deltaTime;
			var rotationAxis = angularChange == Vector3.Zero ? Vector3.One : angularChange.Normalized();
			var rotationAngle = angularChange.Length;
			GameObject.Orientation = Quaternion.FromAxisAngle(rotationAxis, rotationAngle) * GameObject.Orientation;
		}
	}
}