using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Game.Components;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		private static int ASTEROID_HP = 20;
		public readonly MoveComponent MoveComponent;
		private readonly RenderComponent renderComponent;
		public readonly SphereCollider CollisionComponent;
		public readonly HealthComponent HealthComponent;

		

		public Asteroid(Model3D model, int textureId, GameObject referenceObject = null) {
			MoveComponent = referenceObject != null ? new GravityMovement(this, 0.0) : new MoveComponent(this);
			
			renderComponent = new RenderComponent(
				model,
				MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
				new MaterialSettings {
					ColorTexture = textureId,
					Shininess = 1
				},
				this
			);
			HealthComponent = new HealthComponent(this,ASTEROID_HP);
			optionalComponents.Add(ComponentType.HEALTH_COMPONENT,new List<Component>{HealthComponent});
			
			CollisionComponent = new SphereCollider(this, renderComponent.Model,
				message => {
				
					if (message.OtherCollisonComponent.GameObject.searchOptionalComponents(ComponentType.HEALTH_COMPONENT, out var componentList)) {
						for (int i = 0; i < componentList.Count; i++) {
								((HealthComponent) componentList[i]).takeDamage(25);
						}	
					}
				}
				//noActive,
				//noPhysics
					);
			
		}

		public override void Awake() {
			base.Awake();
			RenderEngine.RegisterRenderComponent(renderComponent);
			CollisionEngine.Register(CollisionComponent);
			
			Radius = renderComponent.Model.Radius(TransformComponent.Scale);
			renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
		}

		public override void Update() {
			MoveComponent.Update();
			base.Update();
			renderComponent.Update();
		}

		protected override void OnDestroy() {
			RenderEngine.UnregisterRenderComponent(renderComponent);
			CollisionEngine.Unregister(CollisionComponent);
		}
	}
}