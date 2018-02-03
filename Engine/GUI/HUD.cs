using System.Collections.Generic;
using Engine.Render;
using Engine.Render.Billboard;
using OpenTK;

namespace Engine.GUI {
	public static class HUD {
		private static readonly Dictionary<int, HudTextElement> HUD_TEXT_ELEMENTS = new Dictionary<int, HudTextElement>();
		private static readonly Dictionary<int, HudObjectMarker> HUD_OBJECT_MARKER_ELEMENTS = new Dictionary<int, HudObjectMarker>();
		private static int elementCounter;
		private static readonly TimingStats RENDER_STATS = new TimingStats("HUD");

		static HUD() => TimingRegistry.AddRenderTiming(RENDER_STATS);

		public static HudObjectMarker CreateHudObjectMarker(GameObject gameObject) {
			var marker = new HudObjectMarker(elementCounter++, gameObject);
			return marker;
		}

		public static void AddHudObjectMarker(HudObjectMarker objectMarker) 
			=> HUD_OBJECT_MARKER_ELEMENTS.Add(objectMarker.ID, objectMarker);

		public static void RemoveHudObjectMarker(int id) => HUD_OBJECT_MARKER_ELEMENTS.Remove(id);

		private static void DrawObjectMarker() {
			foreach (var objectMarker in HUD_OBJECT_MARKER_ELEMENTS) {
				HudObjectMarkerRenderer.Draw(objectMarker.Value);
			}
		}

		public static HudTextElement CreateHudTextElement(string text = "", Vector2 position = default(Vector2), TextAnchor anchor = TextAnchor.TOP_LEFT, float scale = 1, bool enabled = true)
			=> new HudTextElement(elementCounter++, text, position, anchor, scale, enabled);

		public static void AddHudTextElement(HudTextElement textElement) => HUD_TEXT_ELEMENTS.Add(textElement.ID, textElement);

		public static void RemoveHudTextElement(int id) => HUD_TEXT_ELEMENTS.Remove(id);

		private static void DrawTextElements() {
			foreach (var textElement in HUD_TEXT_ELEMENTS) {
				if (textElement.Value.Enabled) {
					TextRenderer2D.DrawString(textElement.Value.Text, textElement.Value.Position, textElement.Value.Anchor, textElement.Value.Scale);
				}
			}
		}

		public static void Draw() {
			RENDER_STATS.Start();
			DrawObjectMarker();
			DrawTextElements();
			RENDER_STATS.Stop();
		}
	}
}