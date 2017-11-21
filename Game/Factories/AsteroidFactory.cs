using System;
using System.Collections.Generic;
using Engine;
using Engine.Model;
using Engine.Texture;
using Game.GameObjects;

namespace Game.Utils {
	internal static class AsteroidModelRegistry {
		private const int NUM_MODELS = 7;
		private const string PATH = "data/objects/asteroids/asteroid_";

		private static readonly Model3D[] ASTEROID_MODELS;
		private static readonly Random RANDOM;

		static AsteroidModelRegistry() {
			ASTEROID_MODELS = new Model3D[NUM_MODELS];
			RANDOM = new Random();

			for (var i = 0; i < NUM_MODELS; i++) {
				ASTEROID_MODELS[i] = ModelLoaderObject3D.Load(PATH + i + ".obj");
			}
		}

		public static Model3D GetRandomAsteroidModel() => ASTEROID_MODELS[RANDOM.Next(NUM_MODELS)];
	}
	
	public static class AsteroidFactory {
		public enum AsteroidType {
			STANDARD,
			STRAWBERRY
		}

		private static readonly Dictionary<AsteroidType, string> ASTEROID_MODEL_DICTIONARY =
			new Dictionary<AsteroidType, string> {
				{AsteroidType.STANDARD, "data/objects/asteroids/asteroid_"},
				{AsteroidType.STRAWBERRY, "data/objects/asteroids/strawberry.obj"}
			};

		private static readonly Dictionary<AsteroidType, string> ASTEROID_TEXTURES_DICTIONARY =
			new Dictionary<AsteroidType, string> {
				{AsteroidType.STANDARD, "data/textures/asteroids/asteroid_0.png"},
				{AsteroidType.STRAWBERRY, "data/textures/asteroids/strawberry.png"}
			};

		public static Asteroid GenerateAsteroid(AsteroidType type) {
			var textureId = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[type]);
			var asteroid = new Asteroid(AsteroidModelRegistry.GetRandomAsteroidModel(), textureId);
			return asteroid;
		}

		public static Asteroid GenerateGravityAsteroid(AsteroidType type, GameObject referenceObject) {
			var textureId = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[type]);
			var asteroid = new Asteroid(AsteroidModelRegistry.GetRandomAsteroidModel(), textureId, referenceObject);
			return asteroid;
		}
	}
}