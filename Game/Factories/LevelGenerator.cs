using System.Collections.Generic;
using Game.GameObjects;
using Game.GamePlay;
using OpenTK;

namespace Game.Utils {
	public static class LevelGenerator {
		private static Vector3d startingPoint = new Vector3d(0.0, 0.0, 0.0);
		private static Quaterniond startOrientation = Quaterniond.FromAxisAngle(Vector3d.UnitY, 0);

		public static void StartLevel(int number) {
			var raceTrack = TrackFactory.GenerateRaceTrack(number);

			ObstacleCreation(number);

			GamePlayEngine.ResetSpaceShip(raceTrack.StartPoint, raceTrack.StartOrientation);

			GamePlayEngine.PLAYER_SPACESHIP.Instantiate();
			GamePlayEngine.NAVIGATION_ARROW.Instantiate();

	
		}

        private static void ObstacleCreation(int lvlNumber) {
            
            List<Vector3d> positions = new List<Vector3d>() {
                new Vector3d(22571,983,-13391
                ),
                new Vector3d(30823,-289,8655),
                
                //blackholes [2+]
                new Vector3d(30567,-4757,16459),
                new Vector3d(24471,-2921,16051),
                new Vector3d(19277,-3100,12000),
                new Vector3d(37693,-3927,12120),
                
                //neptun after bh
                new Vector3d(1920,-3239,11316),
                
                
                new Vector3d(-9343,-1042,20263),
                
                
                //helix entry obstacle [7+]
                new Vector3d(-25632,3500,9850),
                new Vector3d(-25685,5778,-6895),
                
                //after helix
                new Vector3d(-14335,11,-22669),
                
                
                
            };

            var planet1 = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.EARTHLIKE, positions[0],
                new Vector3d(3000),  new Vector3d(0,0.3,0));
            
            var planet2 = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.REDPLANET, positions[1],
                new Vector3d(3000 /*3000*/), new Vector3d(0,0.3,0));
            //black hole field
            var black1 = PlanetFactory.GenerateBlackhole(positions[2], new Vector3d(1200));
            var black2 = PlanetFactory.GenerateBlackhole(positions[3], new Vector3d(1200));
            var black3 = PlanetFactory.GenerateBlackhole(positions[4], new Vector3d(1200));
            var black4 = PlanetFactory.GenerateBlackhole(positions[5], new Vector3d(800));
            ////////

