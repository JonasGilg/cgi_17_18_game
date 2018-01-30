using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Texture;
using Game.GamePlay;

namespace Game.GameObjects {
    public class GoalRing : Ring {
        private static int colorTexture = TextureManager.LoadTexture("data/textures/supplyRing/violett.png");
        
        protected override CollisionComponent InitCollider() => new SphereCollider(this, Model, passiveMessage => {
            if (GamePlayEngine.playerSpaceship.Equals(passiveMessage.OtherCollisonComponent.GameObject)) {
                GamePlayEngine.supplyRingCollected(this);
            }
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