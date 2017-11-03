using Engine.Material;
using Engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	public class RenderComponent : Component{
		public BaseMaterial Material;

		public RenderComponent(GameObject gameObject) : base(gameObject) { }


		public override void Update() {

		}

		public void Draw() {
			Material.Draw();
		}
	}

	
}
