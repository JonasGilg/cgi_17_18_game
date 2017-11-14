using System;
using System.Collections.Generic;
using Engine;
using Engine.Texture;
using Game.GameObjects;

namespace Game.Utils {
	public static class AsteroidFactory {
		private static Random _random = new Random();
		public enum AsteroidType {
			Standard,
			Strawberry
		}

		public static readonly Dictionary<AsteroidType, string> AsteroidModelDictionary =
			new Dictionary<AsteroidType, string> {
				{AsteroidType.Standard, "data/objects/asteroids/asteroid_"},
				{AsteroidType.Strawberry, "data/objects/asteroids/strawberry.obj"}
			};

		public static readonly Dictionary<AsteroidType, string> AsteroidTexturesDictionary =
			new Dictionary<AsteroidType, string> {
				{AsteroidType.Standard, "data/textures/asteroids/asteroid_0.png"},
				{AsteroidType.Strawberry, "data/textures/asteroids/strawberry.png"}
			};

		public static Asteroid GenerateAsteroid(AsteroidType type) {
			var textureId = TextureManager.LoadTexture(AsteroidTexturesDictionary[type]);
			var asteroid = new Asteroid(AsteroidModelDictionary[type], textureId);
			return asteroid;
		}

		public static Asteroid GenerateGravityAsteroid(AsteroidType type, GameObject referenceObject) {
			var rng = _random.Next(0, 6);
			var textureId = TextureManager.LoadTexture(AsteroidTexturesDictionary[type]);
			var asteroid = new Asteroid(AsteroidModelDictionary[type]+rng+".obj", textureId, referenceObject);
			return asteroid;
		}
	}
}