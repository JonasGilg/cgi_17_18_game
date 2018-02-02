using System.Collections.Generic;
using Game.GameObjects;
using Game.GamePlay;
using OpenTK;

namespace Game.Utils {
	public static class LevelGenerator {
		private static Vector3d startingPoint = new Vector3d(0.0, 0.0, 0.0);
		private static Quaterniond startOrientation = Quaterniond.FromAxisAngle(Vector3d.UnitY, 0);

		public static void StartLevel() {
			var raceTrack = TrackFactory.GenerateRaceTrack();

			ObstacleCreation();

			GamePlayEngine.ResetSpaceShip(raceTrack.StartPoint, raceTrack.StartOrientation);

			GamePlayEngine.PLAYER_SPACESHIP.Instantiate();
			GamePlayEngine.NAVIGATION_ARROW.Instantiate();

		}

        private static void ObstacleCreation() {
            
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
	       
            var planet5 = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.EARTHLIKE, positions[9], new Vector3d(5000), new Vector3d(0.3));
            
            //after helix
            var planet6 = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.NEPTUN, positions[10], new Vector3d(5000), new Vector3d(0.3));
        }
	}
}