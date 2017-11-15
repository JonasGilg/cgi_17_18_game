﻿using System;
using Engine;
using Engine.GUI;
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
		private readonly Vector3d startingPoint = new Vector3d(7000.0, 0.0, -1000.0);

		private AppStarter() : base(1600, 900, new GraphicsMode(32, 24, 8, 2), "Space Game", GameWindowFlags.Default,
			DisplayDevice.Default,
			3, 0, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug) { }

		private readonly HUDElement upsCounter = HUD.CreateHUDElement("", new Vector2(-1f, 1f));
		private readonly HUDElement fpsCounter = HUD.CreateHUDElement("", new Vector2(-1f, 0.94f));

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			HUD.AddHUDElement(upsCounter);
			HUD.AddHUDElement(fpsCounter);

			DisplayCamera.SetWidthHeightFov(Width, Height, 75);

			Light.SetSpotLight(new Vector3d(0f, 0f, 0f),
				//           r      g      b      a
				new Vector4(.010f, .010f, .010f, 0f),
				new Vector4(.950f, .950f, .950f, 0f),
				new Vector4(.950f, .950f, .950f, 0f));

			//sun
			var sun = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(0, 0, 0), new Vector3d(2000.0),
				new Vector3d(0, 0.1, 0));
			sun.RenderComponent.Material = MaterialManager.GetMaterial(Material.SIMPLE);
			World.AddToWorld(sun);


			for (var i = 1; i < 3; i++) {
				var planet = PlanetFactory.GeneratePlanetWithAsteroidBeld((PlanetFactory.PlanetTexture) i,
					AsteroidFactory.AsteroidType.STANDARD, 30, new Vector3d(10000.0 * i, 0, 0),
					new Vector3d(1000.0), new Vector3d(0, 0.5, 0), sun);
				World.AddToWorld(planet);
			}

			var ship = new SpaceShip {
				TransformComponent = {
					Scale = new Vector3d(0.02f),
					Position = startingPoint,
					Orientation = Quaterniond.FromAxisAngle(Vector3d.UnitY, 0)
				}
			};
			World.AddToWorld(ship);

			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Less);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Front);
			GL.ClearColor(1f, 0f, 0f, 0f);
		}

		protected override void OnUpdateFrame(FrameEventArgs e) {
			EngineKeyboard.Update(Keyboard.GetState());
			EngineMouse.Update(Mouse.GetState());
			Time.UpdateUpdateTime(e.Time);

			upsCounter.Text = $"{((int) (1 / Time.AverageUpdateTime())).ToString()}UPS";

			if (EngineKeyboard.Released(Key.Escape)) {
				Exit();
			}

			if (EngineKeyboard.Released(Key.F11)) {
				WindowState = WindowState != WindowState.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;
			}

			if (EngineKeyboard.Released(Key.Period)) {
				Time.IncreaseGameSpeed();
				Console.Out.WriteLine($"{Time.GameSpeed.ToString("N2")}");
			}

			if (EngineKeyboard.Released(Key.Comma)) {
				Time.DecreaseGameSpeed();
				Console.Out.WriteLine($"{Time.GameSpeed.ToString("N2")}");
			}

#if(DEBUG)
//Console.Out.WriteLine(TimingRegistry.GetStatsText());
#endif

			World.UpdateWorld();
		}


		protected override void OnRenderFrame(FrameEventArgs e) {
			Time.UpdateRenderTime(e.Time);

			fpsCounter.Text = ((int) (1 / Time.AverageRenderTime())).ToString() + "FPS";

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
				example.Run(200, 200);
			}
		}
	}
}