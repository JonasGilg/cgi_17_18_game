using System;
using System.Collections.Generic;
using Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Util {
	public static class TextRenderer2D {
		private static readonly int Program;
		private static readonly Font Font;
		private static readonly int PositionLocation;
		private static readonly int UVLocation;

		static TextRenderer2D() {
			TimingRegistry.AddRenderTiming(RenderStats);
			
			PositionLocation = GL.GenBuffer();
			UVLocation = GL.GenBuffer();
			
			Program = ShaderLoader.LoadShader("Render/Text/Text_VS.glsl", "Render/Text/Text_FS.glsl");
			GL.LinkProgram(Program);

			var fontId = FontManager.CreateFont("data/Font/CrystalFont.bmp", "data/Font/CrystalFontData.csv");
			Font = FontManager.GetFont(fontId);
		}

		private static readonly List<HUDElement> _hudElements = new List<HUDElement>();
		
		/// <summary>
		/// Registers a text to be drawn on the HUD in the following frame.
		/// </summary>
		/// <param name="text">The text to be drawn</param>
		/// <param name="position">The position of the text from (-1, -1) -> bottom left, to (1, 1) -> top right</param>
		/// <param name="scale">The scale of the text (DOES NOTHING YET)</param>
		public static void RegisterHUDElement(string text, Vector2 position, float scale = 1) {
			_hudElements.Add(new HUDElement(text, position, scale));
		}
		
		private static readonly TimingStats RenderStats = new TimingStats("GUI");

		/// <summary>
		/// <b>Only to be called ONCE at the END of the render loop.</b>
		/// <br/>
		/// Draws all the registered HUD elements
		/// </summary>
		public static void Draw() {
			RenderStats.Start();

			for (var i = 0; i < _hudElements.Count; i++) {
				var hudElement = _hudElements[i];
				DrawString(hudElement.text, hudElement.position, hudElement.scale);
			}
			
			_hudElements.Clear();
			
			RenderStats.Stop();
		}

		private static void DrawString(string text, Vector2 position, float scale = 1) {
			var vertices = new List<Vector2>();
			var uvs = new List<Vector2>();

			var currX = 0.0f;
			for (var i = 0; i < text.Length; i++) {
				var charDimensions = Font.GetCharDimensions(text[i]);
				var charWidth = charDimensions.W;

				var vertexDownLeft = new Vector2(position.X + currX, position.Y);
				var vertexDownRight = new Vector2(position.X + charWidth + currX, position.Y);
				var vertexUpRight = new Vector2(position.X + charWidth + currX, position.Y - (Font.FontHeight / (float) Font.ImageHeight));
				var vertexUpLeft = new Vector2(position.X + currX, position.Y - (Font.CellHeight / (float) Font.ImageHeight));
				
				currX += charWidth;
				
				var x = charDimensions.X;
				var y = charDimensions.Y;
				var w = charDimensions.W;
				var h = charDimensions.H;

				var uvDownLeft = new Vector2(x, y);
				var uvDownRight = new Vector2(x + w, y);
				var uvUpRight = new Vector2(x + w, y + h);
				var uvUpLeft = new Vector2(x, y + h);
				
				vertices.Add(vertexUpLeft);
				vertices.Add(vertexDownLeft);
				vertices.Add(vertexUpRight);
				
				uvs.Add(uvUpLeft);
				uvs.Add(uvDownLeft);
				uvs.Add(uvUpRight);
				
				vertices.Add(vertexDownRight);
				vertices.Add(vertexUpRight);
				vertices.Add(vertexDownLeft);

				uvs.Add(uvDownRight);
				uvs.Add(uvUpRight);
				uvs.Add(uvDownLeft);
			}
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, PositionLocation);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector2.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, UVLocation);
			GL.BufferData(BufferTarget.ArrayBuffer, uvs.Count * Vector2.SizeInBytes, uvs.ToArray(), BufferUsageHint.StaticDraw);
			
			GL.UseProgram(Program);
			
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, Font.TexId);
			
			GL.EnableVertexAttribArray(0);
			GL.EnableVertexAttribArray(1);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, PositionLocation);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, IntPtr.Zero);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, UVLocation);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, IntPtr.Zero);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			
			GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count);
			
			GL.Disable(EnableCap.Blend);
			
			GL.DisableVertexAttribArray(0);
			GL.DisableVertexAttribArray(1);
		}
		
		private struct HUDElement {
			public readonly string text;
			public readonly Vector2 position;
			public readonly float scale;

			public HUDElement(string text, Vector2 position, float scale) {
				this.text = text;
				this.position = position;
				this.scale = scale;
			}
		}
	}
}