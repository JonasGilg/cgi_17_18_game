using System;
using OpenTK;

namespace Engine {
	public static class DisplayCamera {
		private static CameraComponent _activeCamera;
		private static Matrix4d _perspectiveProjection;
		
		public static Vector3d Position => _activeCamera.Position;
		public static Matrix4d Transformation => _activeCamera.LookAtMatrix;
		public static Matrix4d PerspectiveProjection => _perspectiveProjection;
		
		public static void SetActiveCamera(CameraComponent camera) => _activeCamera = camera;

		public static bool SphereIsInFrustum(Vector3d center, double radius) =>
			_activeCamera.SphereIsInFrustum(center, radius);

		static DisplayCamera() {
			_perspectiveProjection = Matrix4d.Identity;
			_activeCamera = new CameraComponent(null);
		}

		public static void SetWidthHeightFov(int width, int height, double fov) {
			var aspectRatio = width / (float) height;
			Matrix4d.CreatePerspectiveFieldOfView(fov * Math.PI / 180.0, aspectRatio, 0.01, 100000,
				out _perspectiveProjection);
		}
	}
}