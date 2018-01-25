using System.Collections.Generic;
using Engine.Material.Billboard;
using Engine.Render;
using OpenTK;

namespace Engine.GUI {
	public static class HUD {
		private static readonly Dictionary<int, HudTextElement> HUD_TEXT_ELEMENTS = new Dictionary<int, HudTextElement>();
		private static readonly Dictionary<int, HudObjectMarker> HUD_OBJECT_MARKER_ELEMENTS = new Dictionary<int, HudObjectMarker>();
		private static int elementCounter;
		private static readonly TimingStats RENDER_STATS = new TimingStats("HUD");

		static HUD() {
			TimingRegistry.AddRenderTiming(RENDER_STATS);
		}
		
		// START: OBJECT MARKER 
		public static HudObjectMarker CreateHudObjectMarker(GameObject gameObject, int textureId) => new HudObjectMarker(elementCounter++,gameObject, textureId);

		public static void AddHudObjectMarker(HudObjectMarker objectMarker) =>
			HUD_OBJECT_MARKER_ELEMENTS.Add(objectMarker.id, objectMarker);

		public static void RemoveHudObjectMarker(int id) => HUD_OBJECT_MARKER_ELEMENTS.Remove(id);
		
		private static void drawObjectMarker() {
			foreach (var objectMarker in HUD_OBJECT_MARKER_ELEMENTS) {
				HudObjectMarkerRenderer.Draw(objectMarker.Value);
			}
		}
		// END: OBJECT MARKER
		//----------------------------------
		// START: TEXT ELEMENTS
		public static HudTextElement CreateHudTextElement(string text = "", Vector2 position = default(Vector2), float scale = 1)
			=> new HudTextElement(elementCounter++, text, position, scale);

		public static void AddHudTextElement(HudTextElement textElement) => HUD_TEXT_ELEMENTS.Add(textElement.ID, textElement);
		
		public static void RemoveHudTextElement(int id) => HUD_TEXT_ELEMENTS.Remove(id);
		
		private static void drawTextElements() {
			foreach (var textElement in HUD_TEXT_ELEMENTS) {
				TextRenderer2D.DrawString(textElement.Value.Text, textElement.Value.Position, textElement.Value.Scale);
			}
		}
		// END: TEXT ELEMENTS
		
		
		
		
		
		public static void Draw() {
			RENDER_STATS.Start();

			
			drawObjectMarker();
			
			//text is last
			drawTextElements();

			RENDER_STATS.Stop();
		}

		

		
	}
}