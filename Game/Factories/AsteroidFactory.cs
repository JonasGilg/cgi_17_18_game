using System;
using System.Collections.Generic;
using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Engine.Util;
using Game.GameObjects;
using OpenTK;

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
			new List<string> {
				PATH + "cavefloor1/",
				PATH + "cratered-rock/",
				PATH + "limestone-rock/",
				PATH + "rock_vstreaks/",
				PATH + "slate2-tiled/"
			};

		private static readonly MaterialSettings[] ASTEROID_MATERIALS;

		static AsteroidTextureRegistry() {
			ASTEROID_MATERIALS = new MaterialSettings[NUM_PACKS];
			RANDOM = new Random();

			for (int i = 0; i < NUM_PACKS; i++) {
				ASTEROID_MATERIALS[i] = new MaterialSettings {
					AOTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i] + "ao.png"),
					ColorTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i] + "color.png"),
					NormalTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i] + "normal.png"),
					MetalnessTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i] + "metalness.png"),
					RoughnessTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i] + "roughness.png"),
					GlowTexture = TextureManager.LoadTexture(ASTEROID_TEXTURES_DICTIONARY[i] + "blur.png")
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

		public static Asteroid GenerateAsteroid() => new Asteroid(AsteroidModelRegistry.GetRandomAsteroidModel(), AsteroidTextureRegistry.GetRandomMaterialSettings());

		public static Asteroid GenerateGravityAsteroid(GameObject referenceObject, double startAngle, double distance, double speed) {
			var asteroid = new Asteroid(AsteroidModelRegistry.GetRandomAsteroidModel(),
				AsteroidTextureRegistry.GetRandomMaterialSettings(), referenceObject, startAngle, distance, speed);

			return asteroid;
		}

		public static List<Asteroid> GenerateAsteroidRingForCheckpoint(GameObject parent, Vector3d rotationAxle, Vector3d direction, double scale) {

			var number = 6;
			var angleStep = MathHelper.TwoPi / 6;
			var distanceFromCenter = 700;
			scale *= 0.6;
			for (int i = 0; i < 6; i++) {
				var finalPos = Quaterniond.FromAxisAngle(rotationAxle,angleStep*i).Rotate(direction*distanceFromCenter)+parent.TransformComponent.Position;
				
				GenerateSingleAsteroid(finalPos, scale);
			}
			
			return new List<Asteroid>();
		}
		
		public static List<Asteroid> GenerateAsteroidRing(Vector3d center, Vector3d eulerAngle, int count, double radius, double scale = 5.0) {
			var asteroids = new List<Asteroid>();
			if (count < 1) return asteroids; //nothing to generate if count is 0


			for (int i = 0; i < count; i++) {
				var pos = new Vector3d(radius * Math.Cos(i * Math.PI * 2 / count), radius * Math.Sin(i * Math.PI * 2 / count), center.Z);
				var rotatedPos = Quaterniond.FromEulerAngles(eulerAngle.ToRadiansVector3D()).Rotate(pos) + center;
				asteroids.Add(GenerateSingleAsteroid(rotatedPos, scale));
			}


			return asteroids;
		}

		public static List<Asteroid> GenerateAsteroidTorus(Vector3d center, Vector3d eulerAngle, int count, int innerRadius, int outerRadius, int ringHeight, double scale = 5.0) {
			var asteroids = new List<Asteroid>();
			if (count < 1) return asteroids; //nothing to generate if count is 0

			var rand = new Random();
			var heightVariance = ringHeight / 2;

			for (int i = 0; i < count; i++) {
				var radius = rand.Next(innerRadius, outerRadius);
				var pos = new Vector3d(radius * Math.Cos(i * Math.PI * 2 / count),
					radius * Math.Sin(i * Math.PI * 2 / count),
					rand.Next(((int) center.Z) - heightVariance, ((int) center.Z) + heightVariance));

				//rotate by given vector
				var rotatedPos = Quaterniond.FromEulerAngles(eulerAngle.ToRadiansVector3D()).Rotate(pos) + center;
				asteroids.Add(GenerateSingleAsteroid(rotatedPos, scale));
			}


			return asteroids;
		}

		public static Asteroid GenerateSingleAsteroid(Vector3d position, double scale = 1.0) {
			var asteroid = GenerateAsteroid();
			asteroid.TransformComponent.Scale = new Vector3d(scale);
			asteroid.TransformComponent.Position = position;
			asteroid.MoveComponent.AngularVelocity = new Vector3d(0, 0, 0);

			asteroid.Instantiate();
			//IO.PrintAsync(""+asteroid.TransformComponent.WorldPosition);
			return asteroid;
		}
	}
}