using Engine;
using Engine.Component;
using Engine.Model;

namespace Game.GameObjects {
    public abstract class PowerUp : Item {
        public static readonly Model3D Model = ModelLoaderObject3D.Load("data/objects/SimpleCube.obj");

    }
}