            var planet3 = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.NEPTUN, positions[6],
                new Vector3d(4000/*4000.0*/), Vector3d.UnitY);
            
            
            //HELIX 
            var planet4 = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.REDPLANET, positions[8], new Vector3d(3000), new Vector3d(0.3));
	        var asteroidbelt = AsteroidFactory.GenerateAsteroidTorus(positions[8], Vector3d.UnitX, 20, 5000, 6000,1000,150);
            var planet5 = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.EARTHLIKE, positions[9], new Vector3d(5000), new Vector3d(0.3));
            
            //after helix
            var planet6= PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.NEPTUN, positions[10], new Vector3d(5000), new Vector3d(0.3));
        }
        
        
        /*
        public static void GenerateLevel0() {
            //sun
            var sun = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(0, 0, 0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0));
            sun.RenderComponent.Material = MaterialManager.GetMaterial(Material.SUN_LAVAFLOW);
            //HUD.AddHudObjectMarker(HUD.CreateHudObjectMarker(sun));
            GameObject.Instantiate(sun);
            //HUD.AddHudObjectMarker(HUD.CreateHudObjectMarker(sun));
            //sun.activateMarker();
            
            Light.SetSpotLight(sun.TransformComponent.Position,
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));

			//planets

			List<Planet> planets = new List<Planet>();
			for (var i = 1; i < 3; i++) {
				var planet = PlanetFactory.GeneratePlanetWithAsteroidBeld((PlanetFactory.PlanetTexture) i,
					AsteroidFactory.AsteroidType.STANDARD, 30, new Vector3d(10000.0 * i, 0, 0),
					new Vector3d(1000.0), new Vector3d(0, 0.5, 0));

				GameObject.Instantiate(planet);
			}

			


		    //the player
		    GamePlayEngine.ResetSpaceShip();
		    GamePlayEngine.playerSpaceship.TransformComponent.Position = startingPoint;
		    GamePlayEngine.playerSpaceship.TransformComponent.Orientation = startOrientation;
		    GameObject.Instantiate(GamePlayEngine.playerSpaceship);
		    
		    //finish
		    var finishMarker = new FinishMarker {
		        TransformComponent = {
		            Position = startingPoint + new Vector3d(500, -500, 500),
		            Scale = new Vector3d(200)
		        }
		    };
		    GamePlayEngine.currentFinishMarker = finishMarker;

			//black hole
			var blackHole = new BlackHole {
				TransformComponent = {
					Scale = new Vector3d(100),
					Position = startingPoint + new Vector3d(1000.0, 0.0, 0.0),
					Orientation = Quaterniond.Identity
				}
			};

			GameObject.Instantiate(blackHole);

			//metal chunks
			//MetalChunkFactory.GenerateLine(startingPoint + new Vector3d(500, 0, 1000), startingPoint + new Vector3d(500, 800, 1000), MetalType.Copper, 10,15.0);

			//MetalChunkFactory.GenerateRing(startingPoint + new Vector3d(200, 0, 0), new Vector3d(0, 0, 45), MetalType.Gold, 10, 100, 20.0);

			//MetalChunkFactory.GenerateEye(startingPoint + new Vector3d(0, 200, 0),new Vector3d(45,90,0),MetalType.Silver, 16, 100.0, MetalType.Gold);

		    //MetalChunkFactory.GenerateSingle(startingPoint + new Vector3d(0, 200, 200), PointType.Gold, 50.0);

		    var goalRing = new GoalRing {
		        TransformComponent = {
		            Position = startingPoint + new Vector3d(0, 200, 200),
		            Scale = new Vector3d(5)
		        }
		    };
		    GameObject.Instantiate(goalRing);
		    GamePlayEngine.registerGoalRing(goalRing);
		}
		*/

		/*
	   //############################################# LEVEL 1 #############################################
	   public static void GenerateLevel1() {
		 //sun
		 GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(11000,0,0), new Vector3d(2000.0),
		     new Vector3d(0,0.5,0)));
		 
		 Light.SetSpotLight(new Vector3d(-500,0,0), 
		     //           r      g      b      a
		     new Vector4(.010f, .010f, .010f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f));
		 
		 //the player
		 GamePlayEngine.ResetSpaceShip();
		 GameObject.Instantiate(GamePlayEngine.playerSpaceship);

		 PointRingFactory.GenerateOnPositions(new Dictionary<Vector3d, PointType> {
		     {new Vector3d(200,0,0),PointType.Copper},
		     {new Vector3d(350,0,50),PointType.Copper},
		     {new Vector3d(500,0,0),PointType.Copper},
		     {new Vector3d(650,0,-50),PointType.Copper}
		 });
		 MetalChunkFactory.GeneratePointRingWithAsteroidRing(new Vector3d(100, 0, 0), new Vector3d(0,90,0), PointType.Silver, 10, 75);
		 MetalChunkFactory.GenerateLine(new Vector3d(800, 0, 0), new Vector3d(1200, 0, 0), PointType.Copper, 5);
		 MetalChunkFactory.GenerateLine(new Vector3d(1400, 0, 100), new Vector3d(1800, 0, 100), PointType.Copper, 5);
		 MetalChunkFactory.GenerateLine(new Vector3d(2000, 0, -100), new Vector3d(2500, 0, -100), PointType.Copper, 5);
		 MetalChunkFactory.GenerateLine(new Vector3d(2700, 100, 0), new Vector3d(3500, 100, 0), PointType.Copper, 5);
		 MetalChunkFactory.GenerateSingle(new Vector3d(3600, 0, 0), PointType.Copper);
		 MetalChunkFactory.GenerateLine(new Vector3d(3700, -100, 0), new Vector3d(4500, -100, 0), PointType.Copper, 5);
		 MetalChunkFactory.GenerateLine(new Vector3d(4700, 0, 100), new Vector3d(5200, 0, -100), PointType.Copper, 15);
		 MetalChunkFactory.GenerateLine(new Vector3d(5400, 0, -100), new Vector3d(5900, 0, 100), PointType.Copper, 15);
		 MetalChunkFactory.GenerateLine(new Vector3d(6100, 100, 0), new Vector3d(6600, -100, 0), PointType.Copper, 15);
		 MetalChunkFactory.GenerateLine(new Vector3d(6800, -100, 0), new Vector3d(7300, 100, 0), PointType.Copper, 15);

		 //finish
		 GameObject.Instantiate(new FinishMarker {
		     TransformComponent = {
		         Position = new Vector3d(7500,0,0),
		         Scale = new Vector3d(200)
		     }
		 });
	   }
	   
	   //############################################# LEVEL 2 #############################################
	   public static void GenerateLevel2() {
		 //sun
		 GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(15000,0,0), new Vector3d(2000.0),
		     new Vector3d(0,0.5,0)));
		 
		 Light.SetSpotLight(new Vector3d(-500,0,0),
		     //           r      g      b      a
		     new Vector4(.010f, .010f, .010f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f));
		 
		 //the player
		 GamePlayEngine.ResetSpaceShip();
		 GameObject.Instantiate(GamePlayEngine.playerSpaceship);

		 PointRingFactory.GenerateSingle(new Vector3d(200,0,0),PointType.Copper );
		 PointRingFactory.GenerateSingle(new Vector3d(350,0,50),PointType.Copper );
		 PointRingFactory.GenerateSingle(new Vector3d(500,0,0),PointType.Copper );
		 PointRingFactory.GenerateSingle(new Vector3d(650,0,-50),PointType.Copper );
		 
		 PointRingFactory.GenerateLine(new Vector3d(800, 0, 0), new Vector3d(1200, 0, 0), PointType.Copper, 5);
		 
		 PointRingFactory.GenerateRing(new Vector3d(1500, -200, 0), Vector3d.Zero, PointType.Copper, 25, 200);
		 
		 PointRingFactory.GenerateLine(new Vector3d(1600, 0, 0), new Vector3d(2500, 0, 0), PointType.Copper, 25);
		 PointRingFactory.GenerateLine(new Vector3d(2700, 0, 0), new Vector3d(3500, 100, 0), PointType.Copper, 20);
		 PointRingFactory.GenerateLine(new Vector3d(3600, 100, 0), new Vector3d(4400, 0, 100), PointType.Copper, 20);
		 PointRingFactory.GenerateLine(new Vector3d(4500, 0, 100), new Vector3d(5300, -100, 0), PointType.Copper, 20);
		 PointRingFactory.GenerateLine(new Vector3d(5400, -100, 0), new Vector3d(6100, 0, -100), PointType.Copper, 20);
		 PointRingFactory.GenerateLine(new Vector3d(6200, 0, -100), new Vector3d(6600, 100, 0), PointType.Copper, 10);
		 PointRingFactory.GenerateLine(new Vector3d(6700, 100, 0), new Vector3d(7100, 100, 0), PointType.Copper, 20);
		 PointRingFactory.GenerateLine(new Vector3d(7200, 100, 0), new Vector3d(8000, 0, 0), PointType.Copper, 20);
		 PointRingFactory.GenerateLine(new Vector3d(8100, 0, 0), new Vector3d(8700, 0, 0), PointType.Copper, 20);
		 
		 PointRingFactory.GenerateRing(new Vector3d(9000, 300, 300), new Vector3d(0,0,90), PointType.Copper, 25, 300);
		 
		 PointRingFactory.GenerateLine(new Vector3d(9300, 0, 0), new Vector3d(9500, 0, 0), PointType.Copper, 4);
		 
		 PointRingFactory.GenerateRing(new Vector3d(9700, 0, 0), new Vector3d(0,90,0), PointType.Copper, 5, 18);
		 PointRingFactory.GenerateRing(new Vector3d(9750, 0, 0), new Vector3d(0,90,0), PointType.Copper, 5, 18);
		 PointRingFactory.GenerateRing(new Vector3d(9800, 0, 0), new Vector3d(0,90,0), PointType.Copper, 5, 18);
		 PointRingFactory.GenerateRing(new Vector3d(9850, 0, 0), new Vector3d(0,90,0), PointType.Copper, 5, 18);
		 PointRingFactory.GenerateRing(new Vector3d(9900, 0, 0), new Vector3d(0,90,0), PointType.Copper, 5, 18);

		 //finish
		 GameObject.Instantiate(new FinishMarker {
		     TransformComponent = {
		         Position = new Vector3d(10500,0,0),
		         Scale = new Vector3d(200)
		     }
		 });
	   }
	   
	   //############################################# LEVEL 3 #############################################
	   public static void GenerateLevel3() {
		 //sun
		 GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(11000,0,0), new Vector3d(2000.0),
		     new Vector3d(0,0.5,0)));
		 
		 Light.SetSpotLight(new Vector3d(-500,0,0),
		     //           r      g      b      a
		     new Vector4(.010f, .010f, .010f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f));
		 
		 //the player
		 GamePlayEngine.ResetSpaceShip();
		 GameObject.Instantiate(GamePlayEngine.playerSpaceship);
		 
		 
		 //finish
		 GameObject.Instantiate(new FinishMarker {
		     TransformComponent = {
		         Position = new Vector3d(7500,0,0),
		         Scale = new Vector3d(200)
		     }
		 });
	   }
	   
	   //############################################# LEVEL 4 #############################################
	   public static void GenerateLevel4() {
		 //sun
		 GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(11000,0,0), new Vector3d(2000.0),
		     new Vector3d(0,0.5,0)));
		 
		 Light.SetSpotLight(new Vector3d(-500,0,0),
		     //           r      g      b      a
		     new Vector4(.010f, .010f, .010f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f));
		 
		 //the player
		 GamePlayEngine.ResetSpaceShip();
		 GameObject.Instantiate(GamePlayEngine.playerSpaceship);
		 
		 
		 //finish
		 GameObject.Instantiate(new FinishMarker {
		     TransformComponent = {
		         Position = new Vector3d(7500,0,0),
		         Scale = new Vector3d(200)
		     }
		 });
	   }
	   
	   //############################################# LEVEL 5 #############################################
	   public static void GenerateLevel5() {
		 //sun
		 GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(11000,0,0), new Vector3d(2000.0),
		     new Vector3d(0,0.5,0)));
		 
		 Light.SetSpotLight(new Vector3d(-500,0,0),
		     //           r      g      b      a
		     new Vector4(.010f, .010f, .010f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f));
		 
		 //the player
		 GamePlayEngine.ResetSpaceShip();
		 GameObject.Instantiate(GamePlayEngine.playerSpaceship);
		 
		 
		 //finish
		 GameObject.Instantiate(new FinishMarker {
		     TransformComponent = {
		         Position = new Vector3d(7500,0,0),
		         Scale = new Vector3d(200)
		     }
		 });
	   }
	   
	   //############################################# LEVEL 6 #############################################
	   public static void GenerateLevel6() {
		 //sun
		 GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(11000,0,0), new Vector3d(2000.0),
		     new Vector3d(0,0.5,0)));
		 
		 Light.SetSpotLight(new Vector3d(-500,0,0),
		     //           r      g      b      a
		     new Vector4(.010f, .010f, .010f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f),
		     new Vector4(.950f, .950f, .950f, 0f));
		 
		 //the player
		 GamePlayEngine.ResetSpaceShip();
		 GameObject.Instantiate(GamePlayEngine.playerSpaceship);
		 
		 
		 //finish
		 GameObject.Instantiate(new FinishMarker {
		     TransformComponent = {
		         Position = new Vector3d(7500,0,0),
		         Scale = new Vector3d(200)
		     }
		 });
	   }
	   */
	}
}