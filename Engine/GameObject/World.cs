using System.Collections.Generic;

namespace Engine {
	public class World {
		public List<GameObject> objects = new List<GameObject>();

		public void UpdateWorld() {
			objects.ForEach(obj => obj.Update());
		}

		public void RenderWorld() {
			objects.ForEach(obj => obj.Draw());
		}

		public void AddToWorld(GameObject obj) {
			objects.Add(obj);
		}
	}
}
