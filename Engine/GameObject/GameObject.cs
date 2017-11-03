using Engine;
using Engine.Model;
using OpenTK.Input;
using System;

namespace Engine {
	public class GameObject {
		public readonly TransformComponent TransformComponent;
		public CollisionComponent collisionComponent;
		public RenderComponent Renderer;
		public string name;
		public string tag;

		public GameObject() {
			TransformComponent = TransformComponent.Identity();
		}

		public virtual void EarlyUpdate() {

		}

		public virtual void Update() {
			TransformComponent.Update();
		}


		public virtual void LateUpdate() {

		}

		

		public void Draw() {
			Renderer?.Draw();
		}

		public virtual void OnCollisionEnter(Collision collision) {
			Console.WriteLine(collision.gameObjects[0].name + " and " + collision.gameObjects[1].name + " collided!");
		}
	}
}