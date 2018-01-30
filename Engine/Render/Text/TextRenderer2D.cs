using System;
using System.Collections.Generic;
using Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Render {
	public static class TextRenderer2D {
		private static readonly int PROGRAM;
		private static readonly Font FONT;
		private static readonly int POSITION_LOCATION;
		private static readonly int UV_LOCATION;

		static TextRenderer2D() {
			POSITION_LOCATION = GL.GenBuffer();
			UV_LOCATION = GL.GenBuffer();

			PROGRAM = ShaderLoader.LoadShader("Render/Text/Text_VS.glsl", "Render/Text/Text_FS.glsl");
			GL.LinkProgram(PROGRAM);

			var fontId = FontManager.CreateFont("data/Font/CrystalFont.bmp", "data/Font/CrystalFontData.csv");
			FONT = FontManager.GetFont(fontId);
		}

		public static void DrawString(string text, Vector2 position, float scale = 1) {
			var vertices = new List<Vector2>();
			var uvs = new List<Vector2>();

			var currX = 0.0f;
			for (var i = 0; i < text.Length; i++) {

				Font.Rectangle charDimensions;
				try {
					charDimensions = FONT.GetCharDimensions(text[i]);
				}
				catch (KeyNotFoundException e) {
					Console.WriteLine($"Character: '{(int) text[i]}' is not supported!");
					charDimensions = FONT.GetCharDimensions('?');
				}
				var charWidth = charDimensions.W;

				var vertexDownLeft = new Vector2(position.X + currX, position.Y);
				var vertexDownRight = new Vector2(position.X + charWidth + currX, position.Y);
				var vertexUpRight = new Vector2(position.X + charWidth + currX,
					position.Y - (FONT.FontHeight / (float) FONT.ImageHeight));
				var vertexUpLeft = new Vector2(position.X + currX, position.Y - (FONT.CellHeight / (float) FONT.ImageHeight));

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

			GL.BindBuffer(BufferTarget.ArrayBuffer, POSITION_LOCATION);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector2.SizeInBytes, vertices.ToArray(),
				BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, UV_LOCATION);
			GL.BufferData(BufferTarget.ArrayBuffer, uvs.Count * Vector2.SizeInBytes, uvs.ToArray(), BufferUsageHint.StaticDraw);

			GL.UseProgram(PROGRAM);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, FONT.TexId);

			GL.EnableVertexAttribArray(0);
			GL.EnableVertexAttribArray(1);

			GL.BindBuffer(BufferTarget.ArrayBuffer, POSITION_LOCATION);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, IntPtr.Zero);

			GL.BindBuffer(BufferTarget.ArrayBuffer, UV_LOCATION);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, IntPtr.Zero);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Disable(EnableCap.DepthTest);
			
			GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count);

			GL.Enable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Blend);

			GL.DisableVertexAttribArray(0);
			GL.DisableVertexAttribArray(1);
		}
	}
}