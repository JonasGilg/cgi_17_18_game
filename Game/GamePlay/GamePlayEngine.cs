using System;
using System.Collections.Generic;
using System.Media;
using Engine;
using Engine.GUI;
using Game.GameObjects;
using Game.Utils;
using OpenTK;

namespace Game.GamePlay {
	public static class GamePlayEngine {
		public static readonly HudTextElement HUD_LEVEL_INDICATOR_TEXT = HUD.CreateHudTextElement("", new Vector2(0.7f, 0.9f));
		public static readonly HudTextElement HUD_CHECKPOINT_TEXT_ELEMENT = HUD.CreateHudTextElement("", new Vector2(-0.075f, 0.9f));

		public static HudTextElement GameOverTextElement = HUD.CreateHudTextElement("GAME OVER", new Vector2(-0.65f, 0.2f), 4f);
		public static HudTextElement GameWonTextElement = HUD.CreateHudTextElement("YOU WIN", new Vector2(-0.45f, 0.2f), 4f);
		
		public static FinishMarker CurrentFinishMarker;

		static GamePlayEngine() => HUD.AddHudTextElement(HUD_CHECKPOINT_TEXT_ELEMENT);

		public static readonly SpaceShip PLAYER_SPACESHIP = new SpaceShip {
			TransformComponent = {
				Scale = new Vector3d(1f),
				Position = Vector3d.Zero,
				Orientation = Quaterniond.Identity
			}
		};

		public static void ResetSpaceShip(Vector3d startPosition, Quaterniond startOrientation) {
			PLAYER_SPACESHIP.TransformComponent.Position = startPosition;
			PLAYER_SPACESHIP.TransformComponent.Orientation = startOrientation;
			PLAYER_SPACESHIP.moveComponent.LinearVelocity = Vector3d.Zero;
			PLAYER_SPACESHIP.moveComponent.AngularVelocity = Vector3d.Zero;
		}

		public static int CurrentLevelIndex = -1;

		public static Action[] Levels = {
			/*
			LevelGenerator.GenerateLevel1,
			LevelGenerator.GenerateLevel2,
			LevelGenerator.GenerateLevel3,
			LevelGenerator.GenerateLevel4,
			LevelGenerator.GenerateLevel5,
			LevelGenerator.GenerateLevel6
			*/
		};

		private static int maxCheckpoints;

		public static void LoadLevel(int index) {
			World.ClearWorld();
			GOAL_RING_LIST.Clear();
			CurrentLevelIndex = index;
			HUD_LEVEL_INDICATOR_TEXT.Text = $"LEVEL: {CurrentLevelIndex}";
			LevelGenerator.startLevel(index);
			maxCheckpoints = GOAL_RING_LIST.Count;
			HUD_CHECKPOINT_TEXT_ELEMENT.Text = $"{0}/{maxCheckpoints}";

			HUD.AddHudObjectMarker(GOAL_RING_LIST.Peek().objectMarker);
		}

		public static void LoadNextLevel() => LoadLevel(CurrentLevelIndex + 1);
		public static void RestartLevel() => LoadLevel(CurrentLevelIndex);

		public static void GameOver() {
			PLAYER_SPACESHIP.Destroy();
			HUD.AddHudTextElement(GameOverTextElement);
		}

		public static void GameWon() {
			Statistics.Stop();
			HUD.AddHudTextElement(GameWonTextElement);
			Statistics.ScoreTextElement.Position = new Vector2(-0.45f,-0.2f);
			Statistics.TimeSpentTextElement.Position = new Vector2(-0.45f,-0.3f);
		}

		public static void RemoveObjectFromWorld(GameObject gameObject) {
			if (gameObject.Equals(PLAYER_SPACESHIP)) {
				GameOver();
			}
			else {
				gameObject.Destroy();
			}
		}

		private static readonly Queue<GoalRing> GOAL_RING_LIST = new Queue<GoalRing>();

		public static void RegisterGoalRing(GoalRing ring) => GOAL_RING_LIST.Enqueue(ring);

		public static void CheckPointPassed(GoalRing chunk) {
			if (GOAL_RING_LIST.Peek().Equals(chunk)) {
				SystemSounds.Exclamation.Play();
				GOAL_RING_LIST.Dequeue();
				HUD_CHECKPOINT_TEXT_ELEMENT.Text = $"{maxCheckpoints - GOAL_RING_LIST.Count}/{maxCheckpoints}";
				HUD.RemoveHudObjectMarker(chunk.objectMarker.ID);
				chunk.Destroy();
				if (GOAL_RING_LIST.Count == 0) {
					GameWon();
				}
				else {
					HUD.AddHudObjectMarker(GOAL_RING_LIST.Peek().objectMarker);
				}
			}
		}

		private static void ShowFinishMarker() => CurrentFinishMarker.Instantiate();
	}
}