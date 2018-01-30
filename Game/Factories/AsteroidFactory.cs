using System;
using System.Collections.Generic;
using Engine;
using Engine.GUI;
using Engine.Material;
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

	internal static class AsteroidTextureRegistry {
		//# of folders in textures/asteroid/PBR
		private const int NUM_PACKS = 5;
		private const string PATH = "data/textures/asteroids/PBR/";
		private static readonly Random RANDOM;
		private static readonly List<string> ASTEROID_TEXTURES_DICTIONARY =
			new List<string>(){
				PATH+"cavefloor1/",
				PATH+"cratered-rock/",
				PATH+"limestone-rock/",
				PATH+"rock_vstreaks/",
				PATH+"slate2-tiled/"
			};

		private static readonly MaterialSettings[] ASTEROID_MATERIALS;
		
		static AsteroidTextureRegistry(){
			ASTEROID_MATERIALS = new MaterialSettings[NUM_PACKS];
			RANDOM = new Random();

			for (int i = 0; i < NUM_PACKS; i++) {
				ASTEROID_MATERIALS[i] = new MaterialSettings {
					AOTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i]+"ao.png"),
					ColorTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i]+"color.png"),
					NormalTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i]+"normal.png"),
					MetalnessTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i]+"metalness.png"),
					RoughnessTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i]+"roughness.png"),
					GlowTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i]+"blur.png")
				};
			}
		}


		public static MaterialSettings GetRandomMaterialSettings() => ASTEROID_MATERIALS[RANDOM.Next(NUM_PACKS)];
	}
	
	public static class AsteroidFactory {
		public enum AsteroidType {
			STANDARD
		}

		private static readonly Dictionary<AsteroidType, string> ASTEROID_MODEL_DICTIONARY =
			new Dictionary<AsteroidType, string> {
				{AsteroidType.STANDARD, "data/objects/asteroids/asteroid_"}
			};

		

		public static Asteroid GenerateAsteroid(AsteroidType type) {
			
			var asteroid = new Asteroid(AsteroidModelRegistry.GetRandomAsteroidModel(), AsteroidTextureRegistry.GetRandomMaterialSettings());
			
			return asteroid;
		}

		public static Asteroid GenerateGravityAsteroid(AsteroidType type, GameObject referenceObject) {
			
			var asteroid = new Asteroid(AsteroidModelRegistry.GetRandomAsteroidModel(), AsteroidTextureRegistry.GetRandomMaterialSettings(), referenceObject);
			return asteroid;
		}

		
	}
}