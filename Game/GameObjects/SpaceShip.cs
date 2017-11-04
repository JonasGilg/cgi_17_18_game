﻿using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Texture;
using Engine.Util;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.GameObjects {
	public class SpaceShip : GameObject {
		public readonly MoveComponent MoveComponent;
		public readonly CameraComponent CameraComponent;
		public readonly RenderComponent RenderComponent;
		
		public SpaceShip() {
			MoveComponent = new MoveComponent(this);
			CameraComponent = new ThirdPersonCameraComponent(new Vector3(-0.3f, 0.05f, 0.0f), this);
			RenderComponent = new RenderComponent(
				ModelLoaderObject3D.load("data/objects/SpaceShip.obj"),
				MaterialManager.GetMaterial(Material.AmbientDiffuseSpecular),
				TextureManager.LoadTexture("data/textures/test.png"),
				this
			);
			DisplayCamera.SetActiveCamera(CameraComponent);
		}

		public override void Update() {
			//move forward
			if (Keyboard.Down(Key.W)) {
				var forward = TransformComponent.Orientation * new Vector3(Time.DeltaTime, 0.0f, 0.0f);
				MoveComponent.LinearVelocity += forward;
			}
			
			//move backward
			if (Keyboard.Down(Key.S)) {
				var backwards = TransformComponent.Orientation * new Vector3(-Time.DeltaTime, 0.0f, 0.0f);
				MoveComponent.LinearVelocity += backwards;
			}
			
			//move left
			if (Keyboard.Down(Key.A)) {
				var left = TransformComponent.Orientation * new Vector3(0.0f, 0.0f, -Time.DeltaTime);
				MoveComponent.LinearVelocity += left;
			}

			//move right
			if (Keyboard.Down(Key.D)) {
				var right = TransformComponent.Orientation * new Vector3(0.0f, 0.0f, Time.DeltaTime);
				MoveComponent.LinearVelocity += right;
			}

			//move up
			if (Keyboard.Down(Key.Space)) {
				var right = TransformComponent.Orientation * new Vector3(0.0f, Time.DeltaTime, 0.0f);
				MoveComponent.LinearVelocity += right;
			}
			
			//move down
			if (Keyboard.Down(Key.X)) {
				var right = TransformComponent.Orientation * new Vector3(0.0f, -Time.DeltaTime, 0.0f);
				MoveComponent.LinearVelocity += right;
			}
			
			//turn left
			if (Keyboard.Down(Key.Q)) {
				var left = TransformComponent.Orientation * new Vector3(0.0f, Time.DeltaTime, 0.0f);
				MoveComponent.AngularVelocity += left;
			}
			
			//turn right
			if (Keyboard.Down(Key.E)) {
				var right = TransformComponent.Orientation * new Vector3(0.0f, -Time.DeltaTime, 0.0f);
				MoveComponent.AngularVelocity += right;
			}
			
			//tilt forward
			if (Keyboard.Down(Key.Up)) {
				var forward = TransformComponent.Orientation * new Vector3(0.0f, 0.0f, -Time.DeltaTime);
				MoveComponent.AngularVelocity += forward;
			}
			
			//tilt backward
			if (Keyboard.Down(Key.Down)) {
				var backward = TransformComponent.Orientation * new Vector3(0.0f, 0.0f, Time.DeltaTime);
				MoveComponent.AngularVelocity += backward;
			}
			
			//tilt left
			if (Keyboard.Down(Key.Left)) {
				var left = TransformComponent.Orientation * new Vector3(-Time.DeltaTime, 0.0f, 0.0f);
				MoveComponent.AngularVelocity += left;
			}
			
			//tilt right
			if (Keyboard.Down(Key.Right)) {
				var right = TransformComponent.Orientation * new Vector3(Time.DeltaTime, 0.0f, 0.0f);
				MoveComponent.AngularVelocity += right;
			}

			
			if (Keyboard.Pressed(Key.C)) {
				MoveComponent.LinearVelocity = Vector3.Zero;
				MoveComponent.AngularVelocity = Vector3.Zero;
			}

			
			if (Keyboard.Down(Key.B)) {
				if (MoveComponent.LinearVelocity.Length > 0.05f) {
					MoveComponent.LinearVelocity *= 1 - Time.DeltaTime;
				}
				else {
					MoveComponent.LinearVelocity = Vector3.Zero;
				}

				if (MoveComponent.AngularVelocity.Length > 0.05f) {
					MoveComponent.AngularVelocity *= 1 - Time.DeltaTime;
				}
				else {
					MoveComponent.AngularVelocity = Vector3.Zero;
				}
			}
			
			MoveComponent.Update();
			base.Update();
			RenderComponent.Update();
			CameraComponent.Update();
		}

		public override void Draw() {
			base.Draw();
			RenderComponent.Draw(16.0f);
		}
	}
}