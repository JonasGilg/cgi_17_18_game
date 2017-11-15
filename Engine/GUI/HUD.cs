using System.Collections.Generic;
using Engine.Util;
using OpenTK;

namespace Engine.GUI {
	public static class HUD {
		private static readonly Dictionary<int, HUDElement> HUD_ELEMENTS = new Dictionary<int, HUDElement>();
		private static int elementCounter;
		private static readonly TimingStats RENDER_STATS = new TimingStats("HUD");

		static HUD() {
			TimingRegistry.AddRenderTiming(RENDER_STATS);
		}

		public static HUDElement CreateHUDElement(string text = "", Vector2 position = default(Vector2), float scale = 1)
			=> new HUDElement(elementCounter++, text, position, scale);

		public static void AddHUDElement(HUDElement element) => HUD_ELEMENTS.Add(element.ID, element);
		public static void RemoveHUDElement(int id) => HUD_ELEMENTS.Remove(id);

		public static void Draw() {
			RENDER_STATS.Start();

			foreach (var element in HUD_ELEMENTS) {
				TextRenderer2D.DrawString(element.Value.Text, element.Value.Position, element.Value.Scale);
			}

			RENDER_STATS.Stop();
		}
	}
}