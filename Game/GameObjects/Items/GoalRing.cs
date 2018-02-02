using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Texture;
using Engine.Util;
using Game.GamePlay;

namespace Game.GameObjects {
    public class GoalRing : Ring {
        private static string PATH = "data/textures/pointring/violett";

        private static MaterialSettings materialSettings = new MaterialSettings {
            ColorTexture = TextureManager.LoadTexture(PATH + "/color.png"),
            AOTexture = TextureManager.LoadTexture(PATH + "/ao.png"),
            NormalTexture = TextureManager.LoadTexture(PATH + "/normal.png"),
            MetalnessTexture = TextureManager.LoadTexture(PATH + "/metalness.png"),
            RoughnessTexture = TextureManager.LoadTexture(PATH + "/roughness.png"),
            GlowTexture = TextureManager.LoadTexture(PATH + "/blur.png")
        };
        
        protected override CollisionComponent InitCollider() => new SphereCollider(this, Model, passiveMessage => {
            if (GamePlayEngine.PLAYER_SPACESHIP.Equals(passiveMessage.OtherCollisonComponent.GameObject)) {
                GamePlayEngine.CheckPointPassed(this);
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