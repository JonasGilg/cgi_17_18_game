using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.GUI;
using Engine.Material;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
    public static class LevelGenerator {
        private static readonly Vector3d startingPoint = new Vector3d(0.0, 2000.0, -5500.0);
        private static readonly Quaterniond startOrientation = Quaterniond.FromAxisAngle(Vector3d.UnitY, -1.5);

        public static int CurrentLevelIndex = -1;
        public static Action[] LEVELS = {
            GenerateLevel1b,
            GenerateLevel2,
            GenerateLevel3,
            GenerateLevel4,
            GenerateLevel5,
            GenerateLevel6
        };

        public static void LoadLevel(int index) {
            if (index == CurrentLevelIndex || index >= LEVELS.Length) return;
            
            World.ClearWorld();
            CurrentLevelIndex = index;
            LEVELS[index]();
        }

        public static void LoadNextLevel() {
            LoadLevel(CurrentLevelIndex+1);
        }
        
        public static void GenerateLevel1() {
            //sun
            var sun = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(0, 0, 0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0));
            sun.RenderComponent.Material = MaterialManager.GetMaterial(Material.SUN_LAVAFLOW);
            //HUD.AddHudObjectMarker(HUD.CreateHudObjectMarker(sun));
            GameObject.Instatiate(sun);
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
                    new Vector3d(1000.0), new Vector3d(0, 0.5, 0), sun);
                
                GameObject.Instatiate(planet);
                planets.Add(planet);
                
            }

            planets.First().activateMarker();
            //planets.Last().activateMarker();
            

            //the player
            var ship = new SpaceShip {
                TransformComponent = {
                    Scale = new Vector3d(0.02f),
                    Position = startingPoint,
                    Orientation = startOrientation
                }
            };
            GameObject.Instatiate(ship);

            //black hole
            var blackHole = new BlackHole {
                TransformComponent = {
                    Scale = new Vector3d(100),
                    Position = startingPoint + new Vector3d(1000.0, 0.0, 0.0),
                    Orientation = Quaterniond.Identity
                }
            };
			
            GameObject.Instatiate(blackHole);
            
            //metal chunks
            MetalChunkFactory.GenerateLine(startingPoint + new Vector3d(500, 0, 1000), startingPoint + new Vector3d(500, 800, 1000), MetalType.Copper, 10);

            MetalChunkFactory.GenerateRing(startingPoint + new Vector3d(200, 0, 0), new Vector3d(0, 0, 45), MetalType.Gold, 10, 100);
			
            //MetalChunkFactory.GenerateEye(startingPoint + new Vector3d(0, 200, 0),new Vector3d(45,90,0),MetalType.Silver, 16, 100.0, MetalType.Gold);
        }
        

        private static void GenerateLevel1b() {
            //sun
            var sun = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(5000,0,0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0));
            GameObject.Instatiate(sun);
            
            Light.SetSpotLight(sun.TransformComponent.Position,
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));
            
            //the player
            GameObject.Instatiate(new SpaceShip {
                TransformComponent = {
                    Scale = new Vector3d(0.02f),
                    Position = Vector3d.Zero,
                    Orientation = Quaterniond.Identity
                }
            });
            
            GameObject.Instatiate(new FinishMarker() {
                TransformComponent = {
                    Position = new Vector3d(2000,0,0),
                    Scale = new Vector3d(200)
                }
            });
        }
        
        private static void GenerateLevel2() {
            //sun
            var sun = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(5000,0,0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0));
            GameObject.Instatiate(sun);
            
            Light.SetSpotLight(sun.TransformComponent.Position,
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));
            
            //the player
            GameObject.Instatiate(new SpaceShip {
                TransformComponent = {
                    Scale = new Vector3d(0.02f),
                    Position = new Vector3d(0,0,500),
                    Orientation = Quaterniond.Identity
                }
            });
            
            //Finish
            GameObject.Instatiate(new FinishMarker() {
                TransformComponent = {
                    Position = new Vector3d(2000,0,0),
                    Scale = new Vector3d(200)
                }
            });
        }
        
        private static void GenerateLevel3() {
            //the player
            GameObject.Instatiate(new SpaceShip {
                TransformComponent = {
                    Scale = new Vector3d(0.02f),
                    Position = new Vector3d(0,0,500),
                    Orientation = Quaterniond.Identity
                }
            });
        }
        
        private static void GenerateLevel4() {
            
        }
        
        private static void GenerateLevel5() {
            
        }
        
        private static void GenerateLevel6() {
            
        }
    }
}