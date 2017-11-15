using System;
using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Game.Components;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		public readonly MoveComponent MoveComponent;
		private readonly RenderComponent renderComponent;
		public readonly SphereCollider CollisionComponent;

		public Asteroid(string modelObjectPath, int textureId, GameObject referenceObject = null) {
			MoveComponent = referenceObject != null ? new GravityMovement(this, 0.0) : new MoveComponent(this);

			var model = ModelLoaderObject3D.Load(modelObjectPath, this);

			renderComponent = new RenderComponent(
				model,
				MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
				textureId,
				this
			);

			CollisionComponent = new SphereCollider(this, renderComponent.Model,
				collision => { Console.WriteLine("Asteroid collided with" + collision.GameObject.ToString()); });
			CollisionComponent.Register();
		}

		public override void Awake() {
			base.Awake();

			Radius = renderComponent.Model.GetRadius();
		}

		public override void Update() {
			MoveComponent.Update();
			base.Update();
			renderComponent.Update();
		}

		public override void Draw() {
			base.Draw();
			renderComponent.Draw(0.1f);
		}
	}
}