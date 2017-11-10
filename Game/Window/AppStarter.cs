using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using Engine;
using Engine.Component;
using Engine.GUI;
using Engine.Material;
using Engine.Util;
using Game.Components;
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
		private AppStarter() : base(1600, 900, new GraphicsMode(32, 24, 8, 2), "Space Game", GameWindowFlags.Default,
			DisplayDevice.Default,
			3, 0, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug) { }

		private readonly HUDElement _upsCounter = HUD.CreateHUDElement("", new Vector2(-1f, 1f));
		private readonly HUDElement _fpsCounter = HUD.CreateHUDElement("", new Vector2(-1f, 0.94f));

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			Time.initialize();
			HUD.AddHUDElement(_upsCounter);
			HUD.AddHUDElement(_fpsCounter);

			DisplayCamera.SetWidthHeightFov(Width, Height, 75);

			Light.SetSpotLight(new Vector3d(0f, 0f, 0f),
				//           r      g      b      a
				new Vector4(.021f, .011f, .011f, 0f),
				new Vector4(.950f, .950f, .950f, 0f),
				new Vector4(.950f, .950f, .950f, 0f));

			//sun
			var sun = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.Sun, new Vector3d(0, 0, 0), new Vector3d(2000.0),
				new Vector3d(0, 0.1, 0));
			World.AddToWorld(sun);

			sun.RenderComponent.Material = MaterialManager.GetMaterial(Material.Simple);

			for (var i = 1; i < 3; i++) {
				var planet = PlanetFactory.GeneratePlanetWithAsteroidBeld((PlanetFactory.PlanetTexture) i,
					AsteroidFactory.AsteroidType.STRAWBERRY, 1, new Vector3d(10000.0 * i, 0, 0),
					new Vector3d(1000.0), new Vector3d(0, 0.5, 0));
				World.AddToWorld(planet);
			}

			var ship = new SpaceShip {
				TransformComponent = {
					Scale = new Vector3d(0.02f),
					Position = new Vector3d(0f, 0f, -2000.0f),
					Orientation = Quaterniond.FromAxisAngle(Vector3d.UnitY, 0)
				}
			};

			World.AddToWorld(ship,ship.CollisionComponent);

			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Less);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Front);
		}

		protected override void OnUpdateFrame(FrameEventArgs e) {
			EngineKeyboard.Update(Keyboard.GetState());
			EngineMouse.Update(Mouse.GetState());
			Time.UpdateUpdateTime(e.Time);
			
			_upsCounter.Text = ((int) (1 / Time.AverageUpdateTime())).ToString() + "UPS";

			if (EngineKeyboard.Released(Key.Escape))
				Exit();

			if (EngineKeyboard.Released(Key.F11)) {
				WindowState = WindowState != WindowState.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;
			}

#if(DEBUG)
			//Console.Out.WriteLine(TimingRegistry.GetStatsText());
#endif

			World.UpdateWorld();
		}


		protected override void OnRenderFrame(FrameEventArgs e) {
			Time.UpdateRenderTime(e.Time);
			
			_fpsCounter.Text = ((int) (1 / Time.AverageRenderTime())).ToString() + "FPS";
			
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.StencilBufferBit | ClearBufferMask.DepthBufferBit);

			World.RenderWorld();

			HUD.Draw();
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
				example.Run(200);
			}
		}
	}
}