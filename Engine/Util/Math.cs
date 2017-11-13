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
		
		public static Matrix3 ToFloat(this Matrix3d matrix) => new Matrix3(
			(float) matrix.M11, (float) matrix.M12, (float) matrix.M13, 
			(float) matrix.M21, (float) matrix.M22, (float) matrix.M23, 
			(float) matrix.M31, (float) matrix.M32, (float) matrix.M33);

		public static Vector3 ToFloat(this Vector3d vec) => new Vector3((float) vec.X, (float) vec.Y, (float) vec.Z);

		public static Vector3d ToDouble(this Vector3 vec) => new Vector3d(vec.X, vec.Y, vec.Z);
	}

	public static class Math1D {
		public static double Clamp(double val, double min, double max) => val < min ? min :
			val > max ? max : val;
	}
}