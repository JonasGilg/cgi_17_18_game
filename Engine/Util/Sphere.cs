using OpenTK;

namespace Engine.Util {
	public struct Sphere {
		public readonly Vector3d Center;
		public readonly double Radius;
		
		public Sphere(Vector3d center, double radius) {
			Center = center;
			Radius = radius;
		}
	}
}