using Engine.Model;

namespace Game.GameObjects {
	public abstract class PowerUp : Item {
		protected static readonly Model3D MODEL = ModelLoaderObject3D.Load("data/objects/SimpleCube.obj");
	}
}