using OpenTK;

namespace Engine.Component {
	public class TransformComponent {
		protected Vector3 Position;
		protected Quaternion Orientation;
		protected Vector3 Scale;

		public static TransformComponent Identity() => new TransformComponent();

		#region Constructors

		public TransformComponent() {
			Position = Vector3.Zero;
			Orientation = Quaternion.Identity;
			Scale = Vector3.One;
		}

		public TransformComponent(Vector3 position) {
			Position = position;
			Orientation = Quaternion.Identity;
			Scale = Vector3.One;
		}

		public TransformComponent(Quaternion orientation) {
			Position = Vector3.Zero;
			Orientation = orientation;
			Scale = Vector3.One;
		}

		public TransformComponent(Vector3 position, Quaternion orientation, Vector3 scale) {
			Position = position;
			Orientation = orientation;
			Scale = scale;
		}

		public TransformComponent(TransformComponent other) {
			this.Position = other.Position;
			this.Orientation = other.Orientation;
			this.Scale = other.Scale;
		}

		public TransformComponent(Matrix4 transformMatrix) {
			Position = transformMatrix.ExtractTranslation();
			Orientation = transformMatrix.ExtractRotation();
			Scale = transformMatrix.ExtractScale();
		}

		#endregion Constructors

		
	}
}