using System.Collections.Generic;
using Engine.Render.Skybox;

namespace Engine {
	public class World {
		public List<GameObject> objects = new List<GameObject>();
		private Skybox _skybox = new Skybox();

		public void UpdateWorld() {
			_skybox.Update();
			objects.ForEach(obj => obj.Update());
		}

		public void RenderWorld() {
			_skybox.Draw();
			objects.ForEach(obj => obj.Draw());
		}

		public void AddToWorld(GameObject obj) {
			objects.Add(obj);
		}
	}
}
