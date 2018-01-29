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
		private static readonly List<GameObject> OBJECTS = new List<GameObject>();

		

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

			//check for collision
			CollisionEngine.CheckCollisions();

			UPDATE_STATS.Stop();
		}

		public static void RenderWorld() {
			RENDER_STATS.Start();

			//TODO better perfomance possible if skybox is rendered last (that needs a refactoring of the shader though)
			Skybox.Draw();
			RenderEngine.Draw();
			PostProcessing.DrawMaterials();

			RENDER_STATS.Stop();
		}


		public static void AddToWorld(GameObject obj) {
			obj.Awake();
			OBJECTS.Add(obj);
		}

		public static void RemoveFromWorld(GameObject obj) => OBJECTS.Remove(obj);
		

		
	}
}