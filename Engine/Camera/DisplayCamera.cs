using System;
using OpenTK;

namespace Engine {
	public static class DisplayCamera {
		private static CameraComponent _activeCamera;

		private static Matrix4d _perspectiveProjection;

		static DisplayCamera() {
			_perspectiveProjection = Matrix4d.Identity;
			_activeCamera = new CameraComponent(null);
		}

		// width, height = size of screen in pixeln, fov = "field of view", der opening-angle for the camera lense
		public static void SetWidthHeightFov(int width, int height, double fov) {
			var aspectRatio = width / (float) height;
			Matrix4d.CreatePerspectiveFieldOfView(fov * Math.PI / 180.0, aspectRatio, 0.01, 100000,
				out _perspectiveProjection);
		}

		// Getter
		public static Vector3d Position => _activeCamera.Position;
		public static Matrix4d Transformation => _activeCamera.LookAtMatrix;

		public static void SetActiveCamera(CameraComponent camera) => _activeCamera = camera;

		public static Matrix4d PerspectiveProjection => _perspectiveProjection;
	}
}