using System;
using System.Linq;
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
		private readonly MoveComponent moveComponent;
		private readonly CameraComponent cameraComponent;
		private readonly RenderComponent renderComponent;
		private readonly MoveInputComponent moveInputComponent;
		public readonly SphereCollider CollisionComponent;

		private readonly HUDElement speed;
		private readonly HUDElement position;


		public SpaceShip() {
			speed = HUD.CreateHUDElement("", new Vector2(-1f, -0.94f));
			position = HUD.CreateHUDElement("", new Vector2(-1f, -0.88f));
			HUD.AddHUDElement(speed);
			HUD.AddHUDElement(position);

			moveComponent = new MoveComponent(this);
			cameraComponent = new ThirdPersonCameraComponent(new Vector3d(-0.3, 0.05, 0.0), this);
			renderComponent = new RenderComponent(
				ModelLoaderObject3D.Load("data/objects/SpaceShip.obj", this),
				MaterialManager.GetMaterial(Material.NORMAL_MAPPING),
				new MaterialSettings {
					ColorTexture = TextureManager.LoadTexture("data/textures/SpaceShip.png"),
					NormalTexture = TextureManager.LoadTexture("data/textures/NormalMap.png"),
					Shininess = 16.0
				},
				this
			);
			
			RenderEngine.RegisterRenderComponent(renderComponent);

			CollisionComponent = new SphereCollider(this, renderComponent.Model,
				collision => { Console.WriteLine(ToString() + " collided with " + collision.GameObject.ToString()); });
			CollisionComponent.Register();

			DisplayCamera.SetActiveCamera(cameraComponent);

			moveInputComponent = new ArcadeMoveInputComponent(this, TransformComponent, moveComponent);
		}

		public override void Update() {
			moveInputComponent.Update();
			moveComponent.Update();
			base.Update();
			renderComponent.Update();
			//Console.Out.WriteLine(renderComponent.AABB.Center.ToString());
			cameraComponent.Update();

			if (Keyboard.Released(Key.Keypad1)) {
				renderComponent.Material = MaterialManager.GetMaterial(Material.NORMAL_MAPPING);
			}

			if (Keyboard.Released(Key.Keypad2)) {
				renderComponent.Material = MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR);
			}

			position.Text =
				$"POSITION: {TransformComponent.WorldPosition.X:N0}, {TransformComponent.WorldPosition.Y:N0}, {TransformComponent.WorldPosition.Z:N0}";
			speed.Text = $"   SPEED: {moveComponent.LinearVelocity.LengthFast:N2}M/S";
		}

		public override void Awake() {
			base.Awake();
			Radius = renderComponent.Model.Radius(TransformComponent.Scale);
			renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
		}
	}
}