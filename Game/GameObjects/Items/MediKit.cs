using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Texture;
using Engine.Util;

namespace Game.GameObjects {
	public class MediKit : PowerUp {
		private const string PATH = "data/textures/medikit";

		private static readonly MaterialSettings MATERIAL_SETTINGS = new MaterialSettings {
			ColorTexture = TextureManager.LoadTexture(PATH + "/color.png"),
			AOTexture = TextureManager.LoadTexture(PATH + "/ao.png"),
			NormalTexture = TextureManager.LoadTexture(PATH + "/normal.png"),
			MetalnessTexture = TextureManager.LoadTexture(PATH + "/metalness.png"),
			RoughnessTexture = TextureManager.LoadTexture(PATH + "/roughness.png"),
			GlowTexture = TextureManager.LoadTexture(PATH + "/blur.png")
		};

		protected override CollisionComponent InitCollider() => new SphereCollider(this, MODEL, passiveMessage => {
			if (passiveMessage.OtherCollisonComponent.GameObject is SpaceShip ship) {
				IO.PrintAsync("medikit collected");
				ship.HealthComponent.HealHp(1);
				Destroy();
			}
		});

		protected override RenderComponent InitRenderer() => new RenderComponent(
			MODEL,
			MaterialManager.GetMaterial(Material.PBR),
			MATERIAL_SETTINGS,
			this
		);
	}
}