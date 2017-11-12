using System;
using System.Collections.Generic;
using Engine.Render.Skybox;
using Engine.Util;

namespace Engine {
	public static class World {
		//game objects
		public static readonly List<GameObject> Objects = new List<GameObject>();
		
		//components
		public static readonly List<CollisionComponent> CollisionComponents = new List<CollisionComponent>();

		
		//stats
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
			//check for collision
			checkCollisions();


			UpdateStats.Stop();
		}

		public static void RenderWorld() {
			RenderStats.Start();

			//TODO better perfomance possible if skybox is rendered last (that needs a refactoring of the shader though)
			Skybox.Draw();
			for (var i = 0; i < Objects.Count; i++) {
				if (DisplayCamera.SphereIsInFrustum(Objects[i].TransformComponent.WorldPosition, Objects[i].Radius)) {
					Objects[i].Draw();
				}
			}

			RenderStats.Stop();
		}

		public static void AddToWorld(GameObject obj) {
			obj.Awake();
			Objects.Add(obj);
		}

		public static void registerCollisionComponent(CollisionComponent component) {
			CollisionComponents.Add(component);
		}

		public static void unregisterCollisionComponent(CollisionComponent component) {
			CollisionComponents.Remove(component);
		}


		private static void checkCollisions() {
			for (var i = 0; i < CollisionComponents.Count; i++) {
				var currObj = CollisionComponents[i];
				for (var j = 0; j < CollisionComponents.Count; j++) {
					
					if (i != j) { //cant collide with yourself
						var collidedWith = CollisionComponents[j];
						if (currObj.IsColliding(collidedWith)) {
							currObj.OnCollision(new Collision.Collision {
								gameObject = collidedWith.GameObject
							});
						}
					}
				}
			}
		}
	}
}