using System;
using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Texture;

namespace Game.GameObjects {
    public class BlackHole : GameObject{
        public readonly RenderComponent RenderComponent;
        public readonly MoveComponent MoveComponent;
        public readonly CollisionComponent CollisionComponent;

        public BlackHole() {
            RenderComponent = new RenderComponent(
                ModelLoaderObject3D.Load("data/objects/Planet.obj", this),
                MaterialManager.GetMaterial(Material.BLACK_HOLE),
                TextureManager.LoadTexture("data/textures/black.png"),
                this
            );
            
            MoveComponent = new MoveComponent(this);
            
            CollisionComponent = new SphereCollider(this, RenderComponent.Model,
                collision => { Console.WriteLine(collision.GameObject.ToString() + " collided with a black hole"); });
            CollisionComponent.Register();
        }
        
        public override void Awake() {
            base.Awake();

            Radius = RenderComponent.Model.GetRadius();
        }

        public override void Update() {
            MoveComponent.Update();
            base.Update();
            RenderComponent.Update();
        }

        public override void Draw() {
            base.Draw();
            RenderComponent.Draw();
        }
    }
}