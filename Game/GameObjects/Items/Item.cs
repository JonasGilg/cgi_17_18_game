using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Model;
using Engine.Render;
using Engine.Texture;

namespace Game.GameObjects {
    public abstract class Item : GameObject {
        public RenderComponent renderComponent;
        public CollisionComponent collisionComponent;
        public MoveComponent moveComponent;

        public Item() {
            renderComponent = InitRenderer();
            collisionComponent = InitCollider();
            moveComponent = new MoveComponent(this);
        }

        protected abstract RenderComponent InitRenderer();

        protected abstract CollisionComponent InitCollider();

        public override void Awake() {
            base.Awake();
            RenderEngine.RegisterRenderComponent(renderComponent);
            CollisionEngine.Register(collisionComponent);
            Radius = renderComponent.Model.Radius(TransformComponent.Scale);
            renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
        }

        public override void Update() {
            base.Update();
            moveComponent.Update();
            renderComponent.Update();
        }
        
        protected override void OnDestroy() {
            //TODO chunk should disappear with a small effect (e.g light)
            RenderEngine.UnregisterRenderComponent(renderComponent);
            CollisionEngine.Unregister(collisionComponent);
        }

        public override string ToString() => GetType() + " " + TransformComponent.WorldPosition.ToString();
    }
}