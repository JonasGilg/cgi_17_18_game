using System.Collections.Generic;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Game.Components;
using OpenTK;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		private const int ASTEROID_HP = 20;
		private const int ASTEROID_DMG = 1;
		private readonly MoveComponent moveComponent;
		private readonly RenderComponent renderComponent;
		private readonly ShadowComponent shadowComponent;
		private readonly SphereCollider collisionComponent;

		public Asteroid(Model3D model, MaterialSettings materialSettings, Vector3d rotationAxis, Vector3d direction, GameObject parent = null, double startAngle = 0.0, double distance = 0.0) {
			if (parent != null) {
				TransformComponent.Parent = parent.TransformComponent;
				moveComponent = new GravityMovement(this, rotationAxis, direction, startAngle, distance);
			}
			else {
				moveComponent = new MoveComponent(this);
			}


			renderComponent = new RenderComponent(
				model,
				MaterialManager.GetMaterial(Material.PBR),
				materialSettings,
				this
			);
			shadowComponent = new ShadowComponent(renderComponent, this);

			var healthComponent = new HealthComponent(this, ASTEROID_HP);
			OptionalComponents.Add(ComponentType.HEALTH_COMPONENT, new List<Component> {healthComponent});

			collisionComponent = new SphereCollider(this, renderComponent.Model,
				message => {
					if (message.OtherCollisonComponent.GameObject.SearchOptionalComponents(ComponentType.HEALTH_COMPONENT, out var healtComponents)) {
						for (var i = 0; i < healtComponents.Count; i++) {
							((HealthComponent) healtComponents[i]).TakeDamage(ASTEROID_DMG);
						}
					}

					if (message.OtherCollisonComponent.GameObject.SearchOptionalComponents(ComponentType.MOVE_COMPONENT, out var moveComponents)) {
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
			);
		}

		public override void Awake() {
			base.Awake();
			RenderEngine.RegisterStaticRenderComponent(renderComponent);
			CollisionEngine.Register(collisionComponent);

			Radius = renderComponent.Model.Radius(TransformComponent.Scale);
			renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
		}

		public override void Update() {
			moveComponent.Update();
			base.Update();
			renderComponent.Update();
			shadowComponent.Update();
		}

		protected override void OnDestroy() {
			RenderEngine.UnregisterStaticRenderComponent(renderComponent);
			CollisionEngine.Unregister(collisionComponent);
		}
	}
}