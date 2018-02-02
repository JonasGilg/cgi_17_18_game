using System.Collections.Generic;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.GUI;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Engine.Util;
using Game.Components;
using OpenTK;

namespace Game.GameObjects {
	public class SpaceShip : GameObject {
		public readonly MoveComponent MoveComponent;
		private readonly CameraComponent cameraComponent;
		private readonly RenderComponent renderComponent;
		private readonly ShadowComponent shadowComponent;
		private readonly MoveInputComponent moveInputComponent;
		private readonly SphereCollider collisionComponent;
		private readonly FiringComponent firingComponent;
		private readonly HealthComponent healthComponent;

		private readonly HudTextElement speed;
		private readonly HudTextElement position;
		private readonly HudTextElement healthPoints;


		private const int PASSIVE_SHIP_DAMAGE = 10;
		private const int SPACE_SHIP_HP = 3;

		public SpaceShip() {
			speed = HUD.CreateHudTextElement("", new Vector2(0, -0.9f),TextAnchor.CENTER);
			position = HUD.CreateHudTextElement("", new Vector2(-0.95f, -0.88f), TextAnchor.TOP_LEFT,0.75f);
			healthPoints = HUD.CreateHudTextElement("", new Vector2(0, -0.8f),TextAnchor.CENTER);
			HUD.AddHudTextElement(speed);
			HUD.AddHudTextElement(position);
			HUD.AddHudTextElement(healthPoints);

			MoveComponent = new MoveComponent(this);
			optionalComponents.Add(ComponentType.MOVE_COMPONENT, new List<Component> {MoveComponent});
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
			moveInputComponent = new RLSpaceMovementComponent(this, TransformComponent, MoveComponent);
			
			cameraComponent = new ThirdPersonSpringCameraComponent(MoveComponent, new Vector3d(-15, 1, 0), this);
			DisplayCamera.SetActiveCamera(cameraComponent);

			firingComponent = new FiringComponent(this);

			healthComponent = new HealthComponent(this, SPACE_SHIP_HP, true);
			optionalComponents.Add(ComponentType.HEALTH_COMPONENT, new List<Component> {healthComponent});


			collisionComponent = new SphereCollider(this, renderComponent.Model,
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
					//IO.PrintAsync("Spaceship ACTIVE triggered");
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
			MoveComponent.Update();
			base.Update();
			renderComponent.Update();
			shadowComponent.Update();
			//Console.Out.WriteLine(renderComponent.AABB.Center.ToString());
			cameraComponent.Update();
			firingComponent.Update();
			healthComponent.Update();

			position.Text =
				$"({TransformComponent.WorldPosition.X:N0}| {TransformComponent.WorldPosition.Y:N0}| {TransformComponent.WorldPosition.Z:N0})";
			speed.Text = $"{MoveComponent.LinearVelocity.LengthFast:N2}M/S";
			healthPoints.Text = healthComponent.healthPointStatus();
		}

		public override void Awake() {
			base.Awake();
			RenderEngine.RegisterDynamicRenderComponent(renderComponent);
			CollisionEngine.Register(collisionComponent);
			
			Radius = renderComponent.Model.Radius(Vector3d.One);
			renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
		}

		protected override void OnDestroy() {
			//TODO explosion animation here
			RenderEngine.UnregisterDynamicRenderComponent(renderComponent);
			CollisionEngine.Unregister(collisionComponent);
		}
	}
}