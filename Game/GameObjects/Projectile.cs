using System;
using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;

namespace Game.GameObjects {
    public class Projectile : GameObject{
        public readonly MoveComponent MoveComponent;
        private readonly RenderComponent renderComponent;
        public readonly SphereCollider CollisionComponent;

        public Projectile() {
            MoveComponent = new MoveComponent(this);
            
            //TODO modify renderComponent
            renderComponent = new RenderComponent(
                ModelLoaderObject3D.Load("data/objects/asteroids/asteroid_0.obj"),
                MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
                new MaterialSettings {
                    ColorTexture = TextureManager.LoadTexture("data/textures/asteroids/asteroid_0.png"),
                    Shininess = 1
                },
                this
            );
            RenderEngine.RegisterRenderComponent(renderComponent);
            
            CollisionComponent = new SphereCollider(this, renderComponent.Model, collision => {
                Console.WriteLine("Projectile hit " + collision.otherGameObject.ToString());
                switch (collision.otherGameObject) {
                    case Asteroid asteroid:
                        asteroid.hp--;
                        break;
                    case Planet planet:
                        planet.hp--;
                        break;
                    case SpaceShip ship:
                        return;
                    case Projectile proj:
                        return;
                }
                Destroy();
            });
            CollisionComponent.Register();
        }
        
        public override void Awake() {
            base.Awake();
            Radius = renderComponent.Model.Radius(TransformComponent.Scale);
            renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
        }

        public override void Update() {
            base.Update();
            MoveComponent.Update();
            renderComponent.Update();
        }
        
        public override void Destroy() {
            base.Destroy();
            RenderEngine.UnregisterRenderComponent(renderComponent);
            CollisionComponent.Unregister();
        }

        public override void OnDestroy() {
            //TODO disappear with small explosion
        }

        public override string ToString() => "Projectile" + TransformComponent.WorldPosition.ToString();
    }
}