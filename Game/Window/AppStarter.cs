using System;
using System.Collections.Generic;
using Engine;
using Engine.Texture;
using Engine.Material;
using Engine.Model;
using Engine.Util;
using Game.GameObjects;
using Game.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using KB = Engine.Input.Keyboard;

namespace Game.Window {
	internal class AppStarter : GameWindow {
		private Model3D _neptuneObject;

		private int _shipTexture;
		private int _asteroidTexture;
		private int _neptuneTexture;

		private AmbientDiffuseSpecularMaterial _ambientDiffuseSpecularMaterial;
		private SimpleTextureMaterial _simpleTextureMaterial;

		private SpaceShip _ship;
		private Asteroid _asteroid;
		
		private List<Asteroid> _asteroids = new List<Asteroid>();

		private AppStarter()
			: base(1280, 720, new GraphicsMode(32, 24, 8, 2), "CGI-MIN Example", GameWindowFlags.Default, DisplayDevice.Default,
				3, 0, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug) { }


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

			DisplayCamera.Init();
			DisplayCamera.SetWidthHeightFov(800, 600, 90);

			Light.SetDirectionalLight(new Vector3(0f, 0f, 1f),
						   //r      g      b      a
				new Vector4(0.15f, 0.15f, 0.15f, 0.0f),
				new Vector4(0.05f, 0.20f, 0.60f, 0.0f),
				new Vector4(0.05f, 0.10f, 0.40f, 0.0f));

			var shipModel = new ModelLoaderObject3D("data/objects/SpaceShip.obj");
			_ship = new SpaceShip(shipModel);
			_ship.TransformComponent.Position = new Vector3(-5f, 0f, -5.0f);
			_ship.TransformComponent.Orientation = Quaternion.FromAxisAngle(Vector3.UnitY, -1.0f);

			_neptuneObject = new ModelLoaderObject3D("data/objects/neptune.obj") {
				Transformation = Matrix4.CreateScale(new Vector3(400f)) * Matrix4.CreateTranslation(0, 0, 1500f)
			};


			_shipTexture = TextureManager.LoadTexture("data/textures/test.png");
			_neptuneTexture = TextureManager.LoadTexture("data/textures/neptunemap.jpg");

			//var asteroid0Model = new ModelLoaderObject3D("data/objects/asteroids/asteroid_1.obj");
			//_asteroid = new Asteroid(asteroid0Model,new Vector3(1.0f,0.4f,0.1f),new Vector3(0.0f,0.0f,0.0f),new Vector3(1.0f)  );
			for (int i = 0; i < 16; i++) {
				_asteroid = AsteroidFactory.generateAsteroid();
				_asteroid.TransformComponent.Position = new Vector3(i*10f, 0.4f, 0.0f);
				_asteroid.MoveComponent.AngularVelocity = new Vector3(1.0f,0.5f,0.2f);
				_asteroids.Add(_asteroid);
			}
			
			
			
			
			_asteroidTexture = TextureManager.LoadTexture("data/textures/asteroids/asteroid_0.png");

			_ambientDiffuseSpecularMaterial = new AmbientDiffuseSpecularMaterial();
			_simpleTextureMaterial = new SimpleTextureMaterial();

			GL.Enable(EnableCap.DepthTest);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Front);
		}

		protected override void OnUpdateFrame(FrameEventArgs e) {
			KB.Update(Keyboard.GetState());
			
			if (KB.Released(Key.Escape))
				Exit();

			if (KB.Released(Key.F11)) {
				WindowState = WindowState != WindowState.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;
			}

			_ship.Update(e.Time);
			_asteroids.ForEach(asteroid => {
				asteroid.Update(e.Time);
			});
		}

		protected override void OnRenderFrame(FrameEventArgs e) {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			_ship.Draw(_ambientDiffuseSpecularMaterial, _shipTexture);
			
			_asteroids.ForEach(asteroid => {
				asteroid.Draw(_ambientDiffuseSpecularMaterial, _asteroidTexture);
			});


			_simpleTextureMaterial.Draw(_neptuneObject, _neptuneTexture);

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