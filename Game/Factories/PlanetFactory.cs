using System.Collections.Generic;
using Engine.Material;
using Engine.Texture;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
	public static class PlanetFactory {
		public enum PlanetTexture {
			SUN,
			NEPTUN,
			JUPITER,
			REDPLANET,
			EARTHLIKE
		}

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

			if (planetTexture == PlanetTexture.SUN) {
				result.RenderComponent.Material = MaterialManager.GetMaterial(Material.SUN_LAVAFLOW);
			}

			result.Instantiate();
			return result;
		}
	}
}