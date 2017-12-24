using Engine.Component;
using Engine.Render;

namespace Engine {
	public abstract class GameObject {
		public readonly TransformComponent TransformComponent;
		public double Radius;
		public Sphere BoundingSphere() => new Sphere(TransformComponent.WorldPosition, Radius);

		protected GameObject() {
			TransformComponent = new TransformComponent(this);
			Radius = 0;
		}

		public virtual void Awake() { }

		public virtual void Update() => TransformComponent.Update();

		public override string ToString() => GetType().Name + "(#" + GetHashCode().ToString() + ")";
	}
}