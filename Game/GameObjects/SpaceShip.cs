using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Util;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.GameObjects {
	public class SpaceShip : GameObject {
		public readonly MoveComponent MoveComponent;
		public readonly CameraComponent CameraComponent;
		
		public SpaceShip() {
			MoveComponent = new MoveComponent(this);
			CameraComponent = new ThirdPersonCameraComponent(new Vector3(-0.3f, 0.05f, 0.0f), this);
			DisplayCamera.SetActiveCamera(CameraComponent);
			
		}

		public override void Update() {
			var deltaTimeF = 0.1f;
			
			//move forward
			if (Keyboard.Down(Key.W)) {
				var forward = new Vector3(deltaTimeF, 0.0f, 0.0f);
				Math3D.Rotate(ref forward, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += forward;
			}
			
			//move backward
			if (Keyboard.Down(Key.S)) {
				var backwards = new Vector3(-deltaTimeF, 0.0f, 0.0f);
				Math3D.Rotate(ref backwards, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += backwards;
			}
			
			//move left
			if (Keyboard.Down(Key.A)) {
				var left = new Vector3(0.0f, 0.0f, -deltaTimeF);
				Math3D.Rotate(ref left, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += left;
			}

			//move right
			if (Keyboard.Down(Key.D)) {
				var right = new Vector3(0.0f, 0.0f, deltaTimeF);
				Math3D.Rotate(ref right, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += right;
			}

			//move up
			if (Keyboard.Down(Key.Space)) {
				var right = new Vector3(0.0f, deltaTimeF, 0.0f);
				Math3D.Rotate(ref right, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += right;
			}
			
			//move down
			if (Keyboard.Down(Key.X)) {
				var right = new Vector3(0.0f, -deltaTimeF, 0.0f);
				Math3D.Rotate(ref right, TransformComponent.Orientation);
				MoveComponent.LinearVelocity += right;
			}
			
			//turn left
			if (Keyboard.Down(Key.Q)) {
				var left = new Vector3(0.0f, deltaTimeF, 0.0f);
				Math3D.Rotate(ref left, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += left;
			}
			
			//turn right
			if (Keyboard.Down(Key.E)) {
				var right = new Vector3(0.0f, -deltaTimeF, 0.0f);
				Math3D.Rotate(ref right, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += right;
			}
			
			//tilt forward
			if (Keyboard.Down(Key.Up)) {
				var forward = new Vector3(0.0f, 0.0f, -deltaTimeF);
				Math3D.Rotate(ref forward, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += forward;
			}
			
			//tilt backward
			if (Keyboard.Down(Key.Down)) {
				var backward = new Vector3(0.0f, 0.0f, deltaTimeF);
				Math3D.Rotate(ref backward, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += backward;
			}
			
			//tilt left
			if (Keyboard.Down(Key.Left)) {
				var left = new Vector3(-deltaTimeF, 0.0f, 0.0f);
				Math3D.Rotate(ref left, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += left;
			}
			
			//tilt right
			if (Keyboard.Down(Key.Right)) {
				var right = new Vector3(deltaTimeF, 0.0f, 0.0f);
				Math3D.Rotate(ref right, TransformComponent.Orientation);
				MoveComponent.AngularVelocity += right;
			}

			
			if (Keyboard.Pressed(Key.C)) {
				MoveComponent.LinearVelocity = Vector3.Zero;
				MoveComponent.AngularVelocity = Vector3.Zero;
			}

			
			if (Keyboard.Down(Key.B)) {
				if (MoveComponent.LinearVelocity.Length > 0.05f) {
					MoveComponent.LinearVelocity *= 1 - deltaTimeF;
				}
				else {
					MoveComponent.LinearVelocity = Vector3.Zero;
				}

				if (MoveComponent.AngularVelocity.Length > 0.05f) {
					MoveComponent.AngularVelocity *= 1 - deltaTimeF;
				}
				else {
					MoveComponent.AngularVelocity = Vector3.Zero;
				}
			}
			
			MoveComponent.Update();
			base.Update();
			Renderer?.Material.model3D.Update(TransformComponent.WorldMatrix);
			CameraComponent.Update();
		}
		
	}
}