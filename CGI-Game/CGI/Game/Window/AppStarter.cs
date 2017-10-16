using System;
using CGI.Engine.Light;
using CGI.Engine.Material.Ambientdiffuse;
using CGI.Engine.Material.Simpletexture;
using CGI.Engine.Object3D;
using CGI.Engine.Camera;
using CGI.Engine.GameObject;
using CGI.Engine.Texture;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace CGI.Game.Window {
	internal class AppStarter : GameWindow {
		private enum CameraMode {
			Corner,
			Net,
			TopView,
			AroundBall
		}

		// camera mode
		private CameraMode _cameraMode;

		// Constants
		private const float BallRadius = 0.01f;

		private const float FieldXBorder = 2.65835f;
		private const float FieldZBorder = 1.39379f;
		private const float Gravity = 0.0004f;
		private const float EnergyLossOnBottom = 1.01f;

		// the objects we load
		private ObjLoaderObject3D _tennisBallObject;

		private ObjLoaderObject3D _tennisArenaObject;

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

		private AppStarter()
			: base(1280, 720, new GraphicsMode(32, 24, 8, 2), "CGI-MIN Example", GameWindowFlags.Default, DisplayDevice.Default,
				3, 0, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug) { }


		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			// Initialize Camera
			Camera.Init();
			Camera.SetWidthHeightFov(800, 600, 60);

			// Initialize Light
			Light.SetDirectionalLight(new Vector3(0.5f, 1, 0), new Vector4(0.1f, 0.1f, 0.1f, 0), new Vector4(1, 1, 1, 0));

			// Loading the object
			_tennisBallObject = new ObjLoaderObject3D("data/objects/tennis_ball.obj");
			_tennisArenaObject = new ObjLoaderObject3D("data/objects/tennis_arena.obj");

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

			// initial camera
			_cameraMode = CameraMode.AroundBall;
		}


		protected override void OnUpdateFrame(FrameEventArgs e) {
			if (Keyboard[Key.Escape])
				Exit();

			if (Keyboard[Key.F11])
				WindowState = WindowState != WindowState.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;

			// updateCounter simply increaes
			_updateCounter++;

			if (Keyboard[Key.Number1]) _cameraMode = CameraMode.Corner;
			if (Keyboard[Key.Number2]) _cameraMode = CameraMode.TopView;
			if (Keyboard[Key.Number3]) _cameraMode = CameraMode.Net;
			if (Keyboard[Key.Number4]) _cameraMode = CameraMode.AroundBall;


			// ---------------------------------------------
			// update the ball (fake and simplified physics)
			// ---------------------------------------------

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


			// ---------------------------------------------
			// set the camera, depending on state
			// ---------------------------------------------
			switch (_cameraMode) {
				case CameraMode.Corner:
					Camera.SetLookAt(new Vector3(1, 1, 1), new Vector3(_ballPositionX, _ballPositionY, _ballPositionZ), Vector3.UnitY);
					break;

				case CameraMode.TopView:
					Camera.SetLookAt(new Vector3(0, 3, 0), new Vector3(0, 0, 0), Vector3.UnitZ);
					break;

				case CameraMode.Net:
					Camera.SetLookAt(new Vector3(0, 1, 2), new Vector3(_ballPositionX, _ballPositionY, _ballPositionZ), Vector3.UnitY);
					break;

				case CameraMode.AroundBall:
					Camera.SetLookAt(
						new Vector3(_ballPositionX + (float) Math.Sin(_updateCounter * 0.01f) * 0.2f, _ballPositionY,
							_ballPositionZ + (float) Math.Cos(_updateCounter * 0.01f) * 0.2f),
						new Vector3(_ballPositionX, _ballPositionY, _ballPositionZ), Vector3.UnitY);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
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