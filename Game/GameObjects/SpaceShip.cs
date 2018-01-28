using System;
using Engine;
using Engine.Component;
using Engine.GUI;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
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

		private readonly HudTextElement speed;
		private readonly HudTextElement position;

		//TODO implement invulnerability for collision with planet and asteroids + blinking effect?
		public const int invulnerabilityTime = 2000; //milliseconds

		public double invulnerableTill = Time.TotalTime;
		
		public const int maxHP = 5;

		//private int _hp;
		public int currentHP;/* {
			get => _hp;
			set {
				_hp = value;
				if (_hp <= 0) {
					Destroy();
				}
			}
		}*/
		private readonly HudTextElement healthPoints;

		public SpaceShip() {
			currentHP = maxHP;
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
				MaterialManager.GetMaterial(Material.NORMAL_MAPPING),
				new MaterialSettings {
					ColorTexture = TextureManager.LoadTexture("data/textures/SpaceShip.png"),
					NormalTexture = TextureManager.LoadTexture("data/textures/NormalMap.png"),
					Shininess = 16.0
				},
				this
			);
			
			RenderEngine.RegisterRenderComponent(renderComponent);

			CollisionComponent = new SphereCollider(this, renderComponent.Model, collision => {
				Console.WriteLine(ToString() + " collided with " + collision.otherGameObject.ToString());
				switch (collision.otherGameObject) {
					case Asteroid asteroid:
						if (invulnerableTill < Time.TotalTime) {
							currentHP--;
							invulnerableTill = Time.TotalTime + invulnerabilityTime;
						}
						moveComponent.LinearVelocity *= -1;//* asteroid.CollisionComponent.PhysicsMaterial.Bounciness;
						break;
					case Planet planet:
						if (invulnerableTill < Time.TotalTime) {
							currentHP = 0;
							invulnerableTill = Time.TotalTime + invulnerabilityTime;
						}
						moveComponent.LinearVelocity *= -1;// * planet.CollisionComponent.PhysicsMaterial.Bounciness;
						break;
					case MetalChunk chunk:
						Statistics.IncreaseScore(chunk.points);
						Console.WriteLine(chunk.points + " points collected");
						chunk.Destroy();
						break;
				}
			});
			CollisionComponent.Register();

			DisplayCamera.SetActiveCamera(cameraComponent);

			moveInputComponent = new ArcadeMoveInputComponent(this, TransformComponent, moveComponent);

			firingComponent = new FiringComponent(this);
		}

		public override void Update() {
			moveInputComponent.Update();
			moveComponent.Update();
			base.Update();
			renderComponent.Update();
			//Console.Out.WriteLine(renderComponent.AABB.Center.ToString());
			cameraComponent.Update();
			firingComponent.Update();

			if (Keyboard.Released(Key.Keypad1)) {
				renderComponent.Material = MaterialManager.GetMaterial(Material.NORMAL_MAPPING);
			}

			if (Keyboard.Released(Key.Keypad2)) {
				renderComponent.Material = MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR);
			}

			position.Text =
				$"POSITION: {TransformComponent.WorldPosition.X:N0}, {TransformComponent.WorldPosition.Y:N0}, {TransformComponent.WorldPosition.Z:N0}";
			speed.Text = $"   SPEED: {moveComponent.LinearVelocity.LengthFast:N2}M/S";
			healthPoints.Text = $"HP: {currentHP}/{maxHP}";

		}

		public override void Awake() {
			base.Awake();
			Radius = renderComponent.Model.Radius(Vector3d.One);
			renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
		}
		
		public override void Destroy() {
			base.Destroy();
			RenderEngine.UnregisterRenderComponent(renderComponent);
			CollisionComponent.Unregister();
		}

		public override void OnDestroy() {
			//TODO explosion animation here
		}
	}
}