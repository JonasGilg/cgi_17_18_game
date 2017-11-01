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
			
			//move forward
			if (input[Key.W]) {
				var forward = new Vector3(deltaTimeF, 0.0f, 0.0f);
				Math3D.Rotate(ref forward, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += forward;
			}
			
			//move backward
			if (input[Key.S]) {
				var backwards = new Vector3(-deltaTimeF, 0.0f, 0.0f);
				Math3D.Rotate(ref backwards, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += backwards;
			}
			
			//move left
			if (input[Key.A]) {
				var left = new Vector3(0.0f, 0.0f, -deltaTimeF);
				Math3D.Rotate(ref left, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += left;
			}

			//move right
			if (input[Key.D]) {
				var right = new Vector3(0.0f, 0.0f, deltaTimeF);
				Math3D.Rotate(ref right, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += right;
			}

			//move up
			if (input[Key.Space]) {
				var right = new Vector3(0.0f, deltaTimeF, 0.0f);
				Math3D.Rotate(ref right, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += right;
			}
			
			//move down
			if (input[Key.X]) {
				var right = new Vector3(0.0f, -deltaTimeF, 0.0f);
				Math3D.Rotate(ref right, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += right;
			}
			
			//turn left
			if (input[Key.Q]) {
				var left = new Vector3(0.0f, deltaTimeF, 0.0f);
				Math3D.Rotate(ref left, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += left;
			}
			
			//turn right
			if (input[Key.E]) {
				var right = new Vector3(0.0f, -deltaTimeF, 0.0f);
				Math3D.Rotate(ref right, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += right;
			}
			
			//tilt forward
			if (input[Key.Up]) {
				var forward = new Vector3(0.0f, 0.0f, -deltaTimeF);
				Math3D.Rotate(ref forward, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += forward;
			}
			
			//tilt backward
			if (input[Key.Down]) {
				var backward = new Vector3(0.0f, 0.0f, deltaTimeF);
				Math3D.Rotate(ref backward, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += backward;
			}
			
			//tilt left
			if (input[Key.Left]) {
				var left = new Vector3(-deltaTimeF, 0.0f, 0.0f);
				Math3D.Rotate(ref left, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += left;
			}
			
			//tilt right
			if (input[Key.Right]) {
				var right = new Vector3(deltaTimeF, 0.0f, 0.0f);
				Math3D.Rotate(ref right, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += right;
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