using Engine.Component;
namespace Engine {
	public class GameObject {
		public readonly TransformComponent TransformComponent;
		public float radius;

		public GameObject() {
			TransformComponent = new TransformComponent(this);
		}

		public virtual void Update() {
			TransformComponent.Update();
		}

		public virtual void Draw() { }
	}
}