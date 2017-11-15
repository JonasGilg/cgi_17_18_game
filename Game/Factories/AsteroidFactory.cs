using System;
using System.Collections.Generic;
using Engine;
using Engine.Texture;
using Game.GameObjects;

namespace Game.Utils {
	public static class AsteroidFactory {
		private static readonly Random RANDOM = new Random();
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
			var asteroid = new Asteroid(ASTEROID_MODEL_DICTIONARY[type], textureId);
			return asteroid;
		}

		public static Asteroid GenerateGravityAsteroid(AsteroidType type, GameObject referenceObject) {
			var rng = RANDOM.Next(0, 6);
			var textureId = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[type]);
			var asteroid = new Asteroid(ASTEROID_MODEL_DICTIONARY[type]+rng+".obj", textureId, referenceObject);
			return asteroid;
		}
	}
}