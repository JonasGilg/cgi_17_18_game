using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Component;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		public readonly MoveComponent MoveComponent;
		private readonly RenderComponent _renderComponent;
		public readonly SphereCollider CollisionComponent;

		public Asteroid(string modelObjectPath, int textureId, GameObject referenceObject = null) {
			if (referenceObject != null) {
				MoveComponent = new Components.GravityMovement(this, 0.0);
			}
			else {
				MoveComponent = new MoveComponent(this);
			}

			var model = ModelLoaderObject3D.Load(modelObjectPath, this);

			_renderComponent = new RenderComponent(
				model,
				MaterialManager.GetMaterial(Material.AmbientDiffuseSpecular),
				textureId,
				this
			);
			
			CollisionComponent = new SphereCollider(this, _renderComponent.Model,
				collision => { System.Console.WriteLine("Asteroid collided with" + collision.gameObject.ToString()); });
		}

		public override void Awake() {
			base.Awake();
			Radius = _renderComponent.Model.GetRadius();
		}

		public override void Update() {
			MoveComponent.Update();
			base.Update();
			_renderComponent.Update();
		}

		public override void Draw() {
			base.Draw();
			_renderComponent.Draw(0.1f);
		}
	}
}