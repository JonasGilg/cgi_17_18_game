using System;
using System.Diagnostics;
using Engine;
using Engine.Texture;
using Engine.Material;
using Engine.Util;
using Game.GameObjects;
using Game.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using EngineKeyboard = Engine.Input.Keyboard;
using EngineMouse = Engine.Input.Mouse;

namespace Game.Window {
	internal class AppStarter : GameWindow {
		private readonly World _world;

		private AppStarter() : base(1280, 720, new GraphicsMode(32, 24, 8, 2), "Space Game", GameWindowFlags.Default,
			DisplayDevice.Default,
			3, 0, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug) {
			_world = new World();
		}

		protected override void OnLoad(EventArgs e) {
			Console.Out.WriteLine("################################\n" +
			                      "#  Controls:                   #\n" +
			                      "#    move forward ..... W      #\n" +
			                      "#    move backward .... S      #\n" +
			                      "#    move left ........ A      #\n" +
			                      "#    move right ....... D      #\n" +
			                      "#    move up .......... SPACE  #\n" +
			                      "#    move down ........ X      #\n" +
			                      "#    rotate left ...... Q      #\n" +
			                      "#    rotate right ..... E      #\n" +
			                      "#    roll left ........ LEFT   #\n" +
			                      "#    roll right ....... RIGHT  #\n" +
			                      "#    roll forward ..... UP     #\n" +
			                      "#    roll backward .... DOWN   #\n" +
			                      "################################");
			base.OnLoad(e);

			MaterialManager.Init();

			DisplayCamera.Init();
			DisplayCamera.SetWidthHeightFov(800, 600, 90);

			Light.SetDirectionalLight(new Vector3(0f, 0f, 1f),
				//r      g      b      a
				new Vector4(.011f, .011f, .011f, 0f),
				new Vector4(.050f, .200f, .700f, 0f),
				new Vector4(.050f, .050f, .100f, 0f));

			//+++++++++++++++++++++++++SPACESHIP+++++++++++++++++++++++++

			for (var i = 0; i < 1; i++) {
				var asteroid = AsteroidFactory.GenerateAsteroid();
				asteroid.TransformComponent.Position = new Vector3(i * 15f, 0.0f, 10.0f);
				asteroid.TransformComponent.Scale = new Vector3(1.0f);
				asteroid.MoveComponent.AngularVelocity = Vector3.UnitX;

				_world.AddToWorld(asteroid);
			}

			var ship = new SpaceShip();
			ship.TransformComponent.Scale = new Vector3(0.02f);
			ship.TransformComponent.Position = new Vector3(-5f, 0f, -5.0f);
			ship.TransformComponent.Orientation = Quaternion.FromAxisAngle(Vector3.UnitY, (float) -(Math.PI / 2));

			_world.AddToWorld(ship);

			//+++++++++++++++++++++++++NEPTUNE+++++++++++++++++++++++++

			var neptune = new Planet(TextureManager.LoadTexture("data/textures/neptunemap.jpg")) {
				TransformComponent = {
					Scale = new Vector3(400f),
					Position = new Vector3(0, 0, 1500f)
				}
			};

			_world.AddToWorld(neptune);

			GL.Enable(EnableCap.DepthTest);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Front);
		}

		protected override void OnUpdateFrame(FrameEventArgs e) {
			EngineKeyboard.Update(Keyboard.GetState());
			EngineMouse.Update(Mouse.GetState());
			Time.Update(e.Time);

			if (EngineKeyboard.Released(Key.Escape))
				Exit();

			if (EngineKeyboard.Released(Key.F11)) {
				WindowState = WindowState != WindowState.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;
			}

			_world.UpdateWorld();
		}

		protected override void OnRenderFrame(FrameEventArgs e) {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			_world.RenderWorld();
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