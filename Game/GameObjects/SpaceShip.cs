using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Texture;
using Engine.Component;
using Engine.GUI;
using Game.Components;
using OpenTK;

namespace Game.GameObjects {
	public class SpaceShip : GameObject {
		private readonly MoveComponent _moveComponent;
		private readonly CameraComponent _cameraComponent;
		private readonly RenderComponent _renderComponent;
		private readonly MoveInputComponent _moveInputComponent;

		private readonly HUDElement _speed;

		public SpaceShip() {
			_speed = HUD.CreateHUDElement("", new Vector2(-1f, -0.94f));
			HUD.AddHUDElement(_speed);
			
			_moveComponent = new MoveComponent(this);
			_cameraComponent = new ThirdPersonCameraComponent(new Vector3d(-0.3, 0.05, 0.0), this);
			_renderComponent = new RenderComponent(
				ModelLoaderObject3D.Load("data/objects/SpaceShip.obj"),
				MaterialManager.GetMaterial(Material.AmbientDiffuseSpecular),
				TextureManager.LoadTexture("data/textures/test.png"),
				this
			);
			DisplayCamera.SetActiveCamera(_cameraComponent);
			
			_moveInputComponent = new ArcadeMoveInputComponent(this, TransformComponent, _moveComponent);
		}

		public override void Update() {
			_moveInputComponent.Update();
			_moveComponent.Update();
			base.Update();
			_renderComponent.Update();
			_cameraComponent.Update();

			_speed.Text = $"SPEED: {_moveComponent.LinearVelocity.LengthFast:N2}M/S";
		}

		public override void Draw() {
			base.Draw();
			_renderComponent.Draw(16.0f);
		}
	}
}