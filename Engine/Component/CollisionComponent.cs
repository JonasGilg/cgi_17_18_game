using System;
using Engine.Component;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace Engine {
	public delegate void Collisionhandler(Collision col);
	public abstract class CollisionComponent : Component.Component {
		public Collisionhandler onCollision;
		public CollisionComponent(GameObject gameObject, Collisionhandler collisionFunction) : base(gameObject) {
			onCollision = collisionFunction;
			Console.WriteLine("Hello Micha");
			//Kollision wird hard...
		}

		public override void Update() {
			
		}

		public abstract bool IsColliding();
	}
}
