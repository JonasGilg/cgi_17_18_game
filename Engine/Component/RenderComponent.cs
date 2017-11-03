using Engine.Material;
using Engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	public class RenderComponent : Component {
		public Model3D Model;
		public BaseMaterial Material;
		public int Texture;

		public RenderComponent(Model3D model, BaseMaterial material, int texture, GameObject gameObject) : base(gameObject) {
			Model = model;
			Material = material;
			Texture = texture;
		}

		public override void Update() {
			Model.Update(GameObject.TransformComponent.WorldMatrix);
		}

		public void Draw() {
			Material.Draw(Model, Texture);
		}
	}
}