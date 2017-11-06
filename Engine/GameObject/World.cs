using System.Collections.Generic;
using Engine.Render.Skybox;

namespace Engine {
	public class World {
		public readonly List<GameObject> Objects = new List<GameObject>();
		private readonly Skybox _skybox = new Skybox();

		public void UpdateWorld() {
			for (int i = 0; i < Objects.Count; i++) {
				Objects[i].Update();
			}
			_skybox.Update();
		}

		public void RenderWorld() {
			//TODO better perfomance possible if skybox is rendered last (that needs a refactoring of the shader though)
			_skybox.Draw();
			for (int i = 0; i < Objects.Count; i++) {
				Objects[i].Draw();
			}
			
		}

		public void AddToWorld(GameObject obj) {
			Objects.Add(obj);
		}
	}
}
