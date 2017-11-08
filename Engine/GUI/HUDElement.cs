using OpenTK;

namespace Engine.GUI {
	public class HUDElement {
		public readonly int ID;

		public string Text;
		public Vector2 Position;
		public float Scale;

		internal HUDElement(int id, string text, Vector2 position, float scale = 1) {
			ID = id;
			Text = text;
			Position = position;
			Scale = scale;
		}
	}
}