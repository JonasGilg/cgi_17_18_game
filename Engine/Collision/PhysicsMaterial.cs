
namespace Engine.Collision {
	public class PhysicsMaterial {
		private readonly double bounciness;
		public double Bounciness => bounciness;

		private readonly double friction;
		public double Friction => friction;

		public PhysicsMaterial(double bounciness, double friction) {
			this.bounciness = bounciness;
			this.friction = friction;
		}
	}
}