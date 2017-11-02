using Engine;
using Engine.Material;
using Engine.Model;
using OpenTK;
using OpenTK.Input;

namespace Game.GameObjects {
    public class Asteroid : GameObject {
        public readonly MoveComponent MoveComponent;
        public readonly Model3D Model;

        public Asteroid(Model3D model3D, Vector3 rotation, Vector3 velocity, Vector3 scale) {
            Model = model3D;
            MoveComponent = new MoveComponent(this);
            MoveComponent.LinearVelocity = velocity;
            MoveComponent.AngularVelocity = rotation;
            TransformComponent.Scale = scale;
        }

        public override void Update(double deltaTime) {
            MoveComponent.Update(deltaTime);
            base.Update(deltaTime);
            Model.Update(TransformComponent.WorldMatrix);
        }

        public void Draw(AmbientDiffuseSpecularMaterial material, int texture) {
            material.Draw(Model, texture, 0.3f);
        }
    }
}