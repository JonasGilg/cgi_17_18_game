using OpenTK;

namespace Engine.Util {
	public static class Math3D {
		public static Vector3 Rotate(Vector3 v, Quaternion r) {
			var u = new Vector3(r.Xyz);
			var s = r.W;

			return 2.0f * Vector3.Dot(v, u) * u + (s * s - Vector3.Dot(u, u)) * v + 2.0f * s * Vector3.Cross(u, v);
		}

		public static void Rotate(ref Vector3 v, Quaternion r) {
			var u = new Vector3(r.Xyz);
			var s = r.W;

			v = 2.0f * Vector3.Dot(v, u) * u + (s * s - Vector3.Dot(u, u)) * v + 2.0f * s * Vector3.Cross(u, v);
		}
	}
}