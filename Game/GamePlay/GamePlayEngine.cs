using System;
using System.Collections.Generic;
using Engine;
using Engine.GUI;
using Engine.Render;
using Engine.Sound;
using Game.GameObjects;
using Game.Utils;
using OpenTK;

namespace Game.GamePlay {
	public static class GamePlayEngine {
		private static readonly HudTextElement HUD_CHECKPOINT_TEXT_ELEMENT = HUD.CreateHudTextElement("", new Vector2(0, 0.9f), TextAnchor.CENTER, 2);

		private static readonly HudTextElement GAME_OVER_TEXT_ELEMENT = HUD.CreateHudTextElement("GAME OVER", Vector2.Zero, TextAnchor.CENTER, 4f, false);
		private static readonly HudTextElement GAME_WON_TEXT_ELEMENT = HUD.CreateHudTextElement("YOU WIN", Vector2.Zero, TextAnchor.CENTER, 4f, false);

		static GamePlayEngine() {
			HUD.AddHudTextElement(HUD_CHECKPOINT_TEXT_ELEMENT);
			HUD.AddHudTextElement(GAME_OVER_TEXT_ELEMENT);
			HUD.AddHudTextElement(GAME_WON_TEXT_ELEMENT);
		}

		public static readonly NavigationArrow NAVIGATION_ARROW = new NavigationArrow {
			TransformComponent = {
				Scale = new Vector3d(1),
				Position = Vector3d.Zero,
				Orientation = Quaterniond.Identity
			}
		};

		public static readonly SpaceShip PLAYER_SPACESHIP = new SpaceShip {
			TransformComponent = {
				Scale = new Vector3d(1),
				Position = Vector3d.Zero,
				Orientation = Quaterniond.Identity
			}
		};

		public static void ResetSpaceShip(Vector3d startPosition, Quaterniond startOrientation) {
			PLAYER_SPACESHIP.TransformComponent.Position = startPosition;
			PLAYER_SPACESHIP.TransformComponent.Orientation = startOrientation;
			PLAYER_SPACESHIP.MoveComponent.LinearVelocity = Vector3d.Zero;
			PLAYER_SPACESHIP.MoveComponent.AngularVelocity = Vector3d.Zero;
		}

		private static int maxCheckpoints;

		public static void LoadLevel() {
			Soundtrack.PlaySoundTrack("data/sound/soundtrack/soundtrack.wav");
			World.ClearWorld();
			GOAL_RING_LIST.Clear();
			LevelGenerator.StartLevel();
			maxCheckpoints = GOAL_RING_LIST.Count;
			HUD_CHECKPOINT_TEXT_ELEMENT.Text = $"{0}/{maxCheckpoints}";
			GAME_OVER_TEXT_ELEMENT.Enabled = false;
			GAME_WON_TEXT_ELEMENT.Enabled = false;
			HUD.AddHudObjectMarker(GOAL_RING_LIST.Peek().ObjectMarker);
		}

		public static void RestartLevel() {
			LoadLevel();
			Time.ResetGameTime();
		}

		private static void GameOver() {
			PLAYER_SPACESHIP.Destroy();
			GAME_OVER_TEXT_ELEMENT.Enabled = true;
		}

		private static readonly Sound VICTORY_SOUND = new Sound("data/sound/victory.wav");

		private static void GameWon() {
			Soundtrack.PlaySoundTrack("data/sound/soundtrack/victory_ambient.wav");
			VICTORY_SOUND.Play();
			Statistics.Stop();
			GAME_WON_TEXT_ELEMENT.Enabled = true;
			Statistics.SCORE_TEXT_ELEMENT.Position = new Vector2(0, -0.3f);
			Statistics.TIME_SPENT_TEXT_ELEMENT.Position = new Vector2(0, -0.4f);
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

		public static GoalRing CurrentGoal {
			get {
				try {
					return GOAL_RING_LIST.Peek();
				}
				catch (InvalidOperationException e) {
					return null;
				}
			}
		}

		public static void RegisterGoalRing(GoalRing ring) => GOAL_RING_LIST.Enqueue(ring);

		private static readonly Sound CHECKPOINT_PASSED_SOUND = new Sound("data/sound/checkpoint.wav");

		public static void CheckPointPassed(GoalRing chunk) {
			if (GOAL_RING_LIST.Peek().Equals(chunk)) {
				CHECKPOINT_PASSED_SOUND.Play();
				GOAL_RING_LIST.Dequeue();
				HUD_CHECKPOINT_TEXT_ELEMENT.Text = $"{maxCheckpoints - GOAL_RING_LIST.Count}/{maxCheckpoints}";
				HUD.RemoveHudObjectMarker(chunk.ObjectMarker.ID);
				chunk.Destroy();
				if (GOAL_RING_LIST.Count == 0) {
					GameWon();
				}
				else {
					HUD.AddHudObjectMarker(GOAL_RING_LIST.Peek().ObjectMarker);
				}
			}
		}
	}
}