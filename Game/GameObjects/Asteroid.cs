using Engine.Component;
using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Util;
using OpenTK;

namespace Game.GameObjects
{
    public class Asteroid: GameObject
    {
        public readonly MoveComponent MoveComponent;
        public readonly Model3D Model;

        public Asteroid(Model3D model3D, Vector3 scaleVector3)
        {
            Model = model3D;
            MoveComponent = new MoveComponent(this);
            Transform

        }
        
        
        
    }
}