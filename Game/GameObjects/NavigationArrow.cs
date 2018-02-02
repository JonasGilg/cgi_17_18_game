using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Engine.Util;
using Game.GamePlay;
using OpenTK;

namespace Game.GameObjects {
    public class NavigationArrow : GameObject{
        public readonly RenderComponent RenderComponent;
        public static Model3D model = ModelLoaderObject3D.Load("data/objects/navigation_arrow_fixed.obj");

        public NavigationArrow(){
            RenderComponent = new RenderComponent(
                model,
                MaterialManager.GetMaterial(Material.PBR),
                new MaterialSettings {
                    ColorTexture = TextureManager.LoadTexture("data/textures/supplyRing/violett.png"),
                    NormalTexture = TextureManager.LoadTexture("data/textures/defaultNormalMap.png"),
                    MetalnessTexture = TextureManager.LoadTexture("data/textures/simpleBlack.png"),
                    RoughnessTexture = TextureManager.LoadTexture("data/textures/simpleBlack.png"),
                    AOTexture = TextureManager.LoadTexture("data/textures/simpleWhite.png"),
                    GlowTexture = TextureManager.LoadTexture("data/textures/planets/blur.png")
                },
                this
            );
        }

        public override void Awake() {
            base.Awake();
            RenderEngine.RegisterDynamicRenderComponent(RenderComponent);
            IO.PrintAsync($"NavigationArrow awake");
            Radius = RenderComponent.Model.Radius(TransformComponent.Scale);
            RenderComponent.AABB = RenderComponent.AABB * TransformComponent.Scale;
        }
        
        protected override void OnDestroy() {
            RenderEngine.UnregisterDynamicRenderComponent(RenderComponent);
        }

        public override void Update() {
            base.Update();
            IO.PrintAsync($"NavigationArrow- position {TransformComponent.Position}");
            TransformComponent.Position = GamePlayEngine.PLAYER_SPACESHIP.TransformComponent.Position + new Vector3d(0,GamePlayEngine.PLAYER_SPACESHIP.Radius , 0);
            RenderComponent.Update();
        }
    }
}