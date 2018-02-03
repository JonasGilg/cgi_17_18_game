using System;
using Game.GameObjects;
using Game.GamePlay;
using OpenTK;

namespace Game.Utils {
	public static class GoalRingFactory {
		public static GoalRing GenerateSingle(Vector3d position, double scale = 5.0) {
			var chunk = new GoalRing {
				TransformComponent = {
					Scale = new Vector3d(scale),
					Position = position,
					Orientation = Quaterniond.FromAxisAngle(Vector3d.UnitX, Math.PI / 2)
				},
				MoveComponent = {AngularVelocity = new Vector3d(0, 3.0, 0)}
			};

			GamePlayEngine.RegisterGoalRing(chunk);
			chunk.Instantiate();

			return chunk;
		}

		public static void GenerateGoalRingWithAsteroidRing(Vector3d center, double pointRingScale, Vector3d rotationV, Vector3d direction) {
			var checkPoint = GenerateSingle(center, pointRingScale);
			AsteroidFactory.GenerateAsteroidRingForCheckpoint(checkPoint, rotationV, direction, pointRingScale);
		}
	}
}