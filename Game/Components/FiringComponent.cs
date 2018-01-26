using System;
using Engine;
using Engine.Component;
using Engine.Render;
using Game.GameObjects;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.Components {
    public class FiringComponent : Component{
        
        public FiringComponent(SpaceShip gameObject) : base(gameObject) {
            
        }

        public override void Update() {
            if (Keyboard.Pressed(Key.F)) {
                Console.WriteLine("FIRING!");
                Console.WriteLine();
                var projectile = new Projectile {
                    TransformComponent = {
                        Scale = new Vector3d(5),
                        Position = GameObject.TransformComponent.Position,
                        Orientation = Quaterniond.Identity
                    },
                    MoveComponent = {
                        LinearVelocity = GameObject.TransformComponent.Orientation.Rotate(Vector3d.UnitX * 500)
                    }
                };
                World.AddToWorld(projectile);
            }
        }
    }
}