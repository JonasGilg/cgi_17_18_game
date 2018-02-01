using System.Collections.Generic;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.GUI;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Game.Components;
using OpenTK;

namespace Game.GameObjects {
	public class SpaceShip : GameObject {
		public readonly MoveComponent moveComponent;
		private readonly CameraComponent cameraComponent;
		private readonly RenderComponent renderComponent;
		private readonly ShadowComponent shadowComponent;
		private readonly MoveInputComponent moveInputComponent;
		public readonly SphereCollider CollisionComponent;
		private readonly FiringComponent firingComponent;
		public readonly HealthComponent HealthComponent;

		private readonly HudTextElement speed;
		private readonly HudTextElement position;
		private readonly HudTextElement healthPoints;


		private readonly int PASSIVE_SHIP_DAMAGE = 10;

		public SpaceShip() {
			speed = HUD.CreateHudTextElement("", new Vector2(-1f, -0.94f));
			position = HUD.CreateHudTextElement("", new Vector2(-1f, -0.88f));
			healthPoints = HUD.CreateHudTextElement("", new Vector2(-1f, -0.82f));
			HUD.AddHudTextElement(speed);
			HUD.AddHudTextElement(position);
			HUD.AddHudTextElement(healthPoints);

			moveComponent = new MoveComponent(this);
			optionalComponents.Add(ComponentType.MOVE_COMPONENT, new List<Component> {moveComponent});
			renderComponent = new RenderComponent(
				ModelLoaderObject3D.Load("data/objects/SpaceShip.obj"),
				MaterialManager.GetMaterial(Material.PBR),
				new MaterialSettings {
					ColorTexture = TextureManager.LoadTexture("data/textures/SpaceShip/color.png"),
					NormalTexture = TextureManager.LoadTexture("data/textures/SpaceShip/normal.png"),
					MetalnessTexture = TextureManager.LoadTexture("data/textures/SpaceShip/metalness.png"),
					RoughnessTexture = TextureManager.LoadTexture("data/textures/SpaceShip/roughness.png"),
					AOTexture = TextureManager.LoadTexture("data/textures/SpaceShip/ao.png"),
					GlowTexture = TextureManager.LoadTexture("data/textures/SpaceShip/blur.png")
				},
				this
			);
			shadowComponent = new ShadowComponent(renderComponent, this);
			optionalComponents.Add(ComponentType.RENDER_COMPONENT, new List<Component> {renderComponent});
			moveInputComponent = new RLSpaceMovementComponent(this, TransformComponent, moveComponent);
			cameraComponent = new ThirdPersonSpringCameraComponent(moveComponent, new Vector3d(-15, 1, 0), this);

			firingComponent = new FiringComponent(this);

			HealthComponent = new HealthComponent(this, 2000, true);
			optionalComponents.Add(ComponentType.HEALTH_COMPONENT, new List<Component> {HealthComponent});


			CollisionComponent = new SphereCollider(this, renderComponent.Model,
				passiveMessage => {
					//IO.PrintAsync("PASSIVE: "+ToString() + " <-- " + passiveMessage.OtherCollisonComponent.GameObject.ToString());
					if (passiveMessage.OtherCollisonComponent.GameObject is Projectile) return;
					if (passiveMessage.OtherCollisonComponent.GameObject.searchOptionalComponents(ComponentType.HEALTH_COMPONENT,
						out var componentList)) {
						for (int i = 0; i < componentList.Count; i++) {
							((HealthComponent) componentList[i]).takeDamage(PASSIVE_SHIP_DAMAGE);
						}
					}
				},
				activeMessage => {
					if (activeMessage.OtherCollisonComponent.GameObject is Projectile) return;
					//IO.PrintAsync("ACTIVE: " + ToString() + " --> " + activeMessage.OtherCollisonComponent.GameObject.ToString());
					if (activeMessage.OtherCollisonComponent.GameObject.searchOptionalComponents(ComponentType.HEALTH_COMPONENT,
						out var componentList)) {
						for (int i = 0; i < componentList.Count; i++) {
							((HealthComponent) componentList[i]).takeDamage(PASSIVE_SHIP_DAMAGE);
						}
					}

					//bouncing
				}
			);
		}

		public override void Update() {
			moveInputComponent.Update();
			moveComponent.Update();
			base.Update();
			renderComponent.Update();
			shadowComponent.Update();
			//Console.Out.WriteLine(renderComponent.AABB.Center.ToString());
			cameraComponent.Update();
			firingComponent.Update();
			HealthComponent.Update();

			position.Text =
				$"POSITION: {TransformComponent.WorldPosition.X:N0}, {TransformComponent.WorldPosition.Y:N0}, {TransformComponent.WorldPosition.Z:N0}";
			speed.Text = $"   SPEED: {moveComponent.LinearVelocity.LengthFast:N2}M/S";
			healthPoints.Text = HealthComponent.healthPointStatus();
		}

		public override void Awake() {
			base.Awake();

			RenderEngine.RegisterRenderComponent(renderComponent);
			CollisionEngine.Register(CollisionComponent);
			DisplayCamera.SetActiveCamera(cameraComponent);

			Radius = renderComponent.Model.Radius(Vector3d.One);
			renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
		}

		protected override void OnDestroy() {
			//TODO explosion animation here
			RenderEngine.UnregisterRenderComponent(renderComponent);
			CollisionEngine.Unregister(CollisionComponent);
		}
	}
}