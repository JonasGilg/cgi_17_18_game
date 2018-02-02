using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Game.GamePlay;
using OpenTK;

namespace Game.GameObjects {
	public class NavigationArrow : GameObject {
		private readonly RenderComponent renderComponent;
		private static readonly Model3D MODEL = ModelLoaderObject3D.Load("data/objects/navigation_arrow_fixed.obj");

		public NavigationArrow() {
			renderComponent = new RenderComponent(
				MODEL,
				MaterialManager.GetMaterial(Material.PBR),
				new MaterialSettings {
					ColorTexture = TextureManager.LoadTexture("data/textures/supplyRing/violett.png"),
					NormalTexture = TextureManager.LoadTexture("data/textures/defaultNormalMap.png"),
					MetalnessTexture = TextureManager.LoadTexture("data/textures/simpleBlack.png"),
					RoughnessTexture = TextureManager.LoadTexture("data/textures/simpleBlack.png"),
					AOTexture = TextureManager.LoadTexture("data/textures/simpleWhite.png"),
					GlowTexture = TextureManager.LoadTexture("data/textures/planets/blur.png")
				},
				this
			);
		}

		public override void Awake() {
			base.Awake();
			RenderEngine.RegisterDynamicRenderComponent(renderComponent);
			Radius = renderComponent.Model.Radius(TransformComponent.Scale);
			renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
		}

		protected override void OnDestroy() => RenderEngine.UnregisterDynamicRenderComponent(renderComponent);

		public override void Update() {
			base.Update();
			TransformComponent.Position = GamePlayEngine.PLAYER_SPACESHIP.TransformComponent.Position +
			                              GamePlayEngine.PLAYER_SPACESHIP.TransformComponent.Orientation.Rotate(new Vector3d(0, GamePlayEngine.PLAYER_SPACESHIP.Radius, 0));

			TransformComponent.Orientation = Math3D.LookAt(TransformComponent.Position, GamePlayEngine.CurrentGoal?.TransformComponent.Position ?? Vector3d.Zero);

			renderComponent.Update();
		}
	}
}