using System;
using Engine;
using Engine.Util;
using OpenTK;

namespace Game.Components {
    public class GravityMovement : MoveComponent {
        public readonly GameObject ReferenceObject;
        public readonly double distance = 2000;
        public readonly double speed = 1.0 / 180;
        public double currentAngle;

        public GravityMovement(GameObject gameObject, GameObject referenceObject, double currentAngle) : base(gameObject) {
            ReferenceObject = referenceObject;
            LinearVelocity = new Vector3d(0.0);
            this.currentAngle = currentAngle;
        }

        public override void Update() {
            
            computeGravitationalMovement();
            base.Update();
        }

        private void computeGravitationalMovement() {
            //Console.Out.WriteLine(speed + " | " + currentAngle + " | " +GameObject.TransformComponent.WorldPosition.ToString()+ " | "+ReferenceObject.TransformComponent.WorldPosition);
            
            GameObject.TransformComponent.Position.X = Math.Cos(currentAngle) * distance;
            GameObject.TransformComponent.Position.Z = Math.Sin(currentAngle) * distance;

            currentAngle += speed * Time.DeltaTimeUpdate;
    
            if(currentAngle > 2 * Math.PI)
                currentAngle -= 2 * Math.PI;
        }
        //
    }
    
    
}