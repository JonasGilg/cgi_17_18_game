using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Engine.Util;
using Game.GamePlay;
using Game.Utils;
using OpenTK;

namespace Game.GameObjects {
    public class FinishMarker : GameObject{
        private static readonly Model3D MODEL = ModelLoaderObject3D.Load("data/objects/Planet.obj");
        private static readonly MaterialSettings MATERIAL_SETTINGS = new MaterialSettings {
            ColorTexture = TextureManager.LoadTexture("data/textures/finish.jpg"),
            Shininess = 1,
        };
        
        public readonly MoveComponent MoveComponent;
        private readonly RenderComponent renderComponent;
        public readonly SphereCollider CollisionComponent;

        public FinishMarker() {
            MoveComponent = new MoveComponent(this) {
                AngularVelocity = new Vector3d(0,0.75,0)
            };
            
            renderComponent = new RenderComponent(
                MODEL,
                MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
                MATERIAL_SETTINGS,
                this
            );
            
            
            CollisionComponent = new SphereCollider(this, renderComponent.Model, collision => {
                if (collision.otherGameObject is SpaceShip) {
                    IO.PrintAsync("Level " + GamePlayEngine.CurrentLevelIndex + " completed!");
                    GamePlayEngine.LoadNextLevel();
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
            RenderEngine.UnregisterRenderComponent(renderComponent);
            CollisionEngine.Unregister(CollisionComponent);
        }
    }
}