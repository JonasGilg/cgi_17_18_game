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

		private static Model3D[] _models = {
			ModelLoaderObject3D.load( /*RandomModelPath()*/ "data/objects/asteroids/asteroid_0.obj", doAverageTangets: false,
				createVAO: false)
		};

		public static Asteroid GenerateAsteroid() {
			var fastNoise = new FastNoise();
			//------General Settings-------

			fastNoise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);
			fastNoise.SetSeed(Random.Next(1000, 100000));
			fastNoise.SetFrequency(0.01f);

			//-----Fractal Settings-------
			fastNoise.SetFractalType(FastNoise.FractalType.FBM);
			fastNoise.SetFractalOctaves(8);
			fastNoise.SetFractalLacunarity(2.0f);
			fastNoise.SetFractalGain(0.5f);
			//----- cellular settings----
			fastNoise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Euclidean);
			fastNoise.SetCellularReturnType(FastNoise.CellularReturnType.Distance);
			//position warp settings

			var model = ModelLoaderObject3D.load( /*RandomModelPath()*/ "data/objects/asteroids/asteroid_0.obj",
				doAverageTangets: false, createVAO: false);
			//Console.Out.WriteLine("Generate Asteroid with noise");
			for (var i = 0; i < model.Positions.Count; i++) {
				var noise = fastNoise.GetNoise(model.Positions[i].X, model.Positions[i].Y, model.Positions[i].Z);
				//Console.Out.WriteLine("Noise: "+(noise));
				model.Positions[i] = Vector3.Multiply(model.Positions[i], noise);
			}

			model.CreateVAO();
			return new Asteroid(model);
		}

		private static string RandomModelPath() {
			var basePath = "data/objects/asteroids/asteroid_";
			//TODO the random is probably not random enough and too dependend on number_of_models
			var number = Random.Next(0, NumberOfModels);
			basePath += number + ".obj";
			//Console.Out.WriteLine("randomized model path: " + basePath);
			return basePath;
		}
	}
}