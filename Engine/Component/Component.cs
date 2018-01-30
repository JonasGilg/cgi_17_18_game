namespace Engine.Component {
	public abstract class Component {
		public readonly GameObject GameObject;
		public bool Enabled;
		public readonly ComponentType Type = ComponentType.GENERAL_COMPONENT;

		protected Component(GameObject gameObject) {
			GameObject = gameObject;
		}


		public abstract void Update();
	}

	public enum ComponentType {
		GENERAL_COMPONENT,
		HEALTH_COMPONENT,
		RENDER_COMPONENT,
		MOVE_COMPONENT
	}
}