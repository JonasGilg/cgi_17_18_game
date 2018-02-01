﻿using System.Collections.Generic;
using Engine.Render;
using Engine.Render.Billboard;
using Engine.Texture;
using OpenTK;

namespace Engine.GUI {
	public static class HUD {
		private static readonly Dictionary<int, HudTextElement> HUD_TEXT_ELEMENTS = new Dictionary<int, HudTextElement>();
		private static readonly Dictionary<int, HudObjectMarker> HUD_OBJECT_MARKER_ELEMENTS = new Dictionary<int, HudObjectMarker>();
		private static int elementCounter;
		private static readonly TimingStats RENDER_STATS = new TimingStats("HUD");

		//TODO this is temporarely
		
		static HUD() {
			TimingRegistry.AddRenderTiming(RENDER_STATS);
			
		}
		
		// START: OBJECT MARKER 
		public static HudObjectMarker CreateHudObjectMarker(GameObject gameObject) {
			//int OBJECT_MARKER_TEXTURE_ID = TextureManager.LoadTexture("data/textures/billboard/planet_tag.png");
			var marker = new HudObjectMarker(elementCounter++, gameObject /*, OBJECT_MARKER_TEXTURE_ID*/);
			return marker;
		}

		public static void AddHudObjectMarker(HudObjectMarker objectMarker) =>
			HUD_OBJECT_MARKER_ELEMENTS.Add(objectMarker.ID, objectMarker);

		public static void RemoveHudObjectMarker(int id) => HUD_OBJECT_MARKER_ELEMENTS.Remove(id);
		
		private static void drawObjectMarker() {
			foreach (var objectMarker in HUD_OBJECT_MARKER_ELEMENTS) {
				HudObjectMarkerRenderer.Draw(objectMarker.Value);

			}
		}
		// END: OBJECT MARKER
		//----------------------------------
		// START: TEXT ELEMENTS
		public static HudTextElement CreateHudTextElement(string text = "", Vector2 position = default(Vector2), TextAnchor anchor = TextAnchor.TOP_LEFT, float scale = 1, bool enabled = true)
			=> new HudTextElement(elementCounter++, text, position, anchor, scale, enabled);

		public static void AddHudTextElement(HudTextElement textElement) => HUD_TEXT_ELEMENTS.Add(textElement.ID, textElement);
		
		public static void RemoveHudTextElement(int id) => HUD_TEXT_ELEMENTS.Remove(id);
		
		private static void drawTextElements() {
			foreach (var textElement in HUD_TEXT_ELEMENTS) {
				if (textElement.Value.Enabled) {
					TextRenderer2D.DrawString(textElement.Value.Text, textElement.Value.Position, textElement.Value.Anchor, textElement.Value.Scale);
				}
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