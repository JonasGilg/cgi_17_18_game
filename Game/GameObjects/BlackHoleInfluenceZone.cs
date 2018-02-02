using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Model;
using Engine.Render;
using Engine.Util;
using OpenTK;

namespace Game.GameObjects {
    public class BlackHoleInfluenceZone : GameObject{
        public readonly SphereCollider CollisionComponent;
        public readonly Model3D model = ModelLoaderObject3D.Load("data/objects/Planet.obj");
        public readonly BlackHole blackHole;

        public BlackHoleInfluenceZone(BlackHole blackhole) {
            blackHole = blackhole;
            
            CollisionComponent = new SphereCollider(this, model,
                passive => {
                    if (passive.OtherCollisonComponent.GameObject is SpaceShip ship) {
                        ship.TransformComponent.Position  = Vector3d.Lerp(ship.TransformComponent.Position, this.TransformComponent.Position, Time.DeltaTimeUpdate * 0.2);
                    }
                }
            );
        }

        public override void Awake() {
            base.Awake();
            TransformComponent.Position = blackHole.TransformComponent.Position;
            TransformComponent.Scale = blackHole.TransformComponent.Scale * 3;
            Radius = model.Radius(TransformComponent.Scale);
            CollisionEngine.Register(CollisionComponent);
        }
        
        protected override void OnDestroy() {
            CollisionEngine.Unregister(CollisionComponent);
        }
    }
}