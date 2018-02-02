using System;
using System.Collections.Generic;
using System.Media;
using Engine;
using Engine.GUI;
using Engine.Util;
using Game.GameObjects;
using Game.Utils;
using OpenTK;
using SoundPlayer = Engine.Sound.SoundPlayer;

namespace Game.GamePlay {
	public static class GamePlayEngine {
		public static readonly HudTextElement HUD_CHECKPOINT_TEXT_ELEMENT = HUD.CreateHudTextElement("", new Vector2(0, 0.9f),TextAnchor.CENTER, 2);

		public static HudTextElement GameOverTextElement = HUD.CreateHudTextElement("GAME OVER", Vector2.Zero, TextAnchor.CENTER, 4f, false);
		public static HudTextElement GameWonTextElement = HUD.CreateHudTextElement("YOU WIN", Vector2.Zero, TextAnchor.CENTER, 4f, false);
		
		public static FinishMarker CurrentFinishMarker;

		static GamePlayEngine() {
			HUD.AddHudTextElement(HUD_CHECKPOINT_TEXT_ELEMENT);
			HUD.AddHudTextElement(GameOverTextElement);
			HUD.AddHudTextElement(GameWonTextElement);
		}

		public static readonly SpaceShip PLAYER_SPACESHIP = new SpaceShip {
			TransformComponent = {
				Scale = new Vector3d(1f),
				Position = Vector3d.Zero,
				Orientation = Quaterniond.Identity
			}
		};

		public static readonly NavigationArrow NAVIGATION_ARROW = new NavigationArrow {
			TransformComponent = {
				Position = PLAYER_SPACESHIP.TransformComponent.Position
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
		private static readonly int SOUNDTRACK_ID = SoundPlayer.LoadSound("data/sound/soundtrack/Steamtech-Mayhem.wav");

		public static void LoadLevel(int index) {
			SoundPlayer.PlayLooping(SOUNDTRACK_ID);
			World.ClearWorld();
			GOAL_RING_LIST.Clear();
			CurrentLevelIndex = index;
			LevelGenerator.StartLevel(index);
			maxCheckpoints = GOAL_RING_LIST.Count;
			HUD_CHECKPOINT_TEXT_ELEMENT.Text = $"{0}/{maxCheckpoints}";
			GameOverTextElement.Enabled = false;
			GameWonTextElement.Enabled = false;
			HUD.AddHudObjectMarker(GOAL_RING_LIST.Peek().objectMarker);
		}

		public static void LoadNextLevel() => LoadLevel(CurrentLevelIndex + 1);

		public static void RestartLevel() {
			IO.PrintAsync("Restarting Level");
			LoadLevel(CurrentLevelIndex);
		}

		public static void GameOver() {
			PLAYER_SPACESHIP.Destroy();
			GameOverTextElement.Enabled = true;
		}

		public static void GameWon() {
			Statistics.Stop();
			GameWonTextElement.Enabled = true;
			Statistics.ScoreTextElement.Position = new Vector2(0,-0.3f);
			Statistics.TimeSpentTextElement.Position = new Vector2(0,-0.4f);
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