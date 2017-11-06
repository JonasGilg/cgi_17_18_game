using System.Collections.Generic;
using Engine.Render.Skybox;

namespace Engine {
	public static class World {
		public static readonly List<GameObject> Objects = new List<GameObject>();
		private static readonly Skybox _skybox = new Skybox();

		public static void UpdateWorld() {
			for (int i = 0; i < Objects.Count; i++) {
				Objects[i].Update();
			}
			_skybox.Update();
		}

		public static void RenderWorld() {
			//TODO better perfomance possible if skybox is rendered last (that needs a refactoring of the shader though)
			_skybox.Draw();
			for (int i = 0; i < Objects.Count; i++) {
				Objects[i].Draw();
			}
			
		}

		public static void AddToWorld(GameObject obj) {
			Objects.Add(obj);
		}
	}
}
