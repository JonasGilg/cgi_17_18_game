using Engine;
using Engine.Material;
using Engine.Model;
using OpenTK;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		public readonly MoveComponent MoveComponent;

		public Asteroid() {
			MoveComponent = new MoveComponent(this);
		}
	}
}