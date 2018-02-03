using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Render;

namespace Game.GameObjects {
	public abstract class Item : GameObject {
		protected readonly RenderComponent RenderComponent;
		private readonly CollisionComponent collisionComponent;
		public readonly MoveComponent MoveComponent;

		protected Item() {
			RenderComponent = InitRenderer();
			collisionComponent = InitCollider();
			MoveComponent = new MoveComponent(this);
		}

		protected abstract RenderComponent InitRenderer();
		protected abstract CollisionComponent InitCollider();

		public override void Awake() {
			base.Awake();
			RenderEngine.RegisterStaticRenderComponent(RenderComponent);
			CollisionEngine.Register(collisionComponent);
			Radius = RenderComponent.Model.Radius(TransformComponent.Scale);
			RenderComponent.AABB = RenderComponent.AABB * TransformComponent.Scale;
		}

		public override void Update() {
			base.Update();
			MoveComponent.Update();
			RenderComponent.Update();
		}

		protected override void OnDestroy() {
			RenderEngine.UnregisterStaticRenderComponent(RenderComponent);
			CollisionEngine.Unregister(collisionComponent);
		}

		public override string ToString() => GetType() + " " + TransformComponent.WorldPosition.ToString();
	}
}