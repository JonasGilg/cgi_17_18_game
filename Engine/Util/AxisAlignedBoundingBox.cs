using System;
using OpenTK;

namespace Engine {
	public struct AxisAlignedBoundingBox {
		private readonly Vector3d center;
		private readonly Vector3d halfDimension;
		
		public AxisAlignedBoundingBox(Vector3d center, Vector3d halfDimension) {
			this.center = center;
			this.halfDimension = halfDimension;
		}

		public bool Intersects(AxisAlignedBoundingBox other) {
			if (Math.Abs(center.X - other.center.X) > halfDimension.X + other.halfDimension.X) return false;
			if (Math.Abs(center.Y - other.center.Y) > halfDimension.Y + other.halfDimension.Y) return false;
			if (Math.Abs(center.Z - other.center.Z) > halfDimension.Z + other.halfDimension.Z) return false;
			
			return true;
		}
	}
}