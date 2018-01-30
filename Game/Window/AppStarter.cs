﻿using System;
using Engine;
using Engine.GUI;
using Engine.Material;
using Engine.Render;
 using Engine.Texture;
 using Engine.Util;
 using Game.GameObjects;
 using Game.GamePlay;
 using Game.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using EngineKeyboard = Engine.Input.Keyboard;
using EngineMouse = Engine.Input.Mouse;

namespace Game.Window {
	internal class AppStarter : GameWindow {

		private AppStarter() : base(1280, 768, new GraphicsMode(32, 24, 8, 8), "Space Game", GameWindowFlags.Default,
			DisplayDevice.Default,
			3, 0, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug) { }

		private readonly HudTextElement upsCounter = HUD.CreateHudTextElement("", new Vector2(-1f, 1f));
		private readonly HudTextElement fpsCounter = HUD.CreateHudTextElement("", new Vector2(-1f, 0.94f));

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			HUD.AddHudTextElement(upsCounter);
			HUD.AddHudTextElement(fpsCounter);
			HUD.AddHudTextElement(GamePlayEngine.hudLevelIndicatorText);
			
			DisplayCamera.SetWidthHeightFov(Width, Height, 75);
			
			GamePlayEngine.LoadLevel(0);

			RenderEngine.IBLData = new IBLData {
				IrradianceCubeTexture = TextureManager.LoadIBLIrradianceMap("data/textures/IBL/Diffuse_Irradiance/skybox_bright", "png"),
				SpecularCubeTexture = TextureManager.LoadIBLSpecularMap("data/textures/IBL/Specular/skybox_bright", "png")
			};
			
			CascadedShadowMapping.Init(4096, 2048, 1024, 15, 20, 90, 1);
			CascadedShadowMapping.SetLightDirection(new Vector3d(100, 8, 10));
			DeferredRendering.Init(Width, Height);

			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Less);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Front);
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
IO.PrintAsync(TimingRegistry.GetStatsText());
#endif

			World.UpdateWorld();
		}


		protected override void OnRenderFrame(FrameEventArgs e) {
			Time.UpdateRenderTime(e.Time);
			Statistics.UpdateTimeSpent();
			
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
			DeferredRendering.Resize(Width, Height);
		}

		[STAThread]
		public static void Main() {
			using (var example = new AppStarter()) {
				example.Run(200, 200);
			}
		}
	}
}