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
			var bh = new BlackHole {
				TransformComponent = {
					Position = position,
					Scale = scale
				}
			};
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

	}
}