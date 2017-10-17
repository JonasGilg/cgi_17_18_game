namespace Engine.Component {
	public abstract class Component {
		protected GameObject.GameObject GameObject;

		protected Component(GameObject.GameObject gameObject) {
			GameObject = gameObject;
		}
	}
}