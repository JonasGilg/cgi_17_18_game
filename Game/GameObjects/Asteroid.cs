﻿using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Component;
using Engine.Texture;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		public readonly MoveComponent MoveComponent;
		public readonly RenderComponent RenderComponent;
		public readonly SphereCollider collisionComponent;

		public Asteroid(Model3D model, int textureId) {
			MoveComponent = new MoveComponent(this);
			RenderComponent = new RenderComponent(
				model,
				MaterialManager.GetMaterial(Material.AmbientDiffuseSpecular),
				textureId,
				this
			);
			collisionComponent = new SphereCollider(this, RenderComponent.Model, collision => {
				System.Console.WriteLine("Asteroid collided with" + collision.gameObject.ToString());
			});
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