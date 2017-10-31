using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace Engine.Component {
	public class TransformComponent : Component {
		public Matrix4 WorldMatrix = Matrix4.Identity;
		public Matrix4 LocalMatrix = Matrix4.Identity;

		public Vector3 Position;
		public Quaternion Orientation;
		public Vector3 Scale;

		public static TransformComponent Identity() => new TransformComponent();

		public TransformComponent(Vector3 position, Quaternion orientation, Vector3 scale, GameObject owner) : base(owner) {
			Position = position;
			Orientation = orientation;
			Scale = scale;

			Children = new List<TransformComponent>();
		}

		public TransformComponent(GameObject owner = null) : this(Vector3.Zero, Quaternion.Identity, Vector3.One, owner) { }

		public TransformComponent(Vector3 position, GameObject owner = null) : this(position, Quaternion.Identity,
			Vector3.One, owner) { }

		public TransformComponent(Quaternion orientation, GameObject owner = null) : this(Vector3.Zero, orientation,
			Vector3.One, owner) { }

		public TransformComponent(TransformComponent other) : this(other.Position, other.Orientation, other.Scale,
			other.GameObject) { }

		public TransformComponent(Matrix4 transformMatrix, GameObject owner = null) : this(
			transformMatrix.ExtractTranslation(),
			transformMatrix.ExtractRotation(), transformMatrix.ExtractScale(), owner) { }

		private TransformComponent _parent;
		public List<TransformComponent> Children;

		public TransformComponent Parent {
			get => _parent;
			set {
				var oldParent = Parent;
				if (oldParent == value) return;

				oldParent?.Children.Remove(this);
				value?.Children.Add(this);

				_parent = value;
			}
		}

		public void UpdateLocalMatrix() {
			LocalMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(Orientation) *
			              Matrix4.CreateTranslation(Position);
		}

		public void UpdateWorldMatrix() {
			UpdateLocalMatrix();
			if (Parent == null) {
				WorldMatrix = LocalMatrix;
			}
			else {
				Parent.UpdateWorldMatrix();
				WorldMatrix = Parent.WorldMatrix * LocalMatrix;
			}
		}

		public override void Update(double deltaTime, KeyboardDevice input) {
			UpdateWorldMatrix();
		}
	}
}