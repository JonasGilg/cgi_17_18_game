using System.Collections.Generic;
using Engine;
using Engine.Texture;
using Game.GameObjects;

namespace Game.Utils {
	public static class AsteroidFactory {
		public enum AsteroidType {
			Standard,
			Strawberry
		}

		public static readonly Dictionary<AsteroidType, string> AsteroidModelDictionary =
			new Dictionary<AsteroidType, string> {
				{AsteroidType.Standard, "data/objects/asteroids/asteroid_0.obj"},
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
			var textureId = TextureManager.LoadTexture(AsteroidTexturesDictionary[type]);
			var asteroid = new Asteroid(AsteroidModelDictionary[type], textureId, referenceObject);
			return asteroid;
		}
	}
}