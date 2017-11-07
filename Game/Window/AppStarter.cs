using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using Engine;
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

		private AppStarter() : base(1280, 720, new GraphicsMode(32, 24, 8, 2), "Space Game", GameWindowFlags.Default,
			DisplayDevice.Default,
			3, 0, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug) {
			
		}

		protected override void OnLoad(EventArgs e) {
			Console.Out.WriteLine("################################\n" +
			                      "#  Controls:                   #\n" +
			                      "#    move forward ..... W      #\n" +
			                      "#    move backward .... S      #\n" +
			                      "#    move left ........ Q      #\n" +
			                      "#    move right ....... E      #\n" +
			                      "#    move up .......... SPACE  #\n" +
			                      "#    move down ........ X      #\n" +
			                      "#    rotate left ...... A      #\n" +
			                      "#    rotate right ..... D      #\n" +
			                      "#    roll left ........ LEFT   #\n" +
			                      "#    roll right ....... RIGHT  #\n" +
			                      "#    roll forward ..... UP     #\n" +
			                      "#    roll backward .... DOWN   #\n" +
			                      "################################");
			base.OnLoad(e);
			
			MaterialManager.Init();

			DisplayCamera.Init();
			DisplayCamera.SetWidthHeightFov(800, 600, 75);

			Light.SetDirectionalLight(new Vector3(0f, 0f, 1f),
				//r      g      b      a
				new Vector4(.021f, .011f, .011f, 0f),
				new Vector4(.050f, .200f, .700f, 0f),
				new Vector4(.050f, .050f, .100f, 0f));

			//sun
			var sun = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.Sun, new Vector3d(0, 0, 0), new Vector3d(2000.0),
				new Vector3d(0, 0.1, 0));
			World.AddToWorld(sun);

			for (var i = 1; i < 3; i++) {
				var planet = PlanetFactory.generatePlanetWithAsteroidBeld((PlanetFactory.PlanetTexture) i,AsteroidFactory.AsteroidType.STRAWBERRY,3, new Vector3d(10000.0 * i, 0, 0),
					new Vector3d(1000.0), new Vector3d(0, 0.5, 0));
				World.AddToWorld(planet);
			}

			var ship = new SpaceShip();
			ship.TransformComponent.Scale = new Vector3d(0.02f);
			ship.TransformComponent.Position = new Vector3d(0f, 0f, -2000.0f);
			ship.TransformComponent.Orientation = Quaterniond.FromAxisAngle(Vector3d.UnitY, -(Math.PI / 2));

			World.AddToWorld(ship);

			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Less);
			
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

			World.UpdateWorld();
		}

		protected override void OnRenderFrame(FrameEventArgs e) {
			GL.Clear(ClearBufferMask.DepthBufferBit);
			World.RenderWorld();
			TextRenderer2D.DrawString(((int) (1 / e.Time)).ToString(), new Vector2(-1f, 1f));
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