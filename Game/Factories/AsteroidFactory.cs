using System;
using Engine.Model;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
	public static class AsteroidFactory {
		private const int NumberOfModels = 3;
		private const int TextureSizeX = 1024;
		private const int TextureSizeY = 1024;
		private static readonly Random Random = new Random();


		public static Asteroid GenerateAsteroid() {
			
			var model = ModelLoaderObject3D.Load( /*RandomModelPath()*/ "data/objects/asteroids/asteroid_0.obj",
				doAverageTangets: false, createVAO: false);

			model.CreateVAO();
			Asteroid asteroid = new Asteroid(model);
			return asteroid;
		}

		private static string RandomModelPath() {
			var basePath = "data/objects/asteroids/asteroid_";
			//TODO the random is probably not random enough and too dependend on number_of_models
			var number = Random.Next(0, NumberOfModels);
			basePath += number + ".obj";
			//Console.Out.WriteLine("randomized model path: " + basePath);
			return basePath;
		}

		private static Vector3 randomStretching() {
			var vector = new Vector3();
			vector.X = (float) Random.Next(1, 100);

			return vector;
		}
	}
}