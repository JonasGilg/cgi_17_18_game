using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Engine.Collision;
using Engine.Material;
using Engine.Render;
using Engine.Render.Skybox;

namespace Engine {
	public static class World {
		//game objects
		private static List<GameObject> OBJECTS = new List<GameObject>();

		

		//stats
		private static readonly TimingStats UPDATE_STATS = new TimingStats("World");
		private static readonly TimingStats RENDER_STATS = new TimingStats("World");


		static World() {
			TimingRegistry.AddUpdateTiming(UPDATE_STATS);
			TimingRegistry.AddRenderTiming(RENDER_STATS);
		}


		public static void UpdateWorld() {
			UPDATE_STATS.Start();

			for (var i = 0; i < OBJECTS.Count; i++) {
				OBJECTS[i].Update();
			}

			CollisionEngine.CheckCollisions();

			UPDATE_STATS.Stop();
		}

		public static void RenderWorld() {
			RENDER_STATS.Start();

			RenderEngine.Draw();
			PostProcessing.DrawMaterials();

			RENDER_STATS.Stop();
		}


		public static void AddToWorld(GameObject obj) {
			OBJECTS.Add(obj);
		}

		public static void RemoveFromWorld(GameObject obj) => OBJECTS.Remove(obj);

		public static void ClearWorld() {
			//Removing objects directly from the OBJECTS list results in a ModificationException
			//workaround: copy all elements to an array, assign empty list to OBJECTS and delete the elements in the created array
			var toDelete = new GameObject[OBJECTS.Count];
			OBJECTS.CopyTo(toDelete);
			OBJECTS = new List<GameObject>();
			foreach (var obj in toDelete) {
				GameObject.Destroy(obj);
			}
		}

	}
}