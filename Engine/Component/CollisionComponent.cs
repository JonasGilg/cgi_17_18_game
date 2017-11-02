using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace Engine {
	public abstract class CollisionComponent : Component {
		public CollisionComponent(GameObject gameObject) : base(gameObject) {
			Console.WriteLine("Hello Micha");
			//Kollision wird hard...
		}

		public override void Update(double deltaTime, KeyboardDevice input) {
			
		}

		public abstract bool IsColliding();
	}
}
