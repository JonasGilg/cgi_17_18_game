using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Engine.Util;
using Game.GamePlay;
using OpenTK;

namespace Game.GameObjects {
	public class BlackHole : GameObject {
		private readonly RenderComponent renderComponent;
		private readonly MoveComponent moveComponent;
		private readonly CollisionComponent collisionComponent;
		private readonly BlackHoleInfluenceZone influenceZone;

		public BlackHole() {
			renderComponent = new RenderComponent(
				ModelLoaderObject3D.Load("data/objects/Planet.obj"),
				MaterialManager.GetMaterial(Material.BLACK_HOLE),
				new MaterialSettings {
					ColorTexture = TextureManager.LoadTexture("data/textures/black.png")
				},
				this
			);

			moveComponent = new MoveComponent(this);

			collisionComponent = new SphereCollider(this, renderComponent.Model,
				passive => { IO.PrintAsync("Blackhole got hit"); },
				active => {
					if (active.OtherCollisonComponent.GameObject is BlackHoleInfluenceZone) return;
					TransformComponent.Scale *= new Vector3d(1.1);
					Radius = renderComponent.Model.Radius(TransformComponent.Scale);
					GamePlayEngine.RemoveObjectFromWorld(active.OtherCollisonComponent.GameObject);
				}
			);

			influenceZone = new BlackHoleInfluenceZone(this);
		}

		public override void Awake() {
			base.Awake();
			Radius = renderComponent.Model.Radius(TransformComponent.Scale);
			renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
			RenderEngine.RegisterStaticRenderComponent(renderComponent);
			CollisionEngine.Register(collisionComponent);
			influenceZone.Instantiate();
		}

		protected override void OnDestroy() {
			RenderEngine.UnregisterStaticRenderComponent(renderComponent);
			CollisionEngine.Unregister(collisionComponent);
			influenceZone.Destroy();
		}

		public override void Update() {
			moveComponent.Update();
			base.Update();
			renderComponent.Update();
		}
	}
}