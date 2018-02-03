using System.Collections.Generic;
using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Sound;
using Engine.Texture;
using Engine.Util;

namespace Game.GameObjects {
	public enum PointType {
		COPPER,
		SILVER,
		GOLD
	}

	internal static class PointRingTextureRegistry {
		private const string PATH = "data/textures/pointring/";
		public static readonly Dictionary<PointType, MaterialSettings> MATERIAL_SETTINGS;

		static PointRingTextureRegistry() {
			MATERIAL_SETTINGS = new Dictionary<PointType, MaterialSettings>();
			foreach (var ptype in EnumUtil.GetValues<PointType>()) {
				MATERIAL_SETTINGS.Add(ptype, new MaterialSettings {
					ColorTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/color.png"),
					AOTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/ao.png"),
					NormalTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/normal.png"),
					MetalnessTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/metalness.png"),
					RoughnessTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/roughness.png"),
					GlowTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/blur.png")
				});
			}
		}
	}

	public class PointRing : Ring {
		private readonly int points;

		public PointRing(PointType type) {
			switch (type) {
				case PointType.COPPER:
					points = 1;
					break;
				case PointType.SILVER:
					points = 5;
					break;
				case PointType.GOLD:
					points = 10;
					break;
			}

			RenderComponent.MaterialSettings = PointRingTextureRegistry.MATERIAL_SETTINGS[type];
		}

		private static readonly Sound RING_SOUND = new Sound("data/sound/goldring.wav");

		protected override CollisionComponent InitCollider() => new SphereCollider(this, MODEL, passiveMessage => {
			Statistics.IncreaseScore(points);
			RING_SOUND.Play();
			Destroy();
		});

		protected override RenderComponent InitRenderer() => new RenderComponent(
			MODEL,
			MaterialManager.GetMaterial(Material.PBR),
			new MaterialSettings {
				Shininess = 1.5
			},
			this
		);
	}
}