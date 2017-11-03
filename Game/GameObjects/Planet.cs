using Engine;
using Engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GameObjects {
	
	class Planet : GameObject {
		public Planet(string planetName) {
			name = planetName;
			Model = new ModelLoaderObject3D("data/objects/neptune.obj");
		}
	}
}
