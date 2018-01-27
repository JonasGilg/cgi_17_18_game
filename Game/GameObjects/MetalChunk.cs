using System;
using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using OpenTK;

namespace Game.GameObjects {
    
    public enum MetalType {
        Charcoal, Copper, Silver, Gold
    }
    
    public class MetalChunk : GameObject {
        private static readonly Model3D Model = ModelLoaderObject3D.Load("data/objects/asteroids/asteroid_0.obj");
        private readonly RenderComponent renderComponent;
        public readonly SphereCollider CollisionComponent;
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
                    ColorTexture = TextureManager.LoadTexture("data/textures/asteroids/asteroid_0.png"),
                    Shininess = 1/*,
                    MetalnessTexture = TextureManager.LoadTexture("data/textures/pbr/" + type.ToString().ToLower() + "-metalness.png"),
                    RoughnessTexture = TextureManager.LoadTexture("data/textures/pbr/" + type.ToString().ToLower() + "-roughness.png")*/
                },
                this
            );
            RenderEngine.RegisterRenderComponent(renderComponent);
            
            CollisionComponent = new SphereCollider(this, renderComponent.Model, collision => {
                Console.WriteLine(ToString() + " collided with " + collision.otherGameObject.ToString());
                
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
            renderComponent.Update();
        }
        
        public override void Destroy() {
            base.Destroy();
            RenderEngine.UnregisterRenderComponent(renderComponent);
            CollisionComponent.Unregister();
        }

        public override void OnDestroy() {
            //TODO chunk should disappear with a small effect (e.g light)
        }

        public override string ToString() => TransformComponent.WorldPosition.ToString();
        
    }
}