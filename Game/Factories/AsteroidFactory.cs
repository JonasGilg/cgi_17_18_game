using System;
using System.Collections.Generic;
using Engine;
using Engine.Model;
using Engine.Texture;
using Game.GameObjects;

namespace Game.Utils {
	public static class AsteroidFactory {
		private static readonly Random RANDOM = new Random();
		public enum AsteroidType {
			STANDARD,
			STRAWBERRY
		}
		
		//MODELS
		
		private static readonly List<Model3D> STANDARD_MODELS = new List<Model3D> {
			ModelLoaderObject3D.Load("data/objects/asteroids/standard/asteroid_0.obj"),
			ModelLoaderObject3D.Load("data/objects/asteroids/standard/asteroid_1.obj"),
			ModelLoaderObject3D.Load("data/objects/asteroids/standard/asteroid_2.obj"),
			ModelLoaderObject3D.Load("data/objects/asteroids/standard/asteroid_3.obj"),
			ModelLoaderObject3D.Load("data/objects/asteroids/standard/asteroid_4.obj"),
			ModelLoaderObject3D.Load("data/objects/asteroids/standard/asteroid_5.obj"),
			ModelLoaderObject3D.Load("data/objects/asteroids/standard/asteroid_6.obj")
		};
		private static readonly List<Model3D> STRAWBERRY_MODELS = new List<Model3D> {
			ModelLoaderObject3D.Load("data/objects/asteroids/strawberry/strawberry.obj")
		};
		private static readonly Dictionary<AsteroidType, List<Model3D>> ASTEROID_MODEL_DICTIONARY =
			new Dictionary<AsteroidType,List<Model3D>> {
				{AsteroidType.STANDARD, STANDARD_MODELS},
				{AsteroidType.STRAWBERRY, STRAWBERRY_MODELS}
				
			};

		
		//TEXTURES
		
		private static readonly List<int> STANDARD_TEXTURES = new List<int> {
			TextureManager.LoadTexture("data/textures/asteroids/standard/textures/asteroid_0.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/textures/asteroid_1.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/textures/asteroid_2.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/textures/asteroid_3.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/textures/asteroid_4.png"),
			
			TextureManager.LoadTexture("data/textures/asteroids/standard/textures/jpeg/jasteroid_0.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/textures/jpeg/jasteroid_1.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/textures/jpeg/jasteroid_2.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/textures/jpeg/jasteroid_3.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/textures/jpeg/jasteroid_4.png")
		};

		private static readonly List<int> STANDARD_NORMALMAPS = new List<int> {
			TextureManager.LoadTexture("data/textures/asteroids/standard/normalmaps/nm_asteroid_0.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/normalmaps/nm_asteroid_1.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/normalmaps/nm_asteroid_2.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/normalmaps/nm_asteroid_3.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/normalmaps/nm_asteroid_4.png"),

			TextureManager.LoadTexture("data/textures/asteroids/standard/normalmaps/jpeg/nm_jasteroid_0.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/normalmaps/jpeg/nm_jasteroid_1.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/normalmaps/jpeg/nm_jasteroid_2.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/normalmaps/jpeg/nm_jasteroid_3.png"),
			TextureManager.LoadTexture("data/textures/asteroids/standard/normalmaps/jpeg/nm_jasteroid_4.png")
		};

		private static readonly List<int> STRAWBERY_TEXTURES = new List<int> {
			TextureManager.LoadTexture("data/textures/asteroids/strawberry/textures/strawberry.png")
		};
		
		private static readonly Dictionary<AsteroidType, List<int>> ASTEROID_TEXTURES_DICTIONARY =
			new Dictionary<AsteroidType, List<int>> {
				{AsteroidType.STANDARD, STANDARD_TEXTURES},
				{AsteroidType.STRAWBERRY, STRAWBERY_TEXTURES}
			};

		public static Asteroid GenerateAsteroid(AsteroidType type) {
			var randomModelNumber = RANDOM.Next(0, ASTEROID_MODEL_DICTIONARY[type].Count - 1);
			
			var randomTextureNumber = RANDOM.Next(0, ASTEROID_TEXTURES_DICTIONARY[type].Count - 1);
			var model = ASTEROID_MODEL_DICTIONARY[type][randomModelNumber];
			var textureId = ASTEROID_TEXTURES_DICTIONARY[type][randomTextureNumber];
			var asteroid = new Asteroid(model, textureId);
			return asteroid;
		}

		public static Asteroid GenerateGravityAsteroid(AsteroidType type, GameObject referenceObject) {
			var randomModelNumber = RANDOM.Next(0, ASTEROID_MODEL_DICTIONARY[type].Count);
			var randomTextureNumber = RANDOM.Next(0, ASTEROID_TEXTURES_DICTIONARY[type].Count - 1);
			var model = ASTEROID_MODEL_DICTIONARY[type][randomModelNumber];
			var textureId = ASTEROID_TEXTURES_DICTIONARY[type][randomTextureNumber];
			var asteroid = new Asteroid(model, textureId, referenceObject);
			return asteroid;
		}
	}
}