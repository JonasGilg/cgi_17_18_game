using System;
using System.Collections.Generic;
using Engine;
using Engine.Texture;
using Game.Components;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
	public static class PlanetFactory {
		public enum PlanetTexture {
			SUN,
			NEPTUN,
			JUPITER
		}

		private static readonly Random RANDOM = new Random(100);

		private static readonly Dictionary<PlanetTexture, string> PLANET_TEXTURES = new Dictionary<PlanetTexture, string> {
			{PlanetTexture.SUN, "data/textures/planets/2k_sun.jpg"},
			{PlanetTexture.NEPTUN, "data/textures/planets/neptunemap.jpg"},
			{PlanetTexture.JUPITER, "data/textures/planets/2k_jupiter.jpg"}
		};


		public static Planet GeneratePlanet(PlanetTexture planetTexture, Vector3d position, Vector3d scale,
			Vector3d rotation) {
			var textureId = TextureManager.LoadTexture(PLANET_TEXTURES[planetTexture]);
			var result = new Planet(textureId) {
				TransformComponent = {
					Scale = scale,
					Position = position
				},
				MoveComponent = {AngularVelocity = rotation}
			};

			return result;
		}

		public static Planet GeneratePlanetWithAsteroidBeld(PlanetTexture planetTexture,
			AsteroidFactory.AsteroidType asteroidType, int numberAsteroids, Vector3d position, Vector3d scale,
			Vector3d rotation, GameObject referenceObject) {
			//TODO randomize angle, distance, speed, size, etc
			var planet = GeneratePlanet(planetTexture, position, scale, rotation);


			for (var i = 0; i < numberAsteroids; i++) {
				var asteroid = AsteroidFactory.GenerateGravityAsteroid(asteroidType, planet);

				asteroid.TransformComponent.Parent = planet.TransformComponent;
				//asteroid.TransformComponent.Scale = new Vector3d(100+_random.NextDouble()*500,100+_random.NextDouble()*500,100+_random.NextDouble()*500);
				asteroid.TransformComponent.Scale = new Vector3d(100);
				asteroid.MoveComponent.AngularVelocity = new Vector3d(0.0, 0.5, 0.0);
				asteroid.MoveComponent.LinearVelocity = new Vector3d(0.0, 0.0, 0.0);
				var component = (GravityMovement) asteroid.MoveComponent;
				component.currentAngle = i * 1.0 / 5;
				
				World.AddToWorld(asteroid);
			}
			return planet;
		}
	}
}