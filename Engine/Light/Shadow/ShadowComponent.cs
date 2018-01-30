using Engine.Component;
using Engine.Material.Shadow;

namespace Engine {
	public class ShadowComponent : Component.Component {
		public static readonly CastShadowMaterial SHADOW_MATERIAL = new CastShadowMaterial();
		
		public readonly RenderComponent RenderComponent;
		public ShadowComponent(RenderComponent renderComponent, GameObject gameObject) : base(gameObject) => RenderComponent = renderComponent;

		public override void Update() {
			SHADOW_MATERIAL.RegisterForDraw(RenderComponent);
		}
	}
}