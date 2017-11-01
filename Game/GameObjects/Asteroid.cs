using Engine.Component;
using Engine;
using Engine.Material;
using Engine.Model;
using OpenTK;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		public readonly MoveComponent MoveComponent;
		public readonly Model3D Model;

		public Asteroid(Model3D model3D) {
			Model = model3D;
			MoveComponent = new MoveComponent(this);
			TransformComponent.Scale = new Vector3(0.4f);
		}

		public void Draw(AmbientDiffuseSpecularMaterial material, int texture) {
			material.Draw(Model, texture, 0.3f);
		}
	}
}