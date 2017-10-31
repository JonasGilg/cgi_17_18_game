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
				MoveComponent.LinearVelocity.Z += (float) (1 * deltaTime);
			}
			
			if (keyboard[Key.A]) {
				MoveComponent.LinearVelocity.X += (float) (1 * deltaTime);
			}

			if (keyboard[Key.S]) {
				MoveComponent.LinearVelocity.Z -= (float) (1 * deltaTime);
			}
			
			if (keyboard[Key.D]) {
				MoveComponent.LinearVelocity.X -= (float) (1 * deltaTime);
			}

			if (keyboard[Key.Q]) {
				MoveComponent.AngularVelocity.Y += (float) (1 * deltaTime);
			}
			
			if (keyboard[Key.E]) {
				MoveComponent.AngularVelocity.Y -= (float) (1 * deltaTime);
			}
			
			MoveComponent.Update(deltaTime, keyboard);
			Update();
			Model.Update(Transform);
		}
		
		public void Draw(AmbientDiffuseMaterial material, int texture) {
			material.Draw(Model, texture);
		}
	}
}