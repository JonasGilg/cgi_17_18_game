using Engine;
using Engine.Material;
using Engine.Model;

namespace Game.GameObjects {
	public class Planet : GameObject {
		public readonly RenderComponent RenderComponent;

		public Planet(int textureId) {
			RenderComponent = new RenderComponent(
				ModelLoaderObject3D.load("data/objects/Planet.obj"),
				MaterialManager.GetMaterial(Material.Simple),
				textureId,
				this
			);
		}

		public override void Update() {
			base.Update();
			RenderComponent.Update();
		}

		public override void Draw() {
			base.Draw();
			RenderComponent.Draw();
		}
	}
}