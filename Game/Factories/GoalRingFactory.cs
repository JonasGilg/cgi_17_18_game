using System;
using System.Collections.Generic;
using Engine;
using Game.GameObjects;
using Game.GamePlay;
using OpenTK;

namespace Game.Utils {
	public class GoalRingFactory {
		
		public static GoalRing GenerateSingle(Vector3d position, double scale = 5.0) {
			var chunk = new GoalRing() {
				TransformComponent = {
					Scale = new Vector3d(scale),
					Position = position,
					Orientation = Quaterniond.FromAxisAngle(Vector3d.UnitX, Math.PI/2)
                    
				},
				moveComponent = { AngularVelocity = new Vector3d(0,4.0,0) }
                
			};
            
       		GamePlayEngine.registerGoalRing(chunk);
			GameObject.Instantiate(chunk);
			
			return chunk;
		}
		
		
		public static List<GameObject> GenerateGoalRingWithAsteroidRing(Vector3d center, Vector3d eulerAngle, int asteroidCount, double radius, double asteroidScale = 5.0, double pointRingScale = 5.0 ) {
			var objs = new List<GameObject>();
            
			objs.AddRange(AsteroidFactory.GenerateAsteroidRing(center,eulerAngle,asteroidCount,radius,asteroidScale));
			objs.Add(GenerateSingle(center,pointRingScale));
            
           

			return objs;
		}
		
		
	}
}