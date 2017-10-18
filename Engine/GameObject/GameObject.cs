using OpenTK;

namespace Engine.GameObject {
	public class GameObject {
		public Vector3 Position = Vector3.Zero;
		public Quaternion Orientation = Quaternion.Identity;
		public Vector3 Scale = Vector3.One;

		//TODO replace with once a tick calculation!
		public Matrix4 GetTransformationMatrix() {
			return Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(Orientation) * Matrix4.CreateTranslation(Position);
		}
	}
}