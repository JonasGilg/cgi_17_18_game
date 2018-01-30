using System;
using System.Collections.Generic;
using Engine;
using Engine.GUI;
using Game.GameObjects;
using Game.Utils;
using OpenTK;

namespace Game.GamePlay {
	public static class GamePlayEngine {

		public static HudTextElement hudLevelIndicatorText = HUD.CreateHudTextElement("",new Vector2(0.7f,0.9f));
		public static List<Ring>SUPPLY_RING_LIST = new List<Ring>();

		public static FinishMarker currentFinishMarker;

		public static void registerSupplyRing(Ring ring) {
			SUPPLY_RING_LIST.Add(ring);
			HUD.AddHudObjectMarker(ring.objectMarker);
		}

		public static SpaceShip playerSpaceship = new SpaceShip {
			TransformComponent = {
				Scale = new Vector3d(0.02f),
				Position = Vector3d.Zero,
				Orientation = Quaterniond.Identity
			}
		};

		public static void ResetSpaceShip() {
			playerSpaceship.TransformComponent.Position = Vector3d.Zero;
			playerSpaceship.TransformComponent.Orientation = Quaterniond.Identity;
			playerSpaceship.moveComponent.LinearVelocity = Vector3d.Zero;
			playerSpaceship.moveComponent.AngularVelocity = Vector3d.Zero;
		}
		
		public static int CurrentLevelIndex = -1;
		public static Action[] LEVELS = {
			LevelGenerator.GenerateLevel0,
			LevelGenerator.GenerateLevel1,
			LevelGenerator.GenerateLevel2,
			LevelGenerator.GenerateLevel3,
			LevelGenerator.GenerateLevel4,
			LevelGenerator.GenerateLevel5,
			LevelGenerator.GenerateLevel6
		};

		public static void LoadLevel(int index) {
			if (index == CurrentLevelIndex || index >= LEVELS.Length) return;
            
			World.ClearWorld();
			SUPPLY_RING_LIST.Clear();
			CurrentLevelIndex = index;
			hudLevelIndicatorText.Text = $"LEVEL: {CurrentLevelIndex}";
			LEVELS[index]();
		}

		public static void LoadNextLevel() {
			LoadLevel(CurrentLevelIndex+1);
		}
		public static void GameOver() {
			GameObject.Destroy(playerSpaceship);
			HUD.AddHudTextElement(HUD.CreateHudTextElement("GAME OVER", new Vector2(-0.5f, 0.5f), 2000f));
		}

		public static void RemoveObjectFromWorld(GameObject gameObject) {
			if (gameObject.Equals(playerSpaceship)) {
				GameOver();
			}
			else {
				GameObject.Destroy(gameObject);
			}
		}

		public static void supplyRingCollected(GoalRing chunk) {
			Statistics.IncreaseScore();
			SUPPLY_RING_LIST.Remove(chunk);
			HUD.RemoveHudObjectMarker(chunk.objectMarker.ID);
			GameObject.Destroy(chunk);
			if (SUPPLY_RING_LIST.Count == 0) showFinishMarker();
		}

		private static void showFinishMarker() {
			GameObject.Instantiate(currentFinishMarker);
		}
	}
}