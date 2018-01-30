using System;
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
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.GameObjects {
	public class SpaceShip : GameObject {
		public readonly MoveComponent moveComponent;
		private readonly CameraComponent cameraComponent;
		private readonly RenderComponent renderComponent;
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
			cameraComponent = new ThirdPersonCameraComponent(new Vector3d(-0.3, 0.05, 0.0), this);
			renderComponent = new RenderComponent(
				ModelLoaderObject3D.Load("data/objects/SpaceShip.obj"),
				MaterialManager.GetMaterial(Material.PBR),
				new MaterialSettings {
					ColorTexture = TextureManager.LoadTexture("data/textures/SpaceShip/SpaceShip.png"),
					NormalTexture = TextureManager.LoadTexture("data/textures/SpaceShip/NormalMap.png"),
					MetalnessTexture = TextureManager.LoadTexture("data/textures/SpaceShip/Metalness.png"),
					RoughnessTexture = TextureManager.LoadTexture("data/textures/SpaceShip/Blur.png"),
					AOTexture = TextureManager.LoadTexture("data/textures/SpaceShip/AmbientOcclusion.png"),
					GlowTexture = TextureManager.LoadTexture("data/textures/SpaceShip/Blur.png"),
					Shininess = 16.0
				},
				this
			);
			optionalComponents.Add(ComponentType.RENDER_COMPONENT, new List<Component>{renderComponent});
			moveInputComponent = new ArcadeMoveInputComponent(this, TransformComponent, moveComponent);

			firingComponent = new FiringComponent(this);
			
			HealthComponent = new HealthComponent(this,2000,true);
			optionalComponents.Add(ComponentType.HEALTH_COMPONENT,new List<Component>{HealthComponent});
			

			CollisionComponent = new SphereCollider(this, renderComponent.Model, 
				passiveMessage => {
					IO.PrintAsync("PASSIVE: "+ToString() + " <-- " + passiveMessage.OtherCollisonComponent.GameObject.ToString());
					if (passiveMessage.OtherCollisonComponent.GameObject.searchOptionalComponents(ComponentType.HEALTH_COMPONENT,
						out var componentList)) {
						for (int i = 0; i < componentList.Count; i++) {
							((HealthComponent)componentList[i]).takeDamage(PASSIVE_SHIP_DAMAGE);
							
						}
					}
				},
				activeMessage => {
					IO.PrintAsync("ACTIVE: " + ToString() + " --> " + activeMessage.OtherCollisonComponent.GameObject.ToString());
					if (activeMessage.OtherCollisonComponent.GameObject.searchOptionalComponents(ComponentType.HEALTH_COMPONENT,
						out var componentList)) {
						for (int i = 0; i < componentList.Count; i++) {
							((HealthComponent)componentList[i]).takeDamage(PASSIVE_SHIP_DAMAGE);
							
						}
					}
				}
				);

			
			
			
		}

		public override void Update() {
			moveInputComponent.Update();
			moveComponent.Update();
			base.Update();
			renderComponent.Update();
			//Console.Out.WriteLine(renderComponent.AABB.Center.ToString());
			cameraComponent.Update();
			firingComponent.Update();
			HealthComponent.Update();

			if (Keyboard.Released(Key.Keypad1)) {
				renderComponent.Material = MaterialManager.GetMaterial(Material.NORMAL_MAPPING);
			}

			if (Keyboard.Released(Key.Keypad2)) {
				renderComponent.Material = MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR);
			}

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