﻿using Engine.Component;
using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Util;
using OpenTK;
using OpenTK.Input;

namespace Game.GameObjects {
	public class SpaceShip : GameObject {
		public readonly MoveComponent MoveComponent;
		public readonly Model3D Model;
		
		public SpaceShip(Model3D model) {
			Model = model;
			MoveComponent = new MoveComponent(this);
			TransformComponent.Scale = new Vector3(0.02f);
		}

		public override void Update(double deltaTime, KeyboardDevice input) {
			var deltaTimeF = (float) deltaTime;
			
			if (input[Key.W]) {
				var forward = new Vector3(deltaTimeF, 0.0f, 0.0f);
				Math3D.Rotate(ref forward, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += forward;
			}
			
			if (input[Key.A]) {
				var left = new Vector3(0.0f, 0.0f, -deltaTimeF);
				Math3D.Rotate(ref left, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += left;
			}

			if (input[Key.S]) {
				var backwards = new Vector3(-deltaTimeF, 0.0f, 0.0f);
				Math3D.Rotate(ref backwards, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += backwards;
			}
			
			if (input[Key.D]) {
				var right = new Vector3(0.0f, 0.0f, deltaTimeF);
				Math3D.Rotate(ref right, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += right;
			}

			if (input[Key.Q]) {
				MoveComponent.AngularVelocity.Y += deltaTimeF;
			}
			
			if (input[Key.E]) {
				MoveComponent.AngularVelocity.Y -= deltaTimeF;
			}
			
			MoveComponent.Update(deltaTime, input);
			base.Update(deltaTime, input);
			Model.Update(TransformComponent.WorldMatrix);
		}
		
		public void Draw(AmbientDiffuseSpecularMaterial material, int texture) {
			material.Draw(Model, texture, 0.5f);
		}
	}
}