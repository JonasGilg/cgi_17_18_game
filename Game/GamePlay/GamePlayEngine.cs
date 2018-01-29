using Engine;
using Engine.GUI;
using Game.GameObjects;
using OpenTK;

namespace Game.GamePlay {
	public static class GamePlayEngine {
	
		
		public static void GameOver() {
			//TODO REMOVE SHIP
			HUD.AddHudTextElement(HUD.CreateHudTextElement("GAME OVER", new Vector2(-0.5f, 0.5f), 2000f));
		}
	}
}