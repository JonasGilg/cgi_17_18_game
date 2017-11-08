using System.Collections.Generic;
using Engine.Util;
using OpenTK;

namespace Engine.GUI {
	public static class HUD {
		private static readonly Dictionary<int, HUDElement> HUDElements = new Dictionary<int, HUDElement>();
		private static int _elementCounter;
		private static readonly TimingStats RenderStats = new TimingStats("HUD");
		
		static HUD() {
			TimingRegistry.AddRenderTiming(RenderStats);
		}
		
		public static HUDElement CreateHUDElement(string text = "", Vector2 position = default(Vector2), float scale = 1) 
			=> new HUDElement(_elementCounter++, text, position, scale);

		public static void AddHUDElement(HUDElement element) => HUDElements.Add(element.ID, element);
		public static void RemoveHUDElement(int id) => HUDElements.Remove(id);

		public static void Draw() {
			RenderStats.Start();
			
			foreach (var element in HUDElements) {
				TextRenderer2D.DrawString(element.Value.Text, element.Value.Position, element.Value.Scale);
			}
			
			RenderStats.Stop();
		}
	}
}