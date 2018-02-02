using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Texture;
using Engine.Util;

namespace Game.GameObjects {
    public class MediKit : PowerUp {
        private static string PATH = "data/textures/medikit";
        private static MaterialSettings materialSettings = new MaterialSettings {
            ColorTexture = TextureManager.LoadTexture(PATH + "/color.png"),
            AOTexture = TextureManager.LoadTexture(PATH + "/ao.png"),
            NormalTexture = TextureManager.LoadTexture(PATH + "/normal.png"),
            MetalnessTexture = TextureManager.LoadTexture(PATH + "/metalness.png"),
            RoughnessTexture = TextureManager.LoadTexture(PATH + "/roughness.png"),
            GlowTexture = TextureManager.LoadTexture(PATH + "/blur.png")
        };

        protected override CollisionComponent InitCollider() => new SphereCollider(this, Model, passiveMessage => {
            if (passiveMessage.OtherCollisonComponent.GameObject is SpaceShip ship) {
                IO.PrintAsync("medikit collected");
                ship.healthComponent.healHP(1);
                Destroy();
            }
        });

        protected override RenderComponent InitRenderer() => new RenderComponent(
            Model,
            MaterialManager.GetMaterial(Material.PBR),
            materialSettings,
            this
        );
    }
}