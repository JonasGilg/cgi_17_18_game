﻿using System;
using System.Collections.Generic;
using Engine;
using Engine.Texture;
using Game.Components;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
	public static class PlanetFactory {
		public enum PlanetTexture {
			Sun,
			Neptun,
			Jupiter
		}

		private static Random _random;

		public static readonly Dictionary<PlanetTexture, string> PlanetTextures = new Dictionary<PlanetTexture, string> {
			{PlanetTexture.Sun, "data/textures/planets/2k_sun.jpg"},
			{PlanetTexture.Neptun, "data/textures/planets/neptunemap.jpg"},
			{PlanetTexture.Jupiter, "data/textures/planets/2k_jupiter.jpg"}
		};


		public static Planet GeneratePlanet(PlanetTexture planetTexture, Vector3d position, Vector3d scale,
			Vector3d rotation) {
			var textureId = TextureManager.LoadTexture(PlanetTextures[planetTexture]);
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
			Vector3d rotation) {
			//TODO randomize angle, distance, speed, size, etc
			var planet = GeneratePlanet(planetTexture, position, scale, rotation);
			
			
			for (var i = 0; i < numberAsteroids; i++) {
				var asteroid = AsteroidFactory.GenerateGravityAsteroid(asteroidType, planet);

				asteroid.TransformComponent.Parent = planet.TransformComponent;
				asteroid.TransformComponent.Scale = new Vector3d(100);
				asteroid.MoveComponent.AngularVelocity = new Vector3d(0.0, 0.5, 0.0);
				asteroid.MoveComponent.LinearVelocity = new Vector3d(0.0, 0.0, 0.0);
				GravityMovement component = (GravityMovement) asteroid.MoveComponent;
				component._currentAngle = i * 1.0 / 5;
				component.Speed = 1.0;
				component.Distance = 4000;
				World.AddToWorld(asteroid);
			}

			return planet;
		}
	}
}