using System;
using OpenTK;

namespace Engine {
	public enum Intersect {
		INSIDE,
		OUTSIDE,
		OVERLAP
	}

	public struct AxisAlignedBoundingBox {
		public Vector3d Center;
		public Vector3d HalfDimension;
		public Vector3d Min;
		public Vector3d Max;

		public static AxisAlignedBoundingBox FromCenterAndDimension(Vector3d center, Vector3d halfDimension)
			=> new AxisAlignedBoundingBox {
				Center = center,
				HalfDimension = halfDimension,
				Min = center - halfDimension,
				Max = center + halfDimension
			};

		public static AxisAlignedBoundingBox FromMinMax(Vector3d min, Vector3d max)
			=> new AxisAlignedBoundingBox {
				Center = (min + max) / 2,
				HalfDimension = (max - min) / 2,
				Min = min,
				Max = max
			};

		public static AxisAlignedBoundingBox Zero()
			=> new AxisAlignedBoundingBox {
				Center = Vector3d.Zero,
				HalfDimension = Vector3d.Zero,
				Min = Vector3d.Zero,
				Max = Vector3d.Zero
			};


		public Vector3d getVertexP(Vector3d planeNormal) {
			
			Vector3d p = Min;
			if (planeNormal.X >= 0) {
				p.X = Max.X;
			}
			if (planeNormal.Y >= 0) {
				p.Y = Max.Y;
			}
			if (planeNormal.Z >= 0) {
				p.Z = Max.Z;
			}
			return p;
		}

		public Vector3d getVertexN(Vector3d planeNormal) {
			Vector3d n = Max;
			if (planeNormal.X >= 0) {
				n.X = Min.X;
			}
			if (planeNormal.Y >= 0) {
				n.Y = Min.Y;
			}
			if (planeNormal.Z >= 0) {
				n.Z = Min.Z;
			}

			return n;
		}
		public Vector3d GetMinOrMax(bool max) => max ? Max : Min;

		public bool IsInside(Vector3d point) => ((point.X > Min.X) && (point.X < Max.X) && (point.Y > Min.Y) &&
		                                         (point.Y < Max.Y) && (point.Z > Min.Z) && (point.Z < Max.Z));

		public bool IsInside(AxisAlignedBoundingBox other) => IsInside(other.Min) && IsInside(other.Max);

		public bool Overlaps(AxisAlignedBoundingBox other) {
			var x = Math.Abs(Center.X - other.Center.X) <= (HalfDimension.X + other.HalfDimension.X);
			var y = Math.Abs(Center.Y - other.Center.Y) <= (HalfDimension.Y + other.HalfDimension.Y);
			var z = Math.Abs(Center.Z - other.Center.Z) <= (HalfDimension.Z + other.HalfDimension.Z);

			return x && y && z;
		}

		public Intersect Intersects(AxisAlignedBoundingBox other) {
			if (IsInside(other)) return Intersect.INSIDE;
			if (Overlaps(other)) return Intersect.OVERLAP;
			return Intersect.OUTSIDE;
		}

		public static AxisAlignedBoundingBox operator +(AxisAlignedBoundingBox self, Vector3d translation)
			=> FromCenterAndDimension(self.Center + translation, self.HalfDimension);

		public static AxisAlignedBoundingBox operator *(AxisAlignedBoundingBox self, Vector3d scale)
			=> FromCenterAndDimension(self.Center, self.HalfDimension * scale);
	}
}