using OpenTK;
using OpenTK.Input;

namespace Engine.Input {
	public static class Keyboard {
		private static KeyboardState previousState;
		private static KeyboardState currentState;

		public static void Update(KeyboardState newState) {
			previousState = currentState;
			currentState = newState;
		}

		public static bool Down(Key key) => currentState[key];
		public static bool Pressed(Key key) => !previousState[key] && currentState[key];
		public static bool Released(Key key) => previousState[key] && !currentState[key];
	}

	public static class Mouse {
		private static MouseState previousState;
		private static MouseState currentState;

		public static void Update(MouseState newState) {
			previousState = currentState;
			currentState = newState;

			CursorDelta = new Vector2(currentState.X - previousState.X, currentState.Y - previousState.Y);
			ScrollDelta = previousState.WheelPrecise - currentState.WheelPrecise;
		}

		public static bool Down(MouseButton button) => currentState[button];
		public static bool Pressed(MouseButton button) => !previousState[button] && currentState[button];
		public static bool Released(MouseButton button) => previousState[button] && !currentState[button];

		public static Vector2 CursorDelta { get; private set; }
		public static double ScrollDelta { get; private set; }
	}

	public static class Controller { }
}