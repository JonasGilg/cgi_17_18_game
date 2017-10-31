using OpenTK.Input;

namespace Engine.Component {
	public abstract class Component {
		protected Engine.GameObject GameObject;

		protected Component(Engine.GameObject gameObject) {
			GameObject = gameObject;
		}

		public abstract void Update(double deltaTime, KeyboardDevice input);
	}
}