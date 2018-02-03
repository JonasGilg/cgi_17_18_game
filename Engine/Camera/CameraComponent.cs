using Engine.Render;
using OpenTK;

namespace Engine {
	public class CameraComponent : Component.Component {
		private enum PlaneEnum {
			NEAR_PLANE = 0,
			FAR_PLANE = 1,
			LEFT_PLANE = 2,
			RIGHT_PLANE = 3,
			TOP_PLANE = 4,
			BOTTOM_PLANE = 5
		}

		public struct Plane {
			public double D;
			public Vector3d Normal;
		}

		public Vector3d Position { get; private set; }
		public Matrix4d PerspectiveProjection;
		public Matrix4d LookAtMatrix;
		public readonly Plane[] Planes;

		public CameraComponent(GameObject gameObject) : base(gameObject) => Planes = new Plane[6];

		protected void SetLookAt(Vector3d eye, Vector3d target, Vector3d up) {
			LookAtMatrix = Matrix4d.LookAt(eye, target, up);
			Position = eye;

			CreateViewFrustumPlanes(LookAtMatrix * DisplayCamera.PerspectiveProjection);
		}

		internal void CreateViewFrustumPlanes(Matrix4d mat) {
			var plane = new Plane {
				Normal = {
					X = mat.M14 + mat.M11,
					Y = mat.M24 + mat.M21,
					Z = mat.M34 + mat.M31
				},
				D = mat.M44 + mat.M41
			};
			Planes[(int) PlaneEnum.LEFT_PLANE] = plane;

			plane = new Plane {
				Normal = {
					X = mat.M14 - mat.M11,
					Y = mat.M24 - mat.M21,
					Z = mat.M34 - mat.M31
				},
				D = mat.M44 - mat.M41
			};
			Planes[(int) PlaneEnum.RIGHT_PLANE] = plane;

			plane = new Plane {
				Normal = {
					X = mat.M14 + mat.M12,
					Y = mat.M24 + mat.M22,
					Z = mat.M34 + mat.M32
				},
				D = mat.M44 + mat.M42
			};
			Planes[(int) PlaneEnum.BOTTOM_PLANE] = plane;

			plane = new Plane {
				Normal = {
					X = mat.M14 - mat.M12,
					Y = mat.M24 - mat.M22,
					Z = mat.M34 - mat.M32
				},
				D = mat.M44 - mat.M42
			};
			Planes[(int) PlaneEnum.TOP_PLANE] = plane;

			plane = new Plane {
				Normal = {
					X = mat.M14 + mat.M13,
					Y = mat.M24 + mat.M23,
					Z = mat.M34 + mat.M33
				},
				D = mat.M44 + mat.M43
			};
			Planes[(int) PlaneEnum.NEAR_PLANE] = plane;

			plane = new Plane {
				Normal = {
					X = mat.M14 - mat.M13,
					Y = mat.M24 - mat.M23,
					Z = mat.M34 - mat.M33
				},
				D = mat.M44 - mat.M43
			};
			Planes[(int) PlaneEnum.FAR_PLANE] = plane;

			for (var i = 0; i < 6; i++) {
				plane = Planes[i];

				var length = plane.Normal.Length;
				plane.Normal.X = plane.Normal.X / length;
				plane.Normal.Y = plane.Normal.Y / length;
				plane.Normal.Z = plane.Normal.Z / length;
				plane.D = plane.D / length;

				Planes[i] = plane;
			}
		}

		private double SignedDistanceToPoint(int planeID, Vector3d pt) => Vector3d.Dot(Planes[planeID].Normal, pt) + Planes[planeID].D;

		public Intersect IsSphereInFrustum(Sphere boundingSphere) {
			var farIn = 0;

			for (var i = 0; i < Planes.Length; i++) {
				var dist = SignedDistanceToPoint(i, boundingSphere.Center);
				if (dist < -boundingSphere.Radius)
					return Intersect.OUTSIDE;
				if (dist > boundingSphere.Radius)
					farIn++;
			}

			return farIn == Planes.Length ? Intersect.INSIDE : Intersect.OVERLAP;
		}

		public override void Update() { }
	}
}