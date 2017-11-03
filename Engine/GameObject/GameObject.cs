namespace Engine {
	public class GameObject {
		public readonly TransformComponent TransformComponent;

		public GameObject() {
			TransformComponent = TransformComponent.Identity();
		}

		public virtual void Update() {
			TransformComponent.Update();
		}

		public virtual void Draw() { }
	}
}