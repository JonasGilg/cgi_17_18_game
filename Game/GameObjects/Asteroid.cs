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
		private static int ASTEROID_DMG = 25;
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
					//damage
					if (message.OtherCollisonComponent.GameObject.searchOptionalComponents(ComponentType.HEALTH_COMPONENT, out var healtComponents)) {
						for (int i = 0; i < healtComponents.Count; i++) {
								((HealthComponent) healtComponents[i]).takeDamage(ASTEROID_DMG);
						}	
					}
					//bouncing!
					if (message.OtherCollisonComponent.GameObject.searchOptionalComponents(ComponentType.MOVE_COMPONENT, out var moveComponents)) {
						for (int i = 0; i < moveComponents.Count; i++) {
						
							var normalVector = message.OtherCollisonComponent.GameObject.TransformComponent.WorldPosition - TransformComponent.WorldPosition;
							var moveVector = ((MoveComponent) moveComponents[i]).LinearVelocity;
							var reflexionVektor = Math3D.ReflectionVector3D(moveVector, normalVector);
							((MoveComponent) moveComponents[i]).LinearVelocity  = reflexionVektor*moveVector.Length;
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