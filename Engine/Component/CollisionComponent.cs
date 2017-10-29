using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Component {
	public abstract class CollisionComponent : Component {
		public CollisionComponent(GameObject gameObject) : base(gameObject) {
			Console.WriteLine("Hello Micha");
			//Kollision wird hard...
		}

		public abstract bool IsColliding();
	}
}
