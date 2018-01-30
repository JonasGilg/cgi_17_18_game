using System;
using System.Collections.Generic;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Engine.Util;
using Game.Components;

namespace Game.GameObjects {
    public class Projectile : GameObject{
        private static readonly Model3D MODEL = ModelLoaderObject3D.Load("data/objects/Planet.obj");
        private static readonly MaterialSettings MATERIAL_SETTINGS = new MaterialSettings {
            ColorTexture = TextureManager.LoadTexture("data/textures/powerfulred.png"),
            Shininess = 1
        };

        public readonly MoveComponent MoveComponent;
        private readonly RenderComponent renderComponent;
        public readonly HealthComponent HealthComponent;
        public readonly SphereCollider CollisionComponent;

        public int DAMAGE = 20;
        public Projectile() {
            MoveComponent = new MoveComponent(this);
            
            renderComponent = new RenderComponent(
                MODEL,
                MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
                MATERIAL_SETTINGS,
                this
            );
            
            HealthComponent = new HealthComponent(this,1);
            optionalComponents.Add(ComponentType.HEALTH_COMPONENT,new List<Component>{HealthComponent});
            
            CollisionComponent = new SphereCollider(this, renderComponent.Model,
                null,
                activeMessage => {
                    if (activeMessage.OtherCollisonComponent.GameObject.searchOptionalComponents(ComponentType.HEALTH_COMPONENT,
                        out var componentList)) {
                        for (int i = 0; i < componentList.Count; i++) {
                            ((HealthComponent)componentList[i]).takeDamage(DAMAGE);
							
                        }
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
            //TODO disappear with small explosion
        }

        public override string ToString() => "Projectile" + TransformComponent.WorldPosition.ToString();
    }
}