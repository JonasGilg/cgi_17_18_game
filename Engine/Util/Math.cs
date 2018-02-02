using System;
using OpenTK;

namespace Engine.Render {
	public static class Math3D {
		public static Vector3d Rotate(this Quaterniond quat, Vector3d vec) {
			Vector3d.Transform(ref vec, ref quat, out var result);
			return result;
		}
		
		public static Quaterniond LookAt(Vector3d sourcePoint, Vector3d destPoint)
		{
			var forwardVector = Vector3d.Normalize(destPoint - sourcePoint);

			var dot = Vector3d.Dot(Vector3d.UnitX, forwardVector);

			if (Math.Abs(dot - (-1.0f)) < 0.000001f)
			{
				return new Quaterniond(0, 1, 0, 3.1415926535897932f);
			}
			if (Math.Abs(dot - (1.0f)) < 0.000001f)
			{
				return Quaterniond.Identity;
			}

			var rotAngle = (float)Math.Acos(dot);
			var rotAxis = Vector3d.Cross(Vector3d.UnitX, forwardVector);
			rotAxis = Vector3d.Normalize(rotAxis);
			return Quaterniond.FromAxisAngle(rotAxis, rotAngle);
		}

		public static Vector3d ReflectionVector3D(Vector3d a, Vector3d norm) {
			Vector3d result = Vector3d.One;
			var aNormal = a.Normalized();
			var normNormal = norm.Normalized();
			double dotProd = Vector3d.Dot(aNormal, normNormal);
			
			
			return new Vector3d(
				aNormal.X - 2 * normNormal.X * dotProd,
				aNormal.Y - 2 * normNormal.Y * dotProd,
				aNormal.Z - 2 * normNormal.Z * dotProd
			                    );
		}
		public static Vector3d ToRadiansVector3D(this Vector3d degreesVector) => new Vector3d(
			degreesVector.X.ToRadians(),
			degreesVector.Y.ToRadians(),
			degreesVector.Z.ToRadians());

		public static Matrix4 ToFloat(this Matrix4d matrix) => new Matrix4(
			(float) matrix.M11, (float) matrix.M12, (float) matrix.M13, (float) matrix.M14,
			(float) matrix.M21, (float) matrix.M22, (float) matrix.M23, (float) matrix.M24,
			(float) matrix.M31, (float) matrix.M32, (float) matrix.M33, (float) matrix.M34,
			(float) matrix.M41, (float) matrix.M42, (float) matrix.M43, (float) matrix.M44);
		
		public static Matrix4d ToDouble(this Matrix4 matrix) => new Matrix4d(
			matrix.M11, matrix.M12, matrix.M13, matrix.M14,
			matrix.M21, matrix.M22, matrix.M23, matrix.M24,
			matrix.M31, matrix.M32, matrix.M33, matrix.M34,
			matrix.M41, matrix.M42, matrix.M43, matrix.M44);

		public static Matrix3 ToFloat(this Matrix3d matrix) => new Matrix3(
			(float) matrix.M11, (float) matrix.M12, (float) matrix.M13,
			(float) matrix.M21, (float) matrix.M22, (float) matrix.M23,
			(float) matrix.M31, (float) matrix.M32, (float) matrix.M33);

		public static Vector3 ToFloat(this Vector3d vec) => new Vector3((float) vec.X, (float) vec.Y, (float) vec.Z);

		public static Vector3d ToDouble(this Vector3 vec) => new Vector3d(vec.X, vec.Y, vec.Z);
		
		public static Vector2 ToFloat(this Vector2d vec) => new Vector2((float)vec.X, (float)vec.Y);
		
		public static Vector2d ToDouble(this Vector2 vec) => new Vector2d(vec.X, vec.Y);

		public static float Distance(Vector3 first, Vector3 second) {
			float deltaX = second.X - first.X;
			float deltaY = second.Y - first.Y;
			float deltaZ = second.Z - first.Z;

			return (float) Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
		}

		public static Vector3d InMiddleOf(Vector3d first, Vector3d second) {
			return (first + second) / 2;
		}
	}

	public static class Math1D {
		public const double TAU = Math.PI * 2;

		public static double ToRadians(this double degrees) => degrees * Math.PI / 180;
		
		public static double ToDegrees(this double radians) => radians * 180 / Math.PI ;
		
		public static double Clamp(double val, double min, double max) => val < min ? min :
			val > max ? max : val;
	}
}