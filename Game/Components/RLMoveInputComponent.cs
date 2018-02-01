using Engine;
using Engine.Component;
using Engine.Render;
using Engine.Util;
using OpenTK;
using OpenTK.Input;
using Keyboard = Engine.Input.Keyboard;

namespace Game.Components {
	public class RLSpaceMovementComponent : MoveInputComponent {
		public RLSpaceMovementComponent(GameObject gameObject, TransformComponent transformComponent,
			MoveComponent moveComponent) : base(gameObject, transformComponent, moveComponent) { }

		public override void Update() {
			base.Update();
			
			var linearVelocity = GetLinearVelocityInObjectSpace();

			if (Keyboard.Down(Key.W))
				AddLinearVelocityInObjectSpace(new Vector3d(Time.DeltaTimeUpdate, 0, 0), MOVEMENT_MULTIPLIER);
			else if (Keyboard.Down(Key.S))
				AddLinearVelocityInObjectSpace(new Vector3d(-Time.DeltaTimeUpdate, 0, 0), 500);
			else if (linearVelocity.X > 0)
				AddLinearVelocityInObjectSpace(new Vector3d(-Time.DeltaTimeUpdate, 0, 0), 500);
			else if (linearVelocity.X < 0)
				AddLinearVelocityInObjectSpace(new Vector3d(Time.DeltaTimeUpdate, 0, 0), 500);

			if (Keyboard.Down(Key.Q))
				AddLinearVelocityInObjectSpace(new Vector3d(0.0, 0.0, -Time.DeltaTimeUpdate), 500);
			else if (Keyboard.Down(Key.E))
				AddLinearVelocityInObjectSpace(new Vector3d(0.0, 0.0, Time.DeltaTimeUpdate), 500);
			else if (linearVelocity.Z < 0)
				AddLinearVelocityInObjectSpace(new Vector3d(0.0, 0.0, Time.DeltaTimeUpdate), 500);
			else if (linearVelocity.Z > 0)
				AddLinearVelocityInObjectSpace(new Vector3d(0.0, 0.0, -Time.DeltaTimeUpdate), 500);

			if (Keyboard.Down(Key.Space))
				AddLinearVelocityInObjectSpace(new Vector3d(0.0, Time.DeltaTimeUpdate, 0.0), 500);
			else if (Keyboard.Down(Key.X))
				AddLinearVelocityInObjectSpace(new Vector3d(0.0, -Time.DeltaTimeUpdate, 0.0), 500);
			else if (linearVelocity.Y > 0)
				AddLinearVelocityInObjectSpace(new Vector3d(0.0, -Time.DeltaTimeUpdate, 0.0), 500);
			else if (linearVelocity.Y < 0)
				AddLinearVelocityInObjectSpace(new Vector3d(0.0, Time.DeltaTimeUpdate, 0.0), 500);

			MoveComponent.LinearVelocity = Vector3d.Lerp(MoveComponent.LinearVelocity, Vector3d.Zero, Time.DeltaTimeUpdate);

			var angularVelocity = GetAngularVelocityInObjectSpace();

			if (Keyboard.Down(Key.A))
				AddAngularVelocityInObjectSpace(new Vector3d(0.0, Time.DeltaTimeUpdate, 0.0), ROTATION_MULTIPLIER);
			else if (Keyboard.Down(Key.D))
				AddAngularVelocityInObjectSpace(new Vector3d(0.0, -Time.DeltaTimeUpdate, 0.0), ROTATION_MULTIPLIER);
			else if (angularVelocity.Y > 0)
				AddAngularVelocityInObjectSpace(new Vector3d(0.0, -Time.DeltaTimeUpdate, 0.0), ROTATION_MULTIPLIER);
			else if (angularVelocity.Y < 0)
				AddAngularVelocityInObjectSpace(new Vector3d(0.0, Time.DeltaTimeUpdate, 0.0), ROTATION_MULTIPLIER);

			if (Keyboard.Down(Key.Up))
				AddAngularVelocityInObjectSpace(new Vector3d(0.0, 0.0, -Time.DeltaTimeUpdate), ROTATION_MULTIPLIER);
			else if (Keyboard.Down(Key.Down))
				AddAngularVelocityInObjectSpace(new Vector3d(0.0, 0.0, Time.DeltaTimeUpdate), ROTATION_MULTIPLIER);
			else if (angularVelocity.Z < 0)
				AddAngularVelocityInObjectSpace(new Vector3d(0.0, 0.0, Time.DeltaTimeUpdate), ROTATION_MULTIPLIER);
			else if (angularVelocity.Z > 0)
				AddAngularVelocityInObjectSpace(new Vector3d(0.0, 0.0, -Time.DeltaTimeUpdate), ROTATION_MULTIPLIER);
				
			if (Keyboard.Down(Key.Left))
				AddAngularVelocityInObjectSpace(new Vector3d(-Time.DeltaTimeUpdate, 0.0, 0.0), ROTATION_MULTIPLIER);
			else if (Keyboard.Down(Key.Right))
				AddAngularVelocityInObjectSpace(new Vector3d(Time.DeltaTimeUpdate, 0.0, 0.0), ROTATION_MULTIPLIER);
			else if(angularVelocity.X < 0)
				AddAngularVelocityInObjectSpace(new Vector3d(Time.DeltaTimeUpdate, 0.0, 0.0), ROTATION_MULTIPLIER);
			else if(angularVelocity.X > 0)
				AddAngularVelocityInObjectSpace(new Vector3d(-Time.DeltaTimeUpdate, 0.0, 0.0), ROTATION_MULTIPLIER);
				
			MoveComponent.AngularVelocity = Vector3d.Lerp(MoveComponent.AngularVelocity, Vector3d.Zero, Time.DeltaTimeUpdate);
		}
	}
}