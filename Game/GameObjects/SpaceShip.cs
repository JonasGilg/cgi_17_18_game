using Engine.Component;
using Engine.GameObject;
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
		}

		public void Update(double deltaTime, KeyboardDevice keyboard) {
			if (keyboard[Key.W]) {
				MoveComponent.LinearVelocity.X += 0.1f;
			}

			if (keyboard[Key.S]) {
				MoveComponent.LinearVelocity.X -= 0.1f;
			}

			if (keyboard[Key.Q]) {
				MoveComponent.AngularVelocity.X += 0.1f;
			}
			
			if (keyboard[Key.E]) {
				MoveComponent.AngularVelocity.X -= 0.1f;
			}
			
			MoveComponent.Update(deltaTime);
			Model.Update(Position, Orientation, new Vector3(0.05f));
		}
	}
}