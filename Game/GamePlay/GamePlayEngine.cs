using System;
using Engine;
using Engine.GUI;
using Game.GameObjects;
using Game.Utils;
using OpenTK;

namespace Game.GamePlay {
	public static class GamePlayEngine {

		public static SpaceShip spaceship = new SpaceShip {
			TransformComponent = {
				Scale = new Vector3d(0.02f),
				Position = Vector3d.Zero,
				Orientation = Quaterniond.Identity
			}
		};
		
		public static int CurrentLevelIndex = -1;
		public static Action[] LEVELS = {
			LevelGenerator.GenerateLevel1b,
			LevelGenerator.GenerateLevel2,
			LevelGenerator.GenerateLevel3,
			LevelGenerator.GenerateLevel4,
			LevelGenerator.GenerateLevel5,
			LevelGenerator.GenerateLevel6
		};

		public static void LoadLevel(int index) {
			if (index == CurrentLevelIndex || index >= LEVELS.Length) return;
            
			World.ClearWorld();
			CurrentLevelIndex = index;
			LEVELS[index]();
		}

		public static void LoadNextLevel() {
			LoadLevel(CurrentLevelIndex+1);
		}
		public static void GameOver() {
			GameObject.Destroy(spaceship);
			HUD.AddHudTextElement(HUD.CreateHudTextElement("GAME OVER", new Vector2(-0.5f, 0.5f), 2000f));
		}
	}
}