using System;
using System.Collections.Generic;
using Engine;
using Engine.Texture;
using Engine.Material;
using Engine.Model;
using Engine.Util;
using Game.GameObjects;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game.Window {
	internal class AppStarter : GameWindow {
		private ModelLoaderObject3D _tennisArenaObject;

		private int _tennisArenaTexture;
		private int _shipTexture;

		private AmbientDiffuseSpecularMaterial _ambientDiffuseMaterial;


		private SpaceShip _ship;
		private Camera _camera;

		private AppStarter()
			: base(1280, 720, new GraphicsMode(32, 24, 8, 2), "CGI-MIN Example", GameWindowFlags.Default, DisplayDevice.Default,
				3, 0, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug) { }


		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			DisplayCamera.Init();
			DisplayCamera.SetWidthHeightFov(800, 600, 90);

			_camera = new Camera();
			DisplayCamera.SetActiveCamera(_camera);

			Light.SetDirectionalLight(new Vector3(0.5f, 1f, 0), new Vector4(0.1f, 0.1f, 0.1f, 0), new Vector4(1, 1, 1, 0),
				new Vector4());

			var shipModel = new ModelLoaderObject3D("data/objects/SpaceShip.obj");
			_ship = new SpaceShip(shipModel);
			_ship.TransformComponent.Position = new Vector3(0.0f, 0.2f, 0.0f);

			_tennisArenaObject = new ModelLoaderObject3D("data/objects/tennis_arena.obj");

			_tennisArenaTexture = TextureManager.LoadTexture("data/textures/tennis_field.png");
			_shipTexture = TextureManager.LoadTexture("data/textures/test.png");

			_ambientDiffuseMaterial = new AmbientDiffuseSpecularMaterial();

			GL.Enable(EnableCap.DepthTest);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Front);
		}

		private bool _f11Pressed;

		protected override void OnUpdateFrame(FrameEventArgs e) {
			if (Keyboard[Key.Escape])
				Exit();

			if (Keyboard[Key.F11])
				_f11Pressed = true;

			if (_f11Pressed && !Keyboard[Key.F11]) {
				WindowState = WindowState != WindowState.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;
				_f11Pressed = false;
			}

			_ship.Update(e.Time, Keyboard);

			var eye = new Vector3(-0.3f, 0.05f, 0.0f);
			Math3D.Rotate(ref eye, _ship.TransformComponent.Orientation);

			_camera.TransformComponent.Position = new Vector3(_ship.TransformComponent.Position.X,
				                                      _ship.TransformComponent.Position.Y,
				                                      _ship.TransformComponent.Position.Z) + eye;

			_camera.SetLookAt(_camera.TransformComponent.Position, _ship.TransformComponent.Position, Vector3.UnitY);

			_camera.Update(e.Time, Keyboard);
		}


		protected override void OnRenderFrame(FrameEventArgs e) {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			_ambientDiffuseMaterial.Draw(_tennisArenaObject, _tennisArenaTexture, 0.2f);
			_ship.Draw(_ambientDiffuseMaterial, _shipTexture);

			SwapBuffers();
		}

		protected override void OnUnload(EventArgs e) { }

		protected override void OnResize(EventArgs e) {
			GL.Viewport(0, 0, Width, Height);
			DisplayCamera.SetWidthHeightFov(Width, Height, 75);
		}

		[STAThread]
		public static void Main() {
			using (var example = new AppStarter()) {
				example.Run(60.0, 60.0);
			}
		}
	}
}