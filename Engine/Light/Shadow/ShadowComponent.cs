using Engine.Component;
using Engine.Material.Shadow;

namespace Engine {
	public class ShadowComponent : Component.Component {
		public static readonly CastShadowMaterial SHADOW_MATERIAL = new CastShadowMaterial();

		private readonly RenderComponent renderComponent;
		public ShadowComponent(RenderComponent renderComponent, GameObject gameObject) : base(gameObject) => this.renderComponent = renderComponent;

		public override void Update() {
			SHADOW_MATERIAL.RegisterForDraw(renderComponent);
		}
	}
}