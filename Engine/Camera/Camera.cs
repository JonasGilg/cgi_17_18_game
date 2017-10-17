using System;
using OpenTK;

namespace Engine.Camera {
	public static class Camera {
		// Matrix for the transformation

		// ... and the petrspective projection
		private static Matrix4 _perspectiveProjection;

		// position for the camera is saved
		private static Vector3 _position;

		public static void Init() {
			_perspectiveProjection = Matrix4.Identity;
			Transformation = Matrix4.Identity;
		}

		// width, height = size of screen in pixeln, fov = "field of view", der opening-angle for the camera lense
		public static void SetWidthHeightFov(int width, int height, float fov) {
			var aspectRatio = width / (float) height;
			Matrix4.CreatePerspectiveFieldOfView((float) (fov * Math.PI / 180.0f), aspectRatio, 0.01f, 500,
				out _perspectiveProjection);
		}

		// generation of the camera-transformation using LookAt
		// position of the camera-"eye", look-at poinmt, "up" direction of camera
		public static void SetLookAt(Vector3 eye, Vector3 target, Vector3 up) {
			_position = eye;
			Transformation = Matrix4.LookAt(eye, target, up);
		}

		// Getter
		public static Vector3 Position => _position;

		public static Matrix4 Transformation { get; private set; }

		public static Matrix4 PerspectiveProjection => _perspectiveProjection;
	}
}