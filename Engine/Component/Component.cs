namespace Engine.Component {
	public abstract class Component {
		public GameObject GameObject;
		public bool enabled;

		protected Component(GameObject gameObject) {
			GameObject = gameObject;
		}

		public abstract void Update();
	}
}