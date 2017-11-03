namespace Engine {
	public class GameObject {
		public readonly TransformComponent TransformComponent;

		public GameObject() {
			TransformComponent = TransformComponent.Identity();
		}

		public virtual void EarlyUpdate() { }

		public virtual void Update() {
			TransformComponent.Update();
		}

		public virtual void LateUpdate() { }

		public virtual void Draw() { }
	}
}