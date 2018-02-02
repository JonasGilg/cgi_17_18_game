using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Model;
using Engine.Render;
using Engine.Util;

namespace Game.GameObjects {
    public class BlackHoleInfluenceZone : GameObject{
        public readonly CollisionComponent CollisionComponent;
        public readonly Model3D model = ModelLoaderObject3D.Load("data/objects/Planet.obj");

        public BlackHoleInfluenceZone() {
            CollisionComponent = new SphereCollider(this, model,
                passive => {
                    IO.PrintAsync("You are in the influence zone of the Blackhole!");
                }
            );
        }

        public override void Awake() {
            base.Awake();
            CollisionEngine.Register(CollisionComponent);
        }
        
        protected override void OnDestroy() {
            CollisionEngine.Unregister(CollisionComponent);
        }
    }
}