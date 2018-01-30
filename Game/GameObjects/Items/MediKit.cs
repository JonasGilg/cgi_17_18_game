using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Texture;
using Engine.Util;

namespace Game.GameObjects {
    public class MediKit : PowerUp {
        public static int colorTexture = TextureManager.LoadTexture("data/textures/supplyRing/gold.png");

        protected override CollisionComponent InitCollider() => new SphereCollider(this, Model, passiveMessage => {
            IO.PrintAsync("points collected");
        });

        protected override RenderComponent InitRenderer() => new RenderComponent(
            Model,
            MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
            new MaterialSettings {
                ColorTexture = colorTexture,
                Shininess = 1.5 /*,
                        MetalnessTexture = TextureManager.LoadTexture("data/textures/pbr/" + type.ToString().ToLower() + "-metalness.png"),
                        RoughnessTexture = TextureManager.LoadTexture("data/textures/pbr/" + type.ToString().ToLower() + "-roughness.png")*/
            },
            this
        );
    }
}