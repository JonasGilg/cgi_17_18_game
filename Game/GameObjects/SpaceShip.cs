using Engine.Component;
using Engine.GameObject;
using Engine.Material.Ambientdiffuse;
using Engine.Model;
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
				MoveComponent.LinearVelocityWorld.X += 0.1f;
			}

			if (keyboard[Key.S]) {
				MoveComponent.LinearVelocityWorld.X -= 0.1f;
			}

			if (keyboard[Key.Q]) {
				MoveComponent.AngularVelocityWorld.X += 0.1f;
			}
			
			if (keyboard[Key.E]) {
				MoveComponent.AngularVelocityWorld.X -= 0.1f;
			}
			
			MoveComponent.Update(deltaTime);
			Model.Update(GetTransformationMatrix());
		}
		
		public void Draw(AmbientDiffuseMaterial material, int texture) {
			material.Draw(Model, texture);
		}
	}
}