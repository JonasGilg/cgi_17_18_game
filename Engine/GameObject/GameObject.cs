using Engine.Component;
using OpenTK.Input;

namespace Engine {
	public class GameObject {
		public TransformComponent TransformComponent;

		public GameObject() {
			TransformComponent = TransformComponent.Identity();
		}
		
		public virtual void Update(double deltaTime, KeyboardDevice input) {
			TransformComponent.Update(deltaTime, input);
		}
	}
}