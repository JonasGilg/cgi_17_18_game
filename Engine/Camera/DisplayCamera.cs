﻿using System;
using Engine.Render;
using OpenTK;

namespace Engine {
	public static class DisplayCamera {
		private static CameraComponent activeCamera;
		private static Matrix4d perspectiveProjection;

		public static Vector3d Position => activeCamera.Position;
		public static Matrix4d Transformation => activeCamera.LookAtMatrix;
		public static Matrix4d PerspectiveProjection => perspectiveProjection;
		public static CameraComponent.Plane[] Planes => activeCamera.Planes;

		static DisplayCamera() {
			perspectiveProjection = Matrix4d.Identity;
			activeCamera = new CameraComponent(null);
		}

		public static void SetActiveCamera(CameraComponent camera) => activeCamera = camera;

		public static Intersect IsSphereInFrustum(Sphere boundingSphere) =>
			activeCamera.IsSphereInFrustum(boundingSphere);

		public static void SetWidthHeightFov(int width, int height, double fov) {
			var aspectRatio = width / (float) height;
			Matrix4d.CreatePerspectiveFieldOfView(fov * Math.PI / 180.0, aspectRatio, 0.01, 100000,
				out perspectiveProjection);
		}
	}
}