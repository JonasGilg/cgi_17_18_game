using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
	public class World {
		public List<GameObject> objects = new List<GameObject>();

		public void UpdateWorld() {
			objects.ForEach(obj => obj.EarlyUpdate());
			objects.ForEach(obj => obj.Update());
			objects.ForEach(obj => obj.LateUpdate());
		}

		public void RenderWorld() {
			objects.ForEach(obj => obj.Draw());
		}

		public void AddToWorld(GameObject obj) {
			objects.Add(obj);
		}
	}
}
