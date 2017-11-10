using Engine.Material;
using Engine.Model;

namespace Engine.Component {
	public class NormalMapRenderComponent : RenderComponent {
		private readonly int _normalMap;

		public NormalMapRenderComponent(Model3D model, BaseMaterial material, int texture, int normalMap, GameObject gameObject) : base(
			model, material, texture, gameObject) {
			_normalMap = normalMap;
		}

		public override void Draw(float shininess = 0) {
			Material.Draw(Model, Texture, shininess, _normalMap);
		}
	}
}