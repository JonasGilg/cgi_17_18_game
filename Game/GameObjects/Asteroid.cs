using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Texture;
using OpenTK;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		public readonly MoveComponent MoveComponent;
		public readonly RenderComponent RenderComponent;

		public Asteroid() {
			MoveComponent = new MoveComponent(this);
			RenderComponent = new RenderComponent(
				new ModelLoaderObject3D("data/objects/asteroids/asteroid_0.obj"),
				MaterialManager.GetMaterial(Material.AmbientDiffuseSpecular),
				TextureManager.LoadTexture("data/textures/asteroids/asteroid_0.png"),
				this
			);
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