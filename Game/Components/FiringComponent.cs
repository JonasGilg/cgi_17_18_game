using Engine;
using Engine.Component;
using Engine.Render;
using Game.GameObjects;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.Components {
	public class FiringComponent : Component {
		public FiringComponent(GameObject gameObject) : base(gameObject) { }

		public override void Update() {
			if (Keyboard.Pressed(Key.F)) {
				//var shipRadius = GameObject.Radius / GameObject.TransformComponent.Scale.X; // the radius of the ship must be scaled back
				var projectile = new Projectile {
					TransformComponent = {
						Position = GameObject.TransformComponent.Position +
						           GameObject.TransformComponent.Orientation.Rotate(new Vector3d(GameObject.Radius, 0, 0)),
						Orientation = Quaterniond.Identity,
						Scale = new Vector3d(7)
					},
					MoveComponent = {
						LinearVelocity = GameObject.TransformComponent.Orientation.Rotate(Vector3d.UnitX * 2000) +
						                 ((SpaceShip) GameObject).moveComponent.LinearVelocity
					}
				};
				projectile.Instantiate();
			}
		}
	}
}