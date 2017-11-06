using System;
using System.Collections.Generic;
using Engine;
using Engine.Texture;
using Game.GameObjects;
using OpenTK;
using OpenTK.Graphics.ES11;

namespace Game.Utils {
    public static class PlanetFactory {
        public enum PlanetTexture {
            Sun,
            Neptun,
            Jupiter
        }

        private static Random _random;
        
        public static readonly Dictionary<PlanetTexture,string> PlanetTextures = new Dictionary <PlanetTexture,string> {
            {PlanetTexture.Sun,"data/textures/planets/2k_sun.jpg"},
            {PlanetTexture.Neptun,"data/textures/planets/neptunemap.jpg"},
            {PlanetTexture.Jupiter,"data/textures/planets/2k_jupiter.jpg"}
            
        };
        
        
        public static Planet GeneratePlanet(PlanetTexture planetTexture, Vector3d position, Vector3d scale, Vector3d rotation) {
            var textureId = TextureManager.LoadTexture(PlanetTextures[planetTexture]);
            var result = new Planet(textureId) {
                TransformComponent = {
                    Scale = scale,
                    Position = position
                },
                MoveComponent = { AngularVelocity = rotation }
            };

            return result;
        }

        public static Planet generatePlanetWithAsteroidBeld(PlanetTexture planetTexture, AsteroidFactory.AsteroidType asteroidType, int numberAsteroids, Vector3d position, Vector3d scale, Vector3d rotation) {
            var planet = GeneratePlanet(planetTexture, position, scale, rotation);
            _random = new Random(1);
            for (int i = 0; i < numberAsteroids; i++) {
                var asteroid = AsteroidFactory.GenerateAsteroid(asteroidType);
                //asteroid.TransformComponent.Parent = planet.TransformComponent;
                asteroid.TransformComponent.Position = planet.TransformComponent.Position +new Vector3d(0,500*i,-3000);
                asteroid.TransformComponent.Scale= new Vector3d(100);
                asteroid.MoveComponent.AngularVelocity= new Vector3d(0.0,0.5,0.0);
                asteroid.MoveComponent.LinearVelocity= new Vector3d(0.0,0.0,0.0);

               // Console.Out.WriteLine("asteroid["+i+"] position: "+asteroid.TransformComponent.WorldPosition.ToString());
                World.AddToWorld(asteroid);
            }

            return planet;
        }
    }
}