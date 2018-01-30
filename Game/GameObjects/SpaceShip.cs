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
		private readonly ShadowComponent shadowComponent;
		private readonly MoveInputComponent moveInputComponent;
		public readonly SphereCollider CollisionComponent;
		private readonly FiringComponent firingComponent;
		public readonly HealthComponent healthComponent;

		private readonly HudTextElement speed;
		private readonly HudTextElement position;

		//TODO implement invulnerability for collision with planet and asteroids + blinking effect?
		public const int invulnerabilityTime = 2000; //milliseconds
		public double invulnerableTill = Time.TotalTime;
		public bool IsInvulnerable => invulnerableTill > Time.TotalTime;
		
		private readonly HudTextElement healthPoints;

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
					RoughnessTexture = TextureManager.LoadTexture("data/textures/SpaceShip/Roughness.png"),
					AOTexture = TextureManager.LoadTexture("data/textures/SpaceShip/AmbientOcclusion.png"),
					GlowTexture = TextureManager.LoadTexture("data/textures/SpaceShip/Blur.png"),
					Shininess = 16.0
				},
				this
			);
			
			shadowComponent = new ShadowComponent(renderComponent, this);

			CollisionComponent = new SphereCollider(this, renderComponent.Model, collision => {
				IO.PrintAsync(ToString() + " collided with " + collision.otherGameObject.ToString());
				switch (collision.otherGameObject) {
					case Asteroid asteroid:
						healthComponent.takeDamage(1);
						moveComponent.LinearVelocity *= -1;//* asteroid.CollisionComponent.PhysicsMaterial.Bounciness;
						break;
					case Planet planet:
						healthComponent.takeDamage(300);
						moveComponent.LinearVelocity *= -1;// * planet.CollisionComponent.PhysicsMaterial.Bounciness;
						break;
					case MetalChunk chunk:
						Statistics.IncreaseScore(chunk.points);
						IO.PrintAsync(chunk.points + " points collected");
						GameObject.Destroy(chunk);
						break;
				}
			});

			moveInputComponent = new ArcadeMoveInputComponent(this, TransformComponent, moveComponent);

			firingComponent = new FiringComponent(this);
			
			healthComponent = new HealthComponent(this);
		}

		private void makeInvulnerable() => invulnerableTill = Time.TotalTime + invulnerabilityTime;

		public override void Update() {
			moveInputComponent.Update();
			moveComponent.Update();
			base.Update();
			renderComponent.Update();
			shadowComponent.Update();
			//Console.Out.WriteLine(renderComponent.AABB.Center.ToString());
			cameraComponent.Update();
			firingComponent.Update();
			healthComponent.Update();

			if (Keyboard.Released(Key.Keypad1)) {
				renderComponent.Material = MaterialManager.GetMaterial(Material.NORMAL_MAPPING);
			}

			if (Keyboard.Released(Key.Keypad2)) {
				renderComponent.Material = MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR);
			}

			position.Text =
				$"POSITION: {TransformComponent.WorldPosition.X:N0}, {TransformComponent.WorldPosition.Y:N0}, {TransformComponent.WorldPosition.Z:N0}";
			speed.Text = $"   SPEED: {moveComponent.LinearVelocity.LengthFast:N2}M/S";
			healthPoints.Text = healthComponent.healthPointStatus();

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