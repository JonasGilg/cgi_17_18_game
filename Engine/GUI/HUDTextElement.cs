using OpenTK;

namespace Engine.GUI {
	public enum TextAnchor {
		TOP_LEFT,
		CENTER
	}
	
	public class HudTextElement {
		public readonly int ID;
		public bool Enabled;

		public string Text;
		public Vector2 Position;
		public readonly float Scale;
		public readonly TextAnchor Anchor;

		internal HudTextElement(int id, string text, Vector2 position, TextAnchor anchor = TextAnchor.TOP_LEFT, float scale = 1, bool enabled = true) {
			ID = id;
			Text = text;
			Position = position;
			Scale = scale;
			Anchor = anchor;
			Enabled = enabled;
		}
	}
}