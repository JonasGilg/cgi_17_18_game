using Engine.Component;
using Engine;
using Engine.Material.Ambientdiffuse;
using Engine.Model;
using Engine.Util;
using OpenTK;
using OpenTK.Input;

namespace Game.GameObjects {
	public class SpaceShip : GameObject {
		public MoveComponent MoveComponent;
		public Model3D Model;
		
		public SpaceShip(Model3D model) {
			Model = model;
			MoveComponent = new MoveComponent(this);
			Scale = new Vector3(0.05f);
		}

		public void Update(double deltaTime, KeyboardDevice keyboard) {
			if (keyboard[Key.W]) {
				MoveComponent.LinearVelocity.Z += 0.1f;
			}

			if (keyboard[Key.S]) {
				MoveComponent.LinearVelocity.Z -= 0.1f;
			}

			if (keyboard[Key.Q]) {
				MoveComponent.AngularVelocity.Y += 0.1f;
			}
			
			if (keyboard[Key.E]) {
				MoveComponent.AngularVelocity.Y -= 0.1f;
			}
			
			MoveComponent.Update(deltaTime);
			Update();
			Model.Update(Transform);
		}
		
		public void Draw(AmbientDiffuseMaterial material, int texture) {
			material.Draw(Model, texture);
		}
	}
}