namespace Engine.Component {
	public abstract class Component<T> where T : GameObject.GameObject {
		private T _gameObject;

		protected Component(T gameObject) {
			_gameObject = gameObject;
		}
	}
}