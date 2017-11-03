﻿namespace Engine {
	public abstract class Component {
		protected GameObject GameObject;

		protected Component(GameObject gameObject) {
			GameObject = gameObject;
		}

		public abstract void Update();
	}
}