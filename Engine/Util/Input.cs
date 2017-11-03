using OpenTK.Input;

namespace Engine.Input {
	public static class Keyboard {
		private static KeyboardState _previousState;
		private static KeyboardState _currentState;

		public static void Update(KeyboardState newState) {
			_previousState = _currentState;
			_currentState = newState;
		}

		public static bool Pressed(Key key) {
			return !_previousState[key] && _currentState[key];
		}

		public static bool Down(Key key) {
			return _currentState[key];
		}

		public static bool Released(Key key) {
			return _previousState[key] && !_currentState[key];
		}
	}

	public static class Mouse {
		
	}

	public static class Controller {
		
	}
}