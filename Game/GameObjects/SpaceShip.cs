using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Texture;
using Engine.Component;
using Engine.GUI;
using Game.Components;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.GameObjects {
	public class SpaceShip : GameObject {
		private readonly MoveComponent _moveComponent;
		private readonly CameraComponent _cameraComponent;
		private readonly RenderComponent _renderComponent;
		private readonly MoveInputComponent _moveInputComponent;
		public readonly SphereCollider CollisionComponent;

		private readonly HUDElement _speed;
		private readonly HUDElement _position;


		public SpaceShip() {
			_speed = HUD.CreateHUDElement("", new Vector2(-1f, -0.94f));
			_position = HUD.CreateHUDElement("", new Vector2(-1f, -0.88f));
			HUD.AddHUDElement(_speed);
			HUD.AddHUDElement(_position);

			_moveComponent = new MoveComponent(this);
			_cameraComponent = new ThirdPersonCameraComponent(new Vector3d(-0.3, 0.05, 0.0), this);
			_renderComponent = new NormalMapRenderComponent(
				ModelLoaderObject3D.Load("data/objects/SpaceShip.obj", this),
				TextureManager.LoadTexture("data/textures/SpaceShip.png"),
				TextureManager.LoadTexture("data/textures/NormalMap.png"),
				this
			);

			CollisionComponent = new SphereCollider(this, _renderComponent.Model,
				collision => { System.Console.WriteLine(ToString() + " collided with " + collision.gameObject.ToString()); });
			CollisionComponent.Register();

			DisplayCamera.SetActiveCamera(_cameraComponent);

			_moveInputComponent = new ArcadeMoveInputComponent(this, TransformComponent, _moveComponent);
		}

		public override void Awake() {
			base.Awake();

			Radius = _renderComponent.Model.GetRadius();
		}

		public override void Update() {
			_moveInputComponent.Update();
			_moveComponent.Update();
			base.Update();
			_renderComponent.Update();
			_cameraComponent.Update();

			if (Keyboard.Released(Key.Keypad1)) {
				_renderComponent.Material = MaterialManager.GetMaterial(Material.NormalMapping);
			}

			if (Keyboard.Released(Key.Keypad2)) {
				_renderComponent.Material = MaterialManager.GetMaterial(Material.AmbientDiffuseSpecular);
			}

			_position.Text =
				$"POSITION: {TransformComponent.WorldPosition.X:N0}, {TransformComponent.WorldPosition.Y:N0}, {TransformComponent.WorldPosition.Z:N0}";
			_speed.Text = $"   SPEED: {_moveComponent.LinearVelocity.LengthFast:N2}M/S";
		}

		public override void Draw() {
			base.Draw();
			_renderComponent.Draw(16f);
		}
	}
}