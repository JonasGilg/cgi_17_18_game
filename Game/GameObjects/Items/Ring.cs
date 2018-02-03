using Engine.Model;

namespace Game.GameObjects {
	public abstract class Ring : Item {
		protected static readonly Model3D MODEL = ModelLoaderObject3D.Load("data/objects/SupplyRing.obj");
	}
}