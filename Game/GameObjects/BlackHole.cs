using System;
using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;

namespace Game.GameObjects {
    public class BlackHole : GameObject{
        public readonly RenderComponent RenderComponent;
        public readonly MoveComponent MoveComponent;
        public readonly CollisionComponent CollisionComponent;

        public BlackHole() {
            RenderComponent = new RenderComponent(
                ModelLoaderObject3D.Load("data/objects/Planet.obj", this),
                MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
                new MaterialSettings {
                    ColorTexture = TextureManager.LoadTexture("data/textures/black.png")
                },
                this
            );
            RenderEngine.RegisterRenderComponent(RenderComponent);
            
            MoveComponent = new MoveComponent(this);
            
            CollisionComponent = new SphereCollider(this, RenderComponent.Model,
                collision => { Console.WriteLine(collision.GameObject.ToString() + " collided with a black hole"); });
            CollisionComponent.Register();
        }

        public override void Awake() {
            base.Awake();
            Radius = RenderComponent.Model.Radius(TransformComponent.Scale);
            RenderComponent.AABB = RenderComponent.AABB * TransformComponent.Scale;
        }

        public override void Update() {
            MoveComponent.Update();
            base.Update();
            RenderComponent.Update();
        }
    }
}