using System;
using System.Collections.Generic;
using Engine.Texture;
using Engine.Camera;
using Engine.Light;
using Engine.Material.Ambientdiffuse;
using Engine.Material.Simpletexture;
using Engine.Model;
using Engine.Util;
using Game.GameObjects;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game.Window {
	internal class AppStarter : GameWindow {
		// Constants
		private const float BallRadius = 0.01f;

		private const float FieldXBorder = 2.65835f;
		private const float FieldZBorder = 1.39379f;
		private const float Gravity = 0.0004f;
		private const float EnergyLossOnBottom = 1.01f;

		// the objects we load
		private ModelLoaderObject3D _tennisBallObject;

		private ModelLoaderObject3D _tennisArenaObject;

		// our textur-IDs
		private int _tennisBallTexture;

		private int _tennisArenaTexture;
		private int _shadowTexture;

		// Materials
		private AmbientDiffuseMaterial _ambientDiffuseMaterial;

		private SimpleTextureMaterial _simpleTextureMaterial;

		// the ball coordinates
		private float _ballPositionX;

		private float _ballPositionY;
		private float _ballPositionZ;

		private float _ballDirectionX;
		private float _ballDirectionZ;
		private float _ballYVelocity;

		private int _updateCounter;

		private Queue<Quaternion> _shipOrientation = new Queue<Quaternion>();

		private SpaceShip _ship;

		private AppStarter()
			: base(1280, 720, new GraphicsMode(32, 24, 8, 2), "CGI-MIN Example", GameWindowFlags.Default, DisplayDevice.Default,
				3, 0, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug) { }


		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			// Initialize Camera
			Camera.Init();
			Camera.SetWidthHeightFov(800, 600, 90);

			// Initialize Light
			Light.SetDirectionalLight(new Vector3(0.5f, 1, 0), new Vector4(0.1f, 0.1f, 0.1f, 0), new Vector4(1, 1, 1, 0));

			var shipModel = new ModelLoaderObject3D("data/objects/monkey.obj");
			_ship = new SpaceShip(shipModel);
			_ship.TransformComponent.Position = new Vector3(0.0f, 0.2f, 0.0f);

			for (var i = 0; i < 10; i++) {
				_shipOrientation.Enqueue(_ship.TransformComponent.Orientation);
			}

			// Loading the object
			_tennisBallObject = new ModelLoaderObject3D("data/objects/tennis_ball.obj");
			_tennisArenaObject = new ModelLoaderObject3D("data/objects/tennis_arena.obj");

			// Loading the textures
			_tennisBallTexture = TextureManager.LoadTexture("data/textures/tennis_ball.png");
			_tennisArenaTexture = TextureManager.LoadTexture("data/textures/tennis_field.png");
			_shadowTexture = TextureManager.LoadTexture("data/textures/shadow_color.png");

			// initialize material
			_ambientDiffuseMaterial = new AmbientDiffuseMaterial();
			_simpleTextureMaterial = new SimpleTextureMaterial();

			// enebale z-buffer
			GL.Enable(EnableCap.DepthTest);

			// backface culling enabled
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Front);

			// initial ball values
			_ballPositionX = 0.0f;
			_ballPositionY = 0.5f;
			_ballPositionZ = 0.0f;

			// the initial direction
			_ballDirectionX = 0.02f;
			_ballDirectionZ = 0.01f;
		}

		private bool _f11Pressed;

		protected override void OnUpdateFrame(FrameEventArgs e) {
			if (Keyboard[Key.Escape])
				Exit();

			if (Keyboard[Key.F11])
				_f11Pressed = true;

			//released
			if (_f11Pressed && !Keyboard[Key.F11]) {
				WindowState = WindowState != WindowState.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;
				_f11Pressed = false;
			}

			// updateCounter simply increaes
			_updateCounter++;

			// ---------------------------------------------
			// update the ball (fake and simplified physics)
			// ---------------------------------------------

			_ship.Update(e.Time, Keyboard);

			// first the x and z position
			_ballPositionX += _ballDirectionX;
			_ballPositionZ += _ballDirectionZ;

			if (_ballPositionX > FieldXBorder - BallRadius) _ballDirectionX = -Math.Abs(_ballDirectionX);
			if (_ballPositionX < -FieldXBorder + BallRadius) _ballDirectionX = Math.Abs(_ballDirectionX);

			if (_ballPositionZ > FieldZBorder - BallRadius) _ballDirectionZ = -Math.Abs(_ballDirectionZ);
			if (_ballPositionZ < -FieldZBorder + BallRadius) _ballDirectionZ = Math.Abs(_ballDirectionZ);

			// y-position affected by gravity
			_ballPositionY -= _ballYVelocity;
			_ballYVelocity += Gravity;

			if (_ballPositionY < BallRadius) {
				_ballYVelocity =
					-Math.Abs(_ballYVelocity) *
					EnergyLossOnBottom; // velocity always moving ball up, some kinetic energy lost so multiplied by ENERGY_LOSS_ON_BOTTOM 
				_ballPositionY = BallRadius;
			}

			
			var eye = new Vector3(0.0f, 0.05f, -0.3f);
			Math3D.Rotate(ref eye, _ship.TransformComponent.Orientation);

			Camera.SetLookAt(
				new Vector3(_ship.TransformComponent.Position.X,
					_ship.TransformComponent.Position.Y,
					_ship.TransformComponent.Position.Z) + eye,
				_ship.TransformComponent.Position,
				Vector3.UnitY);
		}


		protected override void OnRenderFrame(FrameEventArgs e) {
			// the screen and the depth-buffer are cleared
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			// ----------------------------------------------------------------------
			// draw the arena
			// ----------------------------------------------------------------------
			_ambientDiffuseMaterial.Draw(_tennisArenaObject, _tennisArenaTexture);

			// ----------------------------------------------------------------------
			// calculate ball's transformation matrix and draw the ball
			// ----------------------------------------------------------------------

			// reset the ball's transformation matrix
			_tennisBallObject.Transformation = Matrix4.Identity;

			// first scale the ball's matrix
			_tennisBallObject.Transformation *= Matrix4.CreateScale(BallRadius, BallRadius, BallRadius);

			// rotation of object, around x-axis
			_tennisBallObject.Transformation *= Matrix4.CreateRotationX(_updateCounter / 20.0f);

			// around y-axis
			_tennisBallObject.Transformation *= Matrix4.CreateRotationY(_updateCounter / 10.0f);

			// set the balls translation
			_tennisBallObject.Transformation *= Matrix4.CreateTranslation(_ballPositionX, _ballPositionY, _ballPositionZ);

			// draw the ball
			_ambientDiffuseMaterial.Draw(_tennisBallObject, _tennisBallTexture);

			_ship.Draw(_ambientDiffuseMaterial, _tennisArenaTexture);

			// ----------------------------------------------------------------------
			// calculate shadow matrix unsing the ball object to draw a fake shadow
			// ----------------------------------------------------------------------

			// reset the ball shadows transformation matrix
			_tennisBallObject.Transformation = Matrix4.Identity;

			// first scale the ball shadow matrix, y is 0 so the ball is flat
			_tennisBallObject.Transformation *= Matrix4.CreateScale(BallRadius, 0, BallRadius);

			// set the ball shadows translation, y is constantly on the bottom (0.001f).
			_tennisBallObject.Transformation *= Matrix4.CreateTranslation(_ballPositionX, 0.001f, _ballPositionZ);

			//tennisBallObject.Transformation *= Matrix4.CreateTranslation(ballPositionX - Light.lightDirection.X * ballPositionY, 0.001f, ballPositionZ - Light.lightDirection.Z * ballPositionY);

			// draw the ball shadow
			_simpleTextureMaterial.Draw(_tennisBallObject, _shadowTexture);


			SwapBuffers();
		}


		protected override void OnUnload(EventArgs e) {
			_tennisBallObject.UnLoad();
		}


		protected override void OnResize(EventArgs e) {
			GL.Viewport(0, 0, Width, Height);
			Camera.SetWidthHeightFov(Width, Height, 60);
		}


		[STAThread]
		public static void Main() {
			using (var example = new AppStarter()) {
				example.Run(60.0, 60.0);
			}
		}
	}
}