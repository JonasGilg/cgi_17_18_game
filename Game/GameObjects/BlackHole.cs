using System;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Engine.Util;
using Game.Components;
using Game.GamePlay;
using OpenTK;

namespace Game.GameObjects {
    public class BlackHole : GameObject{
        public readonly RenderComponent RenderComponent;
        public readonly MoveComponent MoveComponent;
        public readonly CollisionComponent CollisionComponent;
        public readonly BlackHoleInfluenceZone influenceZone = null;

        public BlackHole() {
            RenderComponent = new RenderComponent(
                ModelLoaderObject3D.Load("data/objects/Planet.obj"),
                MaterialManager.GetMaterial(Material.BLACK_HOLE),
                new MaterialSettings {
                    ColorTexture = TextureManager.LoadTexture("data/textures/black.png")
                },
                this
            );
            
            
            MoveComponent = new MoveComponent(this);

            CollisionComponent = new SphereCollider(this, RenderComponent.Model,
                passive => {
                    IO.PrintAsync("Blackhole got hit");
                    

                },
                active => {
                    TransformComponent.Scale *= new Vector3d(1.1);
                    Radius = RenderComponent.Model.Radius(TransformComponent.Scale);
                    GamePlayEngine.RemoveObjectFromWorld(active.OtherCollisonComponent.GameObject);

                   
                },
                null);
            
        }

        public override void Awake() {
            base.Awake();
            RenderEngine.RegisterStaticRenderComponent(RenderComponent);
            CollisionEngine.Register(CollisionComponent);
            
            Radius = RenderComponent.Model.Radius(TransformComponent.Scale);
            RenderComponent.AABB = RenderComponent.AABB * TransformComponent.Scale;
        }
        
        protected override void OnDestroy() {
            RenderEngine.UnregisterStaticRenderComponent(RenderComponent);
            CollisionEngine.Unregister(CollisionComponent);
        }

        public override void Update() {
            MoveComponent.Update();
            base.Update();
            RenderComponent.Update();
        }
    }
}