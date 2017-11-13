using Engine.Component;

namespace Engine {
	public abstract class GameObject {
		public readonly TransformComponent TransformComponent;
		public double Radius;

		protected GameObject() {
			TransformComponent = new TransformComponent(this);
			Radius = 0;
		}

		public virtual void Awake() { }

		public virtual void Update() {
			TransformComponent.Update();
		}

		public virtual void Draw() { }

		public override string ToString() {
			return base.ToString() + "(#" + GetHashCode() + ")";
		}
	}
}