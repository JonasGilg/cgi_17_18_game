using Engine;
using Engine.Model;
using OpenTK.Input;
using System;

namespace Engine {
	public class GameObject {
		public readonly TransformComponent TransformComponent;
		public CollisionComponent collisionComponent;
		public Model3D Model;
		public string name;
		public string tag;

		public GameObject() {
			TransformComponent = TransformComponent.Identity();
		}

		public virtual void EarlyUpdate() {

		}


		public virtual void LateUpdate() {

		}

		public virtual void Update() {
			TransformComponent.Update();
		}

		public void Draw() {

		}

		public virtual void OnCollisionEnter(Collision collision) {
			Console.WriteLine(collision.gameObjects[0].name + " and " + collision.gameObjects[1].name + " collided!");
		}
	}
}