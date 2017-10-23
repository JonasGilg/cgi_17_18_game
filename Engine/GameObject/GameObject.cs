using System.Collections.Generic;
using OpenTK;

namespace Engine {
	public class GameObject {
		public Vector3 Position = Vector3.Zero;
		public Quaternion Orientation = Quaternion.Identity;
		public Vector3 Scale = Vector3.One;
		
		public Matrix4 Transform = Matrix4.Identity;

		public GameObject Parent;
		public List<GameObject> Children = new List<GameObject>();

		public void AttachChild(GameObject child) {
			child.Parent?.Children?.Remove(child);
			child.Parent = this;
			Children.Add(child);
		}
		
		public void UpdateTransform() {
			Transform =  Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(Orientation) * Matrix4.CreateTranslation(Position);
		}

		public void Update() {
			UpdateTransform();
		}
	}
}