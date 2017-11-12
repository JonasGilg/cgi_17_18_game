﻿using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Component;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		public readonly MoveComponent MoveComponent;
		private readonly RenderComponent RenderComponent;
		public readonly SphereCollider CollisionComponent;

		public Asteroid(string modelObjectPath, int textureId, GameObject referenceObject = null) {
			if (referenceObject != null) {
				MoveComponent = new Components.GravityMovement(this, 0.0);
			}
			else {
				MoveComponent = new MoveComponent(this);
			}

			var model = ModelLoaderObject3D.Load(modelObjectPath, this);

			RenderComponent = new RenderComponent(
				model,
				MaterialManager.GetMaterial(Material.AmbientDiffuseSpecular),
				textureId,
				this
			);
			
			CollisionComponent = new SphereCollider(this, RenderComponent.Model,
				collision => { System.Console.WriteLine("Asteroid collided with" + collision.gameObject.ToString()); });
			CollisionComponent.Register();
		}

		public override void Awake() {
			base.Awake();
			
			Radius = RenderComponent.Model.GetRadius();
		}

		public override void Update() {
			MoveComponent.Update();
			base.Update();
			RenderComponent.Update();
		}

		public override void Draw() {
			base.Draw();
			RenderComponent.Draw(0.1f);
		}
	}
}