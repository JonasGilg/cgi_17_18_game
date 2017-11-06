using System.Collections.Generic;
using Engine.Render.Skybox;

namespace Engine {
	public class World {
		public readonly List<GameObject> Objects = new List<GameObject>();
		private readonly Skybox _skybox = new Skybox();

		public void UpdateWorld() {
			Objects.ForEach(obj => obj.Update());
			_skybox.Update();
		}

		public void RenderWorld() {
			_skybox.Draw();
			Objects.ForEach(obj => obj.Draw());
		}

		public void AddToWorld(GameObject obj) {
			Objects.Add(obj);
		}
	}
}
