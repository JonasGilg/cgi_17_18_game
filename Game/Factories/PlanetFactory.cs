﻿using System;
using System.Collections.Generic;
using Engine;
using Engine.GUI;
using Engine.Material;
using Engine.Texture;
using Game.Components;
using Game.GameObjects;
using OpenTK;
 using OpenTK.Graphics.OpenGL;

namespace Game.Utils {
	public static class PlanetFactory {
		public enum PlanetTexture {
			SUN,
			NEPTUN,
			JUPITER,
			REDPLANET,
			EARTHLIKE
		}

		private static readonly Random RANDOM = new Random(100);

		private static readonly Dictionary<PlanetTexture, string> PLANET_TEXTURES = new Dictionary<PlanetTexture, string> {
			{PlanetTexture.SUN, "data/textures/sun.png"},
			{PlanetTexture.NEPTUN, "data/textures/planets/neptunemap.jpg"},
			{PlanetTexture.JUPITER, "data/textures/planets/2k_jupiter.jpg"},
			{PlanetTexture.REDPLANET, "data/textures/planets/red_planet.jpg"},
			{PlanetTexture.EARTHLIKE, "data/textures/planets/earthlike.png"}
		};

		public static BlackHole GenerateBlackhole(Vector3d position, Vector3d scale) {
			var bh = new BlackHole();
			bh.TransformComponent.Position = position;
			bh.TransformComponent.Scale = scale;
			bh.Instantiate();
			return bh;
		}

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
			
			//special case for the sun
			if (planetTexture == PlanetTexture.SUN) {
				result.RenderComponent.Material = MaterialManager.GetMaterial(Material.SUN_LAVAFLOW);
			}

			//TODO every planet gets tagged now. But we need a List of Planets at some time anyways...  :)
			
			result.Instantiate();
			return result;
		}

		public static Planet GeneratePlanetWithAsteroidBeld(PlanetTexture planetTexture,
			int numberAsteroids, Vector3d position, Vector3d scale,
			Vector3d rotation, double radius, double rotationSpeed) {
			//TODO randomize angle, distance, speed, size, etc
			var planet = GeneratePlanet(planetTexture, position, scale, rotation);

			
			
			
			
			for (var i = 0; i < numberAsteroids; i++) {
				var asteroid = AsteroidFactory.GenerateGravityAsteroid(planet,0.0, radius, rotationSpeed);

				asteroid.TransformComponent.Parent = planet.TransformComponent;
				//asteroid.TransformComponent.Scale = new Vector3d(100+_random.NextDouble()*500,100+_random.NextDouble()*500,100+_random.NextDouble()*500);
				asteroid.TransformComponent.Scale = new Vector3d(150);
				asteroid.MoveComponent.AngularVelocity = new Vector3d(0.0, 0.5, 0.0);
				asteroid.MoveComponent.LinearVelocity = new Vector3d(0.0, 0.0, 0.0);
				var component = (GravityMovement) asteroid.MoveComponent;
				component.currentAngle = i * MathHelper.TwoPi/numberAsteroids;
				
				asteroid.Instantiate();
			}
			
			return planet;
		}
	}
}