using System;
using Engine.Model;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
    public static class AsteroidFactory {

        private static int NUMBER_OF_MODELS = 3;
        private static Random _random = new Random();
        

        public static Asteroid generateAsteroid() {
            var _model = new ModelLoaderObject3D(randomModelPath(), createVAO:false);
            
            
            FastNoise fastNoise = new FastNoise();
            
            fastNoise.SetNoiseType(FastNoise.NoiseType.Perlin);
            fastNoise.SetFrequency(0.1f);
            fastNoise.SetSeed(1337);
            Vector3 oldPosition;
            
            _model.Positions.ForEach(position => {
                oldPosition = position;
                position.X += fastNoise.GetNoise(oldPosition.X, oldPosition.Y, oldPosition.Z);
                position.Y += fastNoise.GetNoise(oldPosition.X, oldPosition.Y, oldPosition.Z);
                position.Z += fastNoise.GetNoise(oldPosition.X, oldPosition.Y, oldPosition.Z);
            });
           
             //faces anpassen, normalen anpassen etc.  
            
            
            _model.CreateVAO();
            return new Asteroid(_model);
        }

        private static String randomModelPath() {
            String basePath = "data/objects/asteroids/asteroid_";
            //TODO the random is probably not random enough and too dependend on number_of_models
            int number = _random.Next(0,NUMBER_OF_MODELS);
            basePath += number+".obj";
            Console.Out.WriteLine("randomized model path: "+basePath);
            return basePath;
        }
        
    }
}