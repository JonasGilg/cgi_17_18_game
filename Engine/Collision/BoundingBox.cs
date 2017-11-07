using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Collision {
	public class BoundingBox {
		public Vector3d minVec;
		public Vector3d maxVec;

		public bool Intersects(BoundingBox bbox) {
			return false;
		}
	}

	
}
