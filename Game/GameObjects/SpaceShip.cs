using System;
using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Texture;
using Engine.Util;
using Engine.Component;
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
			CameraComponent = new ThirdPersonCameraComponent(new Vector3d(-0.3, 0.05, 0.0), this);
			RenderComponent = new RenderComponent(
				ModelLoaderObject3D.Load("data/objects/SpaceShip.obj"),
				MaterialManager.GetMaterial(Material.AmbientDiffuseSpecular),
				TextureManager.LoadTexture("data/textures/test.png"),
				this
			);
			DisplayCamera.SetActiveCamera(CameraComponent);
		}

		public override void Update() {
			//move forward
			if (Keyboard.Down(Key.W)) {
				var forward = TransformComponent.Orientation.Rotate(new Vector3d(Time.DeltaTime, 0.0, 0.0));
				MoveComponent.LinearVelocity += forward * 100;
			}

			//move backward
			if (Keyboard.Down(Key.S)) {
				var backwards = TransformComponent.Orientation.Rotate(new Vector3d(-Time.DeltaTime, 0.0, 0.0));
				MoveComponent.LinearVelocity += backwards;
			}

			//move left
			if (Keyboard.Down(Key.Q)) {
				var left = TransformComponent.Orientation.Rotate(new Vector3d(0.0, 0.0, -Time.DeltaTime));
				MoveComponent.LinearVelocity += left;
			}

			//move right
			if (Keyboard.Down(Key.E)) {
				var right = TransformComponent.Orientation.Rotate(new Vector3d(0.0, 0.0, Time.DeltaTime));
				MoveComponent.LinearVelocity += right;
			}

			//move up
			if (Keyboard.Down(Key.Space)) {
				var right = TransformComponent.Orientation.Rotate(new Vector3d(0.0, Time.DeltaTime, 0.0));
				MoveComponent.LinearVelocity += right;
			}

			//move down
			if (Keyboard.Down(Key.X)) {
				var right = TransformComponent.Orientation.Rotate(new Vector3d(0.0, -Time.DeltaTime, 0.0));
				MoveComponent.LinearVelocity += right;
			}

			//turn left
			if (Keyboard.Down(Key.A)) {
				var left = TransformComponent.Orientation.Rotate(new Vector3d(0.0, Time.DeltaTime, 0.0));
				MoveComponent.AngularVelocity += left;
			}

			//turn right
			if (Keyboard.Down(Key.D)) {
				var right = TransformComponent.Orientation.Rotate(new Vector3d(0.0, -Time.DeltaTime, 0.0));
				MoveComponent.AngularVelocity += right;
			}

			//tilt forward
			if (Keyboard.Down(Key.Up)) {
				var forward = TransformComponent.Orientation.Rotate(new Vector3d(0.0, 0.0, -Time.DeltaTime));
				MoveComponent.AngularVelocity += forward;
			}

			//tilt backward
			if (Keyboard.Down(Key.Down)) {
				var backward = TransformComponent.Orientation.Rotate(new Vector3d(0.0, 0.0, Time.DeltaTime));
				MoveComponent.AngularVelocity += backward;
			}

			//tilt left
			if (Keyboard.Down(Key.Left)) {
				var left = TransformComponent.Orientation.Rotate(new Vector3d(-Time.DeltaTime, 0.0, 0.0));
				MoveComponent.AngularVelocity += left;
			}

			//tilt right
			if (Keyboard.Down(Key.Right)) {
				var right = TransformComponent.Orientation.Rotate(new Vector3d(Time.DeltaTime, 0.0, 0.0));
				MoveComponent.AngularVelocity += right;
			}


			if (Keyboard.Pressed(Key.C)) {
				MoveComponent.LinearVelocity = Vector3d.Zero;
				MoveComponent.AngularVelocity = Vector3d.Zero;
			}


			if (Keyboard.Down(Key.B)) {
				if (MoveComponent.LinearVelocity.Length > 0.05) {
					MoveComponent.LinearVelocity *= 1 - Time.DeltaTime;
				}
				else {
					MoveComponent.LinearVelocity = Vector3d.Zero;
				}

				if (MoveComponent.AngularVelocity.Length > 0.05) {
					MoveComponent.AngularVelocity *= 1 - Time.DeltaTime;
				}
				else {
					MoveComponent.AngularVelocity = Vector3d.Zero;
				}
			}

			//Console.Out.WriteLine(TransformComponent.Position.ToString());

			MoveComponent.Update();
			base.Update();
			RenderComponent.Update();
			CameraComponent.Update();
			
			TextRenderer2D.RegisterHUDElement($"SPEED: {MoveComponent.LinearVelocity.LengthFast.ToString("N2")}M/S", new Vector2(-1f, -0.94f));
		}

		public override void Draw() {
			base.Draw();
			RenderComponent.Draw(16.0f);
		}
	}
}