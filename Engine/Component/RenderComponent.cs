using Engine.Material;
using Engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Component {
	public class RenderComponent : Component {
		public readonly Model3D Model;
		public BaseMaterial Material;
		public readonly int Texture;

		public RenderComponent(Model3D model, BaseMaterial material, int texture, GameObject gameObject) : base(gameObject) {
			Model = model;
			Material = material;
			Texture = texture;
		}

		public override void Update() { Model.Update(); }

		public void Draw(float shininess = 0) {
			Material.Draw(Model, Texture, shininess);
		}
	}
}