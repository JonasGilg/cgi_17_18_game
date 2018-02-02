using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Engine.Render;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
	public static class TrackFactory {
		
		private static readonly string LEVEL_1_FILE_PATH = "data/objects/track_files/track2/track_waypoints.obj";
		
		private static int NUMBER_OF_ELEMENTS=0;
		
		private static List<Vector3d> WAYPOINTS;
		private static int CHECKPOINT_FREQUENCY = 16;
		private const double DIAMETER = 50.0;
		
		
		
		public static RaceTrack GenerateRaceTrack(int number) {
			var result = new RaceTrack();
			var waypoints = createTrackPoints(LEVEL_1_FILE_PATH);
			var richtungsV = waypoints[1] - waypoints[0];
			result.startPoint = richtungsV * -1;
			result.startOrientation = Quaterniond.FromAxisAngle(richtungsV,0);
			
			

			return result;
		}
		
		
		
		public static List<Vector3d> createTrackPoints(string file_path) {
			var wayPoints = loadWayPoints(file_path);
			List<int> LEVEL_1_SPECIAL_CHECKPOINTS = new List<int> {1,3, 5, 7, 8,9,14, 17, 19, 20, 23,24,25,26,27,29};
			int checkpointNumber = 0;
			WAYPOINTS = wayPoints;

			for (int i = 0; i < wayPoints.Count; i++) {
				if (i % CHECKPOINT_FREQUENCY == 0) {
					
					
					if (LEVEL_1_SPECIAL_CHECKPOINTS.Contains(checkpointNumber)) {
						//special checkpoint now

						//createCheckpoint(wayPoints[i]);
						//TODO SUPER LAGGY!!!
						createRingCheckpoint(wayPoints[i],wayPoints[i-1],wayPoints[i+1]);
						
					}
					else {
						createCheckpoint(wayPoints[i]);
					}

					checkpointNumber++;

				}
				else {
					createGoldRing(wayPoints[i]);
				}
				
				
			}
				
			

			return wayPoints;
		}



		private static PointRing createGoldRing(Vector3d pos) {
			return PointRingFactory.GenerateSingle(pos, PointType.Gold, DIAMETER);
		}

		private static GoalRing createCheckpoint(Vector3d pos) {
			return GoalRingFactory.GenerateSingle(pos, DIAMETER*3);
		}

		private static void createRingCheckpoint(Vector3d pos, Vector3d prev, Vector3d next) {

			var a = next - pos;
			var b = prev - pos;
			var aXb = Vector3d.Cross(a, b);
			var w = a + b;

			var rotAxle = Vector3d.Cross(aXb, w);

			aXb.NormalizeFast();
			
			
			
			




			GoalRingFactory.GenerateGoalRingWithAsteroidRing(pos, DIAMETER * 3, rotAxle,aXb);
		}

	
		private static List<Vector3d> loadWayPoints(string filePath, float scale = 5000f) {
			var input = File.ReadLines(filePath);
			var v = new List<Vector3d>();
			
			foreach (var line in input) {
				var parts = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length > 0) {
					Vector3 vec3;
					switch (parts[0]) {
						case "v":
							vec3 = new Vector3(float.Parse(parts[1], CultureInfo.InvariantCulture) * scale,
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
		public List<Vector3d> WAYPOINTS;
		public Vector3d startPoint;
		public Quaterniond startOrientation;
	}
}