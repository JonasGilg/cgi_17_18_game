using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	class World {
		public List<GameObject> objects;

		public void GameLoop() {
			objects.ForEach(obj => obj.EarlyUpdate());
			//objects.ForEach(obj => obj.Update());
			objects.ForEach(obj => obj.LateUpdate());
		}
	}
}
