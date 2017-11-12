using Engine.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Collision {
	public class PhysicsMaterial {
		private double _bounce;
		public double Bounciness {
			get => _bounce;
			set => Math1D.Clamp(value, 0, 1);
		}

		private double _friction;

		public double Friction {
			get => _friction;
			set => Math1D.Clamp(value, 0, 1);
		}

		public PhysicsMaterial(double bounciness, double friction) {
			Bounciness = bounciness;
			Friction = friction;
		}

	}
}
