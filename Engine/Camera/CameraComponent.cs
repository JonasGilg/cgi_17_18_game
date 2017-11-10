using OpenTK;

namespace Engine {
	public class CameraComponent : Component.Component {
		private enum PlaneEnum {
			NearPlane = 0,
			FarPlane = 1,
			LeftPlane = 2,
			RightPlane = 3,
			TopPlane = 4,
			BottomPlane = 5
		}

		private struct Plane {
			public double D;
			public Vector3d Normal;
		}

		public Vector3d Position { get; private set; }
		public Matrix4d LookAtMatrix { get; private set; }
		private readonly Plane[] _planes;

		public CameraComponent(GameObject gameObject) : base(gameObject) {
			_planes = new Plane[6];
		}

		public void SetLookAt(Vector3d eye, Vector3d target, Vector3d up) {
			LookAtMatrix = Matrix4d.LookAt(eye, target, up);
			Position = eye;

			CreateViewFrustumPlanes(LookAtMatrix * DisplayCamera.PerspectiveProjection);
		}

		private void CreateViewFrustumPlanes(Matrix4d mat) {
			// left
			var plane = new Plane {
				Normal = {
					X = mat.M14 + mat.M11,
					Y = mat.M24 + mat.M21,
					Z = mat.M34 + mat.M31
				},
				D = mat.M44 + mat.M41
			};
			_planes[(int) PlaneEnum.LeftPlane] = plane;

			// right
			plane = new Plane {
				Normal = {
					X = mat.M14 - mat.M11,
					Y = mat.M24 - mat.M21,
					Z = mat.M34 - mat.M31
				},
				D = mat.M44 - mat.M41
			};
			_planes[(int) PlaneEnum.RightPlane] = plane;

			// bottom
			plane = new Plane {
				Normal = {
					X = mat.M14 + mat.M12,
					Y = mat.M24 + mat.M22,
					Z = mat.M34 + mat.M32
				},
				D = mat.M44 + mat.M42
			};
			_planes[(int) PlaneEnum.BottomPlane] = plane;

			// top
			plane = new Plane {
				Normal = {
					X = mat.M14 - mat.M12,
					Y = mat.M24 - mat.M22,
					Z = mat.M34 - mat.M32
				},
				D = mat.M44 - mat.M42
			};
			_planes[(int) PlaneEnum.TopPlane] = plane;

			// near
			plane = new Plane {
				Normal = {
					X = mat.M14 + mat.M13,
					Y = mat.M24 + mat.M23,
					Z = mat.M34 + mat.M33
				},
				D = mat.M44 + mat.M43
			};
			_planes[(int) PlaneEnum.NearPlane] = plane;

			// far
			plane = new Plane {
				Normal = {
					X = mat.M14 - mat.M13,
					Y = mat.M24 - mat.M23,
					Z = mat.M34 - mat.M33
				},
				D = mat.M44 - mat.M43
			};
			_planes[(int) PlaneEnum.FarPlane] = plane;

			// normalize
			for (var i = 0; i < 6; i++) {
				plane = _planes[i];

				var length = plane.Normal.Length;
				plane.Normal.X = plane.Normal.X / length;
				plane.Normal.Y = plane.Normal.Y / length;
				plane.Normal.Z = plane.Normal.Z / length;
				plane.D = plane.D / length;

				_planes[i] = plane;
			}
		}

		private double SignedDistanceToPoint(int planeID, Vector3d pt) {
			return Vector3d.Dot(_planes[planeID].Normal, pt) + _planes[planeID].D;
		}

		public bool SphereIsInFrustum(Vector3d center, double radius) {
			for (var i = 0; i < 6; i++) {
				if (SignedDistanceToPoint(i, center) < -radius) {
					return false;
				}
			}
			return true;
		}

		public override void Update() { }
	}
}