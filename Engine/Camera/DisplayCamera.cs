using System;
using OpenTK;

namespace Engine {
	public static class DisplayCamera {
		private static Camera _activeCamera;

		private static Matrix4 _perspectiveProjection;

		public static void Init() {
			_perspectiveProjection = Matrix4.Identity;
			_activeCamera = new Camera();
		}

		// width, height = size of screen in pixeln, fov = "field of view", der opening-angle for the camera lense
		public static void SetWidthHeightFov(int width, int height, float fov) {
			var aspectRatio = width / (float) height;
			Matrix4.CreatePerspectiveFieldOfView((float) (fov * Math.PI / 180.0f), aspectRatio, 0.01f, 2000f,
				out _perspectiveProjection);
		}

		// Getter
		public static Vector3 Position => _activeCamera.TransformComponent.Position;
		public static Matrix4 Transformation => _activeCamera.LookAtMatrix;

		public static void SetActiveCamera(Camera camera) => _activeCamera = camera;

		public static Matrix4 PerspectiveProjection => _perspectiveProjection;
	}
}