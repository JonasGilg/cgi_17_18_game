using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Engine.Render;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
	public static class TrackFactory {
		private static string LEVEL1_FILE_PATH = "data/objects/track_files/track2/track_waypoints.obj";
		private static int NUMBER_OF_ELEMENTS=0;
		private static int CHECKPOINT_FREQUENCY = 16;
		private const double DIAMETER = 50.0;

		public static Vector3d createWayPoints() {
			var wayPoints = loadWayPoints(LEVEL1_FILE_PATH);
			
			NUMBER_OF_ELEMENTS=0;

			foreach (var pos in wayPoints) {
				createAddRing(pos);
			}

			return wayPoints.Peek();
		}
		
		private static void createAddRing(Vector3d pos) {
			if (NUMBER_OF_ELEMENTS % CHECKPOINT_FREQUENCY == 0) {
				createCheckpoint(pos);
			}
			else {
				//TODO other RingTypes
				createGoldRing(pos);
			}

			NUMBER_OF_ELEMENTS++;
		}

		private static void addCheckpoint(Vector3d pos) {
			NUMBER_OF_ELEMENTS++;
			createCheckpoint(pos);
		}

		private static PointRing createGoldRing(Vector3d pos) {
			return PointRingFactory.GenerateSingle(pos, PointType.Gold, DIAMETER);
		}

		private static GoalRing createCheckpoint(Vector3d pos) {
			return GoalRingFactory.GenerateSingle(pos, DIAMETER*2);
		}

	
		private static Queue<Vector3d> loadWayPoints(string filePath, float scale = 5000f) {
			var input = File.ReadLines(filePath);
			Queue<Vector3d> v = new Queue<Vector3d>();
			foreach (var line in input) {
				var parts = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length > 0) {
					Vector3 vec3;
					switch (parts[0]) {
						case "v":
							vec3 = new Vector3(float.Parse(parts[1], CultureInfo.InvariantCulture) * scale,
								float.Parse(parts[2], CultureInfo.InvariantCulture) * scale,
								float.Parse(parts[3], CultureInfo.InvariantCulture) * scale);


							v.Enqueue(vec3.ToDouble());
							break;
					}
				}
			}

			return v;

		}
	}
}