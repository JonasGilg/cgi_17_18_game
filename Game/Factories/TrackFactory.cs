using System;
using System.Collections.Generic;
using Engine.Render;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {

	

	
	public struct RaceTrack {
		public List<Item> TrackList;
	}
	
	public static class TrackFactory {
		private const double DIAMETER = 50.0;
		private static List<Item> TRACK_LIST;
	
		public static List<Item> generateTrackList(double length) {
			TRACK_LIST = new List<Item>();
			
			var startpoint = new Vector3d(200.0,0,0);
			TRACK_LIST.AddRange(createHelix(startpoint, Vector3d.UnitX, 1000, 1000, 20, 20,true));
			

			return TRACK_LIST;
		}

		public static List<Item> createHelix(Vector3d startPosition,Vector3d direction, double maxRadius, double distanceStep, int steps, int density = 20, bool clockwise = true) {
			/////TODO ////
			var angleStep = MathHelper.TwoPi / density;

			

			var stepFraction = distanceStep / density;
			///////////////////////////////
			var resList = new List<Item>();
			var currentPosition = startPosition;
			addCheckpoint(currentPosition);

			//var rotationMatrix = Quaterniond.FromAxisAngle(direction, angleStep);
			//build the helix
			//from radius=0 -> maxRadius in 1 turn
			var radiusGrowthStep = maxRadius / density;
			float currentAngle = 0;
			double currentRadius = radiusGrowthStep;
			while(currentAngle<MathHelper.TwoPi){
				
				currentPosition += new Vector3d(
					currentRadius*Math.Cos(currentAngle),
					currentRadius*Math.Sin(currentAngle),
					MathHelper.TwoPi*stepFraction
					);
				
				
				createAddGoldRing(currentPosition);
				currentAngle += angleStep;
				currentRadius += radiusGrowthStep;
			}
			
			currentAngle = 0;
			currentRadius = maxRadius;
			for (int i = 1; i < steps-1; i++) {
				while (currentAngle < MathHelper.TwoPi) {
					currentPosition += new Vector3d(
						currentRadius * Math.Cos(currentAngle),
						currentRadius * Math.Sin(currentAngle),
						MathHelper.TwoPi * stepFraction
					);


					createAddGoldRing(currentPosition);
					currentAngle += angleStep;
					
				}

				currentAngle = 0;
				
			}

			currentAngle = 0;
			currentRadius = maxRadius;
			while(currentAngle<MathHelper.TwoPi){
				
				currentPosition += new Vector3d(
					currentRadius*Math.Cos(currentAngle),
					currentRadius*Math.Sin(currentAngle),
					MathHelper.TwoPi*stepFraction
				);
				
				
				createAddGoldRing(currentPosition);
				currentAngle += angleStep;
				currentRadius -= radiusGrowthStep;
			}
				
				
			
			


			addCheckpoint(currentPosition+s);
			return resList;
		}

		private static void createAddGoldRing(Vector3d pos) {
			TRACK_LIST.Add(createGoldRing(pos));
		}

		private static void addCheckpoint(Vector3d pos) {
			TRACK_LIST.Add(createCheckpoint(pos));
		}

		private static PointRing createGoldRing(Vector3d pos) {
			return PointRingFactory.GenerateSingle(pos, PointType.Gold, DIAMETER);
		}

		private static GoalRing createCheckpoint(Vector3d pos) {
			return GoalRingFactory.GenerateSingle(pos, DIAMETER);
		}
		
		
		
	}
}