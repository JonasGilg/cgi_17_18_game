using System;
using System.Collections.Generic;
using Engine.Material;
using Engine.Render;
using Engine.Render.Skybox;
using Engine.Util;
using Engine.Output;
using OpenTK.Graphics.OpenGL;
using TextureMinFilter = OpenTK.Graphics.OpenGL4.TextureMinFilter;

namespace Engine {
	public static class World {
		//game objects
		private static readonly List<GameObject> OBJECTS = new List<GameObject>();

		//components
		private static readonly List<CollisionComponent> COLLISION_COMPONENTS = new List<CollisionComponent>();

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
			CheckCollisions();

			UPDATE_STATS.Stop();
		}

		public static void RenderWorld() {
			RENDER_STATS.Start();

			//TODO better perfomance possible if skybox is rendered last (that needs a refactoring of the shader though)
			Skybox.Draw();
			
			/*for (var i = 0; i < OBJECTS.Count; i++) {
				if (DisplayCamera.IsSphereInFrustum(OBJECTS[i].TransformComponent.WorldPosition, OBJECTS[i].Radius)) {
					OBJECTS[i].Draw();
				}
			}*/
			
			RenderEngine.Draw();
			
			PostProcessing.DrawMaterials();
			
			RENDER_STATS.Stop();
		}


		public static void AddToWorld(GameObject obj) {
			obj.Awake();
			OBJECTS.Add(obj);
		}

		public static void RegisterCollisionComponent(CollisionComponent component) {
			COLLISION_COMPONENTS.Add(component);
		}

		public static void UnregisterCollisionComponent(CollisionComponent component) {
			COLLISION_COMPONENTS.Remove(component);
		}

		private static void CheckCollisions() {
			for (var i = 0; i < COLLISION_COMPONENTS.Count; i++) {
				var currObj = COLLISION_COMPONENTS[i];
				for (var j = 0; j < COLLISION_COMPONENTS.Count; j++) {
					if (i != j) {
						//cant collide with yourself
						var collidedWith = COLLISION_COMPONENTS[j];
						if (currObj.IsColliding(collidedWith)) {
							currObj.OnCollision(new Collision.Collision {
								GameObject = collidedWith.GameObject
							});
						}
					}
				}
			}
		}
	}
}