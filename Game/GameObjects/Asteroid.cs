using System.Runtime.InteropServices.ComTypes;
using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Texture;
using OpenTK;
using OpenTK.Graphics.ES10;
using OpenTK.Input;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		public readonly MoveComponent MoveComponent;
		public readonly RenderComponent RenderComponent;

		public Asteroid(Model3D model) {
			MoveComponent = new MoveComponent(this);
			RenderComponent = new RenderComponent(
				model,
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
			RenderComponent.Draw(0.1f);
		}
	}
}