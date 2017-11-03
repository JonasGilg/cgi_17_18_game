using OpenTK;
using OpenTK.Input;

namespace Engine.Input {
	public static class Keyboard {
		private static KeyboardState _previousState;
		private static KeyboardState _currentState;

		public static void Update(KeyboardState newState) {
			_previousState = _currentState;
			_currentState = newState;
		}

		public static bool Down(Key key) => _currentState[key];
		public static bool Pressed(Key key) => !_previousState[key] && _currentState[key];
		public static bool Released(Key key) => _previousState[key] && !_currentState[key];
	}

	public static class Mouse {
		private static MouseState _previousState;
		private static MouseState _currentState;

		public static void Update(MouseState newState) {
			_previousState = _currentState;
			_currentState = newState;
			
			CursorDelta = new Vector2(_currentState.X - _previousState.X, _currentState.Y - _previousState.Y);
		}

		public static bool Down(MouseButton button) => _currentState[button];
		public static bool Pressed(MouseButton button) => !_previousState[button] && _currentState[button];
		public static bool Released(MouseButton button) => _previousState[button] && !_currentState[button];

		public static Vector2 CursorDelta { get; private set; }
	}

	public static class Controller {
		
	}
}