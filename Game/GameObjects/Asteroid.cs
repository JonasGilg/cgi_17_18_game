﻿using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Util;
using Game.Components;
using OpenTK;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		private static int ASTEROID_HP = 20;
		private static int ASTEROID_DMG = 1;
		public readonly MoveComponent MoveComponent;
		private readonly RenderComponent renderComponent;
		private readonly ShadowComponent shadowComponent;
		public readonly SphereCollider CollisionComponent;
		public readonly HealthComponent HealthComponent;


		public Asteroid(Model3D model, MaterialSettings materialSettings, Vector3d rotationAxis, Vector3d direction, GameObject parent = null, double startAngle = 0.0, double distance = 0.0) {
			if (parent != null) {
				TransformComponent.Parent = parent.TransformComponent;
				MoveComponent = new GravityMovement(this, rotationAxis, direction, startAngle, distance);
			}
			else {
				MoveComponent = new MoveComponent(this);
			}


			renderComponent = new RenderComponent(
				model,
				MaterialManager.GetMaterial(Material.PBR),
				materialSettings,
				this
			);
			shadowComponent = new ShadowComponent(renderComponent, this);

			HealthComponent = new HealthComponent(this, ASTEROID_HP);
			optionalComponents.Add(ComponentType.HEALTH_COMPONENT, new List<Component> {HealthComponent});

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
							var reflectionVector = Math3D.ReflectionVector3D(moveVector, normalVector);

							if (double.IsNaN(reflectionVector.X)) reflectionVector.X = 0;
							if (double.IsNaN(reflectionVector.Y)) reflectionVector.Y = 0;
							if (double.IsNaN(reflectionVector.Z)) reflectionVector.Z = 0;
							
							((MoveComponent) moveComponents[i]).LinearVelocity = reflectionVector * moveVector.Length;
						}
					}
				}
				//noActive,
				//noPhysics
			);
		}

		public override void Awake() {
			base.Awake();
			RenderEngine.RegisterStaticRenderComponent(renderComponent);
			CollisionEngine.Register(CollisionComponent);

			Radius = renderComponent.Model.Radius(TransformComponent.Scale);
			renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
		}

		public override void Update() {
			MoveComponent.Update();
			base.Update();
			renderComponent.Update();
			shadowComponent.Update();
		}

		protected override void OnDestroy() {
			RenderEngine.UnregisterStaticRenderComponent(renderComponent);
			CollisionEngine.Unregister(CollisionComponent);
		}
	}
}