using System.Collections.Generic;
using Engine;
using Engine.Material;
using Engine.Model;
using Game.Utils;

namespace Game.GameObjects {
	public class Planet : GameObject {
		public readonly RenderComponent RenderComponent;
		public readonly MoveComponent MoveComponent;

		private List<Asteroid> belt = new List<Asteroid>();

		public Planet(int textureId) {
			RenderComponent = new RenderComponent(
				ModelLoaderObject3D.Load("data/objects/Planet.obj"),
				MaterialManager.GetMaterial(Material.Simple),
				textureId,
				this
			);
			MoveComponent = new MoveComponent(this);
		}

		public void generateAsteroidBelt(int number, double distance) {
			for (int i = 0; i < number; i++) {
				var asteroid = AsteroidFactory.GenerateAsteroid();
			}
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