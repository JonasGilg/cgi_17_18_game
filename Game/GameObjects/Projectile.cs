using System.Collections.Generic;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Game.Components;

namespace Game.GameObjects {
    public class Projectile : GameObject{
        private static readonly Model3D MODEL = ModelLoaderObject3D.Load("data/objects/Planet.obj");
        private static MaterialSettings MATERIAL_SETTINGS;
        
        public static void LoadTextTures() => MATERIAL_SETTINGS = new MaterialSettings {
            ColorTexture = TextureManager.LoadTexture("data/textures/powerfulred.png"),
            NormalTexture = TextureManager.LoadTexture("data/textures/defaultNormalMap.png"),
            MetalnessTexture = TextureManager.LoadTexture("data/textures/simpleBlack.png"),
            RoughnessTexture = TextureManager.LoadTexture("data/textures/simpleBlack.png"),
            AOTexture = TextureManager.LoadTexture("data/textures/simpleBlack.png"),
            GlowTexture = TextureManager.LoadTexture("data/textures/simpleWhite.png")
        };

        public readonly MoveComponent MoveComponent;
        private readonly RenderComponent renderComponent;
        private readonly HealthComponent healthComponent;
        private readonly SphereCollider collisionComponent;

        private double timeToDie;

        private const int DAMAGE = 20;

        public Projectile() {
            MoveComponent = new MoveComponent(this);
            
            renderComponent = new RenderComponent(
                MODEL,
                MaterialManager.GetMaterial(Material.PBR),
                MATERIAL_SETTINGS,
                this
            );
            
            healthComponent = new HealthComponent(this,1);
            optionalComponents.Add(ComponentType.HEALTH_COMPONENT,new List<Component>{healthComponent});
            
            collisionComponent = new SphereCollider(this, renderComponent.Model,
                   null,
                activeMessage => {
                    if (activeMessage.OtherCollisonComponent.GameObject is SpaceShip) return;
                    if (activeMessage.OtherCollisonComponent.GameObject.searchOptionalComponents(ComponentType.HEALTH_COMPONENT,
                        out var componentList)) {
                        for (int i = 0; i < componentList.Count; i++) {
                            ((HealthComponent)componentList[i]).takeDamage(DAMAGE);
							
                        }
                    }
                    Destroy();
            });
            
        }
        
        public override void Awake() {
            base.Awake();
            RenderEngine.RegisterStaticRenderComponent(renderComponent);
            CollisionEngine.Register(collisionComponent);
            Radius = renderComponent.Model.Radius(TransformComponent.Scale);
            renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
            timeToDie = Time.TotalTime + 10 * 1000;
        }

        public override void Update() {
            base.Update();
            MoveComponent.Update();
            renderComponent.Update();
            if(Time.TotalTime > timeToDie) Destroy();
        }
        

        protected override void OnDestroy() {
            RenderEngine.UnregisterStaticRenderComponent(renderComponent);
            CollisionEngine.Unregister(collisionComponent);
            //TODO disappear with small explosion
        }

        public override string ToString() => "Projectile" + TransformComponent.WorldPosition.ToString();
    }
}