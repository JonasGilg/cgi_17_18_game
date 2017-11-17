using System;
using OpenTK;

namespace Engine {
	public static class DisplayCamera {
		private static CameraComponent activeCamera;
		private static Matrix4d perspectiveProjection;

		public static Vector3d Position => activeCamera.Position;
		public static Matrix4d Transformation => activeCamera.LookAtMatrix;
		public static Matrix4d PerspectiveProjection => perspectiveProjection;

		static DisplayCamera() {
			perspectiveProjection = Matrix4d.Identity;
			activeCamera = new CameraComponent(null);
		}

		public static void SetActiveCamera(CameraComponent camera) => activeCamera = camera;

		public static bool IsSphereInFrustum(Vector3d center, double radius) =>
			activeCamera.IsSphereInFrustum(center, radius);

		public static Intersect IsAABBInFrustum(AxisAlignedBoundingBox aabb) => activeCamera.IsAABBInFrustum(aabb);

		public static void SetWidthHeightFov(int width, int height, double fov) {
			var aspectRatio = width / (float) height;
			Matrix4d.CreatePerspectiveFieldOfView(fov * Math.PI / 180.0, aspectRatio, 0.01, 100000,
				out perspectiveProjection);
		}
	}
}