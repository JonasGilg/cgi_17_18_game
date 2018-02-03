using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Engine.Render;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
	public static class TrackFactory {
		private const string LEVEL_1_FILE_PATH = "data/objects/track_files/track2/track_waypoints.obj";

		private const int CHECKPOINT_FREQUENCY = 16;
		private const double DIAMETER = 50.0;


		public static RaceTrack GenerateRaceTrack() {
			var result = new RaceTrack();
			var wps = CreateTrackPoints(LEVEL_1_FILE_PATH);
			var richtungsV = wps[1] - wps[0];
			result.StartPoint = richtungsV * -1;
			result.StartOrientation = Quaterniond.FromAxisAngle(richtungsV, 0);

			return result;
		}


		private static List<Vector3d> CreateTrackPoints(string filePath) {
			var wayPoints = LoadWayPoints(filePath);
			var level1SpecialCheckpoints = new List<int> {1, 3, 5, 7, 8, 9, 14, 17, 19, 20, 23, 24, 25, 26, 27};
			var checkpointNumber = 0;
			GoalRing lastCheckPoint = null;

			for (var i = 0; i < wayPoints.Count; i++) {
				if (i % CHECKPOINT_FREQUENCY == 0) {
					if (level1SpecialCheckpoints.Contains(checkpointNumber)) {
						CreateRingCheckpoint(wayPoints[i], wayPoints[i - 1], wayPoints[i + 1]);
					}
					else {
						lastCheckPoint = CreateCheckpoint(wayPoints[i]);
					}

					checkpointNumber++;
				}
				else {
					switch (checkpointNumber) {
						case var n when n > 20:
							PointRingFactory.GenerateSingle(wayPoints[i], PointType.GOLD, DIAMETER);
							break;
						case var n when n > 10:
							PointRingFactory.GenerateSingle(wayPoints[i], PointType.SILVER, DIAMETER);
							break;
						default:
							PointRingFactory.GenerateSingle(wayPoints[i], PointType.COPPER, DIAMETER);
							break;
					}
				}
			}

			if (lastCheckPoint != null) lastCheckPoint.TransformComponent.Scale *= 4;

			return wayPoints;
		}

		private static PointRing CreateGoldRing(Vector3d pos) => PointRingFactory.GenerateSingle(pos, PointType.GOLD, DIAMETER);

		private static GoalRing CreateCheckpoint(Vector3d pos) => GoalRingFactory.GenerateSingle(pos, DIAMETER * 3);

		private static void CreateRingCheckpoint(Vector3d pos, Vector3d prev, Vector3d next) {
			var a = next - pos;
			var b = prev - pos;
			var aXb = Vector3d.Cross(a, b);
			var w = a + b;

			var rotAxle = Vector3d.Cross(aXb, w);

			aXb.Normalize();

			GoalRingFactory.GenerateGoalRingWithAsteroidRing(pos, DIAMETER * 3, rotAxle, aXb);
		}


		private static List<Vector3d> LoadWayPoints(string filePath, float scale = 5000f) {
			var input = File.ReadLines(filePath);
			var v = new List<Vector3d>();

			foreach (var line in input) {
				var parts = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length > 0) {
					switch (parts[0]) {
						case "v":
							var vec3 = new Vector3(float.Parse(parts[1], CultureInfo.InvariantCulture) * scale,
								float.Parse(parts[2], CultureInfo.InvariantCulture) * scale,
								float.Parse(parts[3], CultureInfo.InvariantCulture) * scale);


							v.Add(vec3.ToDouble());
							break;
					}
				}
			}

			return v;
		}
	}

	public struct RaceTrack {
		public Vector3d StartPoint;
		public Quaterniond StartOrientation;
	}
}