﻿using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Component;
using Game.Components;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		public readonly MoveComponent MoveComponent;
		public readonly RenderComponent RenderComponent;

		public Asteroid(Model3D model, int textureId, GameObject referenceObject = null) {
			if (referenceObject != null) {
				MoveComponent = new GravityMovement(this, 0.0);
			}
			else {
				MoveComponent = new MoveComponent(this);
			}
			
			RenderComponent = new RenderComponent(
				model,
				MaterialManager.GetMaterial(Material.AmbientDiffuseSpecular),
				textureId,
				this
			);
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