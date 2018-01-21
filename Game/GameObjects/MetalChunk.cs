using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;

namespace Game.GameObjects {
    public enum MetalType {
        Bronze, Silver, Gold
    }
    
    public class MetalChunk : GameObject {
        private readonly RenderComponent renderComponent;
        public readonly SphereCollider CollisionComponent;
        public int points;

        public MetalChunk(MetalType type) {
            //TODO assign correct physical based rendering settings for each metal type
            switch (type) {
                case MetalType.Bronze:
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
                ModelLoaderObject3D.Load("data/objects/asteroids/asteroid_0.obj"),
                MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
                new MaterialSettings {
                    ColorTexture = TextureManager.LoadTexture("data/textures/asteroids/asteroid_0.png"),
                    Shininess = 1
                },
                this
            );
            RenderEngine.RegisterRenderComponent(renderComponent);
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
    }
}