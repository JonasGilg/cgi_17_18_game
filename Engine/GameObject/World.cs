﻿using System.Collections.Generic;
using Engine.Render.Skybox;
using Engine.Util;

namespace Engine {
	public static class World {
		public static readonly List<GameObject> Objects = new List<GameObject>();
		public static readonly List<CollisionComponent> collisionObjects = new List<CollisionComponent>();
		
		private static readonly TimingStats UpdateStats = new TimingStats("World");
		private static readonly TimingStats RenderStats = new TimingStats("World");

		static World() {
			TimingRegistry.AddUpdateTiming(UpdateStats);
			TimingRegistry.AddRenderTiming(RenderStats);
		}

		public static void UpdateWorld() {
			UpdateStats.Start();
			
			for (var i = 0; i < Objects.Count; i++) {
				Objects[i].Update();
			}
			
			UpdateStats.Stop();
		}

		public static void RenderWorld() {
			RenderStats.Start();
			
			//TODO better perfomance possible if skybox is rendered last (that needs a refactoring of the shader though)
			Skybox.Draw();
			for (var i = 0; i < Objects.Count; i++) {
				Objects[i].Draw();
			}
			
			RenderStats.Stop();
		}

		public static void AddToWorld(GameObject obj, CollisionComponent collisionToAdd = null) {
			Objects.Add(obj);
			if (collisionToAdd != null) collisionObjects.Add(collisionToAdd);
		}
	}
}
