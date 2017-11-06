using OpenTK;

namespace Engine.Util {
	public static class Math3D {
		public static Vector3d Rotate(this Quaterniond quat, Vector3d vec) {
			Vector3d.Transform(ref vec, ref quat, out var result);
			return result;
		}

		public static Matrix4 ToFloat(this Matrix4d matrix) => new Matrix4(
			(float) matrix.M11, (float) matrix.M12, (float) matrix.M13, (float) matrix.M14,
			(float) matrix.M21, (float) matrix.M22, (float) matrix.M23, (float) matrix.M24,
			(float) matrix.M31, (float) matrix.M32, (float) matrix.M33, (float) matrix.M34,
			(float) matrix.M41, (float) matrix.M42, (float) matrix.M43, (float) matrix.M44);
	}

	public static class Math1D {
		public static double Clamp(double val, double min, double max) => val < min ? min :
			val > max ? max : val;
	}
}