using System;
using Engine.Model;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
    public static class AsteroidFactory {

        private static int NUMBER_OF_MODELS = 3;
        private static Random _random = new Random();

        public static Asteroid GenerateAsteroid() {
            var model = new ModelLoaderObject3D(RandomModelPath(), createVAO:false);
            
            var fastNoise = new FastNoise();
            
            fastNoise.SetNoiseType(FastNoise.NoiseType.Perlin);
            Vector3 oldPosition;
            
            model.Positions.ForEach(position => {
                oldPosition = position;
                position.X = fastNoise.GetNoise(oldPosition.X, oldPosition.Y, oldPosition.Z);
                position.Y = fastNoise.GetNoise(oldPosition.X, oldPosition.Y, oldPosition.Z);
                position.Z = fastNoise.GetNoise(oldPosition.X, oldPosition.Y, oldPosition.Z);
            });
                
            
            
            model.CreateVAO();
            return new Asteroid(model);
        }

        private static string RandomModelPath() {
            var basePath = "data/objects/asteroids/asteroid_";
            //TODO the random is probably not random enough and too dependend on number_of_models
            var number = _random.Next(0,NUMBER_OF_MODELS);
            basePath += number+".obj";
            Console.Out.WriteLine("randomized model path: "+basePath);
            return basePath;
        }
        
    }
}