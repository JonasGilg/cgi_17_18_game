using System;
using Engine.Model;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
    public static class AsteroidFactory {

        private static int NUMBER_OF_MODELS = 2;

        public static Asteroid generateAsteroid() {
            var _model = new ModelLoaderObject3D(randomModelPath());
            
            


            return new Asteroid(_model);
        }

        private static String randomModelPath() {
            String basePath = "data/objects/asteroids/asteroid_";
            //TODO the random is probably not random enough and too dependend on number_of_models
            int number = new Random(NUMBER_OF_MODELS).Next();
            basePath += number+".obj";
            Console.Out.WriteLine("randomized model path: "+basePath);
            return basePath;
        }
        
    }
}