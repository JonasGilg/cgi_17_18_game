using System;
using System.Collections.Generic;
using Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Util {
	public static class TextRenderer2D {
		private static readonly int Texture;
		private static readonly int Program;
		private static readonly Font Font;
		private static readonly int PositionLocation;
		private static readonly int UVLocation;

		static TextRenderer2D() {
			PositionLocation = GL.GenBuffer();
			UVLocation = GL.GenBuffer();
			
			Program = ShaderLoader.LoadShader("Render/Text/Text_VS.glsl", "Render/Text/Text_FS.glsl");
			GL.LinkProgram(Program);

			/*PositionLocation = GL.GetAttribLocation(Program, "vertexPosition");
			UVLocation = GL.GetAttribLocation(Program, "vertexUV");*/
			
			var fontId = FontManager.CreateFont("data/Font/CrystalFont.bmp", "data/Font/CrystalFontData.csv");
			Font = FontManager.GetFont(fontId);
			Texture = Font.TexId;
		}

		public static void DrawString(string text, Vector2 position, float scale = 1) {
			var vertices = new List<Vector2>();
			var uvs = new List<Vector2>();

			var currX = 0.0f;
			for (var i = 0; i < text.Length; i++) {
				var charDimensions = Font.GetCharDimensions(text[i]);
				var charWidth = charDimensions.W;

				var vertexUpLeft = new Vector2(position.X + currX, position.Y);
				var vertexUpRight = new Vector2(position.X + charWidth + currX, position.Y);
				var vertexDownRight = new Vector2(position.X + charWidth + currX, position.Y - (Font.FontHeight / (float) Font.ImageHeight));
				var vertexDownLeft = new Vector2(position.X + currX, position.Y - (Font.CellHeight / (float) Font.ImageHeight));
				
				currX += charWidth;
				
				var x = charDimensions.X;
				var y = charDimensions.Y;
				var w = charDimensions.W;
				var h = charDimensions.H;

				var uvUpLeft = new Vector2(x, y);
				var uvUpRight = new Vector2(x + w, y);
				var uvDownRight = new Vector2(x + w, y + h);
				var uvDownLeft = new Vector2(x, y + h);
				
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
			GL.BindBuffer(BufferTarget.ArrayBuffer, PositionLocation);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, IntPtr.Zero);
			
			GL.EnableVertexAttribArray(1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, UVLocation);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, IntPtr.Zero);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			
			GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count);
			
			GL.Disable(EnableCap.Blend);
			
			GL.DisableVertexAttribArray(0);
			GL.DisableVertexAttribArray(1);
		}
	}
}