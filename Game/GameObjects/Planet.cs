using Engine;
using Engine.Material;
using Engine.Model;

namespace Game.GameObjects {
	public class Planet : GameObject {
		public readonly RenderComponent RenderComponent;
		public readonly MoveComponent MoveComponent;

		public Planet(int textureId) {
			RenderComponent = new RenderComponent(
				ModelLoaderObject3D.Load("data/objects/Planet.obj"),
				MaterialManager.GetMaterial(Material.Simple),
				textureId,
				this
			);
			MoveComponent = new MoveComponent(this);
		}

		public override void Update() {
			MoveComponent.Update();
			base.Update();
			RenderComponent.Update();
		}

		public override void Draw() {
			base.Draw();
			RenderComponent.Draw();
		}
	}
}