using Engine.Material;
using Engine.Model;

namespace Engine.Component {
	public class NormalMapRenderComponent : RenderComponent {
		private readonly int normalMap;

		public NormalMapRenderComponent(Model3D model, int texture, int normalMap, GameObject gameObject) : base(
			model, MaterialManager.GetMaterial(Engine.Material.Material.NORMAL_MAPPING), texture, gameObject) {
			this.normalMap = normalMap;
		}

		public override void Draw(float shininess = 0) {
			Material.Draw(Model, Texture, shininess, normalMap);
		}
	}
}