﻿using System;
using System.Runtime.Remoting.Messaging;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.GUI;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Game.Components;

namespace Game.GameObjects {
	public class Planet : GameObject {
		private static readonly Model3D PLANET_MODEL = ModelLoaderObject3D.Load("data/objects/Planet.obj");

		public readonly RenderComponent RenderComponent;
		public readonly MoveComponent MoveComponent;
		public readonly CollisionComponent CollisionComponent;

		
		

		public Planet(int textureId, GameObject referenceObject = null) {
			RenderComponent = new RenderComponent(
				PLANET_MODEL,
				MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
				new MaterialSettings {
					ColorTexture = textureId,
					Shininess = 0.0
				},
				this
			);

			

			if (referenceObject != null) {
				MoveComponent = new GravityMovement(this, 0.0);
			}
			else {
				MoveComponent = new MoveComponent(this);
			}
			
			CollisionComponent = new SphereCollider(this, RenderComponent.Model,
				message => {
				
					if (message.OtherCollisonComponent.GameObject.searchOptionalComponents(ComponentType.HEALTH_COMPONENT, out var componentList)) {
						for (int i = 0; i < componentList.Count; i++) {
							((HealthComponent) componentList[i]).instaKill();
						}	
					}
				}
				//noActive,
				//noPhysics
			);
			
			
			
			
		}
		
		public void activateMarker() {
			HUD.AddHudObjectMarker(objectMarker);
		}

		public void deactivateMarker() {
			HUD.RemoveHudObjectMarker(objectMarker.ID);
		}

		

		public override void Awake() {
			base.Awake();
			RenderEngine.RegisterRenderComponent(RenderComponent);
			CollisionEngine.Register(CollisionComponent);
			
			Radius = RenderComponent.Model.Radius(TransformComponent.Scale);
			RenderComponent.AABB = RenderComponent.AABB * TransformComponent.Scale;
			TransformComponent.UpdateWorldMatrix();
			
		}

		public override void Update() {
			MoveComponent.Update();
			base.Update();
			RenderComponent.Update();
		}
		
		protected override void OnDestroy() {
			//TODO BIIIG explosion
			RenderEngine.UnregisterRenderComponent(RenderComponent);
			CollisionEngine.Unregister(CollisionComponent);
			HUD.RemoveHudObjectMarker(objectMarker.ID);
		}

		public override string ToString() {
			return TransformComponent.WorldPosition.ToString();
		}
	}
}