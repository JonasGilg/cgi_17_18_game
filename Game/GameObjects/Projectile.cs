using System;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Engine.Util;

namespace Game.GameObjects {
    public class Projectile : GameObject{
        private static readonly Model3D MODEL = ModelLoaderObject3D.Load("data/objects/Planet.obj");
        private static readonly MaterialSettings MATERIAL_SETTINGS = new MaterialSettings {
            ColorTexture = TextureManager.LoadTexture("data/textures/powerfulred.png"),
            Shininess = 1
        };

        public readonly MoveComponent MoveComponent;
        private readonly RenderComponent renderComponent;
        public readonly SphereCollider CollisionComponent;


        public Projectile() {
            MoveComponent = new MoveComponent(this);
            
            //TODO modify renderComponent
            renderComponent = new RenderComponent(
                MODEL,
                MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
                MATERIAL_SETTINGS,
                this
            );
            RenderEngine.RegisterRenderComponent(renderComponent);
            
            CollisionComponent = new SphereCollider(this, renderComponent.Model, collision => {
                IO.PrintAsync("Projectile hit " + collision.otherGameObject.ToString());
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
                World.RemoveFromWorld(this);
            });
            CollisionEngine.Register(CollisionComponent);
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
        

        protected override void OnDestroy() {
            RenderEngine.UnregisterRenderComponent(renderComponent);
            CollisionEngine.Unregister(CollisionComponent);
            //TODO disappear with small explosion
        }

        public override string ToString() => "Projectile" + TransformComponent.WorldPosition.ToString();
    }
}