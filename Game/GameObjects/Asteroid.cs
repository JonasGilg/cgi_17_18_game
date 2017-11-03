using Engine;
using Engine.Material;
using Engine.Model;
using OpenTK;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		public readonly MoveComponent MoveComponent;

		public Asteroid() {
			Model = new ModelLoaderObject3D("data/objects/asteroids/asteroid_0.obj");
			MoveComponent = new MoveComponent(this);
		}

		public void Draw(AmbientDiffuseSpecularMaterial material, int texture) {
			material.Draw(Model, texture, 0.3f);
		}
	}
}