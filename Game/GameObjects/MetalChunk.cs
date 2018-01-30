using System;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Engine.Util;
using Game.GamePlay;
using OpenTK;

namespace Game.GameObjects {
    
    public enum MetalType {
        Charcoal, Copper, Silver, Gold
    }
    
    public class MetalChunk : GameObject {
        private static readonly Model3D Model = ModelLoaderObject3D.Load("data/objects/SupplyRing.obj");
        private static readonly int colorTexture = TextureManager.LoadTexture("data/textures/supplyRing/gold.png");
        private readonly RenderComponent renderComponent;
        public readonly SphereCollider CollisionComponent;
        public readonly MoveComponent MoveComponent;
        public int points;

        public MetalChunk(MetalType type) {
            //TODO assign correct physical based rendering settings for each metal type
            switch (type) {
                case MetalType.Charcoal:
                    points = -5;
                    break;
                case MetalType.Copper:
                    points = 1;
                    break;
                case MetalType.Silver:
                    points = 5;
                    break;
                case MetalType.Gold:
                    points = 20;
                    break;
            }
            //TODO modify renderComponent according to type
            renderComponent = new RenderComponent(
                Model,
                MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
                new MaterialSettings {
                    ColorTexture = colorTexture,
                    Shininess = 1.5/*,
                    MetalnessTexture = TextureManager.LoadTexture("data/textures/pbr/" + type.ToString().ToLower() + "-metalness.png"),
                    RoughnessTexture = TextureManager.LoadTexture("data/textures/pbr/" + type.ToString().ToLower() + "-roughness.png")*/
                },
                this
            );
            
            MoveComponent = new MoveComponent(this);
            
            
            
            
            CollisionComponent = new SphereCollider(this, renderComponent.Model, passiveMessage => {
                if (GamePlayEngine.playerSpaceship.Equals(passiveMessage.OtherCollisonComponent.GameObject)) {
                    GamePlayEngine.supplyRingCollected(this);
                }
            });
            
        }
        
        public override void Awake() {
            base.Awake();
            RenderEngine.RegisterRenderComponent(renderComponent);
            CollisionEngine.Register(CollisionComponent);
            Radius = renderComponent.Model.Radius(TransformComponent.Scale);
            renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
        }

        public override void Update() {
            base.Update();
            MoveComponent.Update();
            renderComponent.Update();
        }
        
        protected override void OnDestroy() {
            //TODO chunk should disappear with a small effect (e.g light)
            RenderEngine.UnregisterRenderComponent(renderComponent);
            CollisionEngine.Unregister(CollisionComponent);
        }

        public override string ToString() => TransformComponent.WorldPosition.ToString();
        
    }
}