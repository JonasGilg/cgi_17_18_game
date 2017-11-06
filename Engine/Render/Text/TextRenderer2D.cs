using System;
using System.Drawing;
using System.Drawing.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Util {
	public class TextRenderer2D : IDisposable {
		private readonly Bitmap _bmp;
		private readonly Graphics _gfx;
		private readonly int _texture;
		private Rectangle _dirtyRegion;
		private bool _disposed;

		public TextRenderer2D(int width, int height) {
			_bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			_gfx = Graphics.FromImage(_bmp);
			_gfx.TextRenderingHint = TextRenderingHint.AntiAlias;

			_texture = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, _texture);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
				PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
		}

		public void DrawString(string text, System.Drawing.Font font, Brush brush, PointF point) {
			_gfx.DrawString(text, font, brush, point);

			var size = _gfx.MeasureString(text, font);
			_dirtyRegion = Rectangle.Round(RectangleF.Union(_dirtyRegion, new RectangleF(point, size)));
			_dirtyRegion = Rectangle.Intersect(_dirtyRegion, new Rectangle(0, 0, _bmp.Width, _bmp.Height));
		}

		public int Texture {
			get {
				UploadBitmap();
				return _texture;
			}
		}

		private void UploadBitmap() {
			if (_dirtyRegion != RectangleF.Empty) {
				var data = _bmp.LockBits(_dirtyRegion,
					System.Drawing.Imaging.ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				GL.BindTexture(TextureTarget.Texture2D, _texture);
				GL.TexSubImage2D(TextureTarget.Texture2D, 0,
					_dirtyRegion.X, _dirtyRegion.Y, _dirtyRegion.Width, _dirtyRegion.Height,
					PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

				_bmp.UnlockBits(data);

				_dirtyRegion = Rectangle.Empty;
			}
		}

		private void Dispose(bool manual) {
			if (!_disposed) {
				if (manual) {
					_bmp.Dispose();
					_gfx.Dispose();
					if (GraphicsContext.CurrentContext != null)
						GL.DeleteTexture(_texture);
				}

				_disposed = true;
			}
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/*private static readonly int Program;
		private static readonly int VertexPositionLocation;
		private static readonly int VertexUVLocation;
		private static readonly int TextColorLocation;
		private static readonly int TexSamplerLocation;

		static TextRenderer2D() {
			Program = ShaderLoader.LoadShader("Render/Text/Text_VS.glsl", "Render/Text/Text_FS.glsl");
			VertexUVLocation = GL.GenBuffer();
			VertexPositionLocation = GL.GenBuffer();
			
			GL.LinkProgram(Program);

			TextColorLocation = GL.GetUniformLocation(Program, "textColor");
			TexSamplerLocation = GL.GetUniformLocation(Program, "texSampler");
		}

		public static void PrintText2D(string text, Vector2 position, Font font, float scale = 1) {
			var vertices = new List<Vector2>();
			var uvs = new List<Vector2>();

			var currX = 0;
			for (var i = 0; i < text.Length; i++) {
				var charDimensions = font.GetCharDimensions(text[i]);
				var charWidth = charDimensions.W;

				var vertexUpLeft = new Vector2(position.X + currX * scale, position.Y + font.FontHeight);
				var vertexUpRight = new Vector2(position.X + (charWidth + currX) * scale, position.Y + font.CellHeight);
				var vertexDownRight = new Vector2(position.X + (charWidth + currX) * scale, position.Y);
				var vertexDownLeft = new Vector2(position.X + currX * scale, position.Y);
				
				vertices.Add(vertexUpLeft);
				vertices.Add(vertexDownLeft);
				vertices.Add(vertexUpRight);
				
				vertices.Add(vertexDownRight);
				vertices.Add(vertexUpRight);
				vertices.Add(vertexDownLeft);

				currX += charWidth;

				
				var x = 1f / charDimensions.X;
				var y = 1f / charDimensions.Y;
				var w = 1f / charDimensions.W;
				var h = 1f / charDimensions.H;

				var uvUpLeft = new Vector2(x, y);
				var uvUpRight = new Vector2(x + w, y);
				var uvDownRight = new Vector2(x + w, y + h);
				var uvDownLeft = new Vector2(x, y + h);
				
				uvs.Add(uvUpLeft);
				uvs.Add(uvDownLeft);
				uvs.Add(uvUpRight);
				
				uvs.Add(uvDownRight);
				uvs.Add(uvUpRight);
				uvs.Add(uvDownLeft);
			}
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexPositionLocation);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector2.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexUVLocation);
			GL.BufferData(BufferTarget.ArrayBuffer, uvs.Count * Vector2.SizeInBytes, uvs.ToArray(), BufferUsageHint.StaticDraw);
			
			GL.UseProgram(Program);
			
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, font.TexId);
			
			GL.Uniform1(TexSamplerLocation, 0);
			
			GL.EnableVertexAttribArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexPositionLocation);
			GL.VertexAttribPointer(0, Vector2.SizeInBytes, VertexAttribPointerType.Float, false, 0, 0);
			
			GL.EnableVertexAttribArray(1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexUVLocation);
			GL.VertexAttribPointer(1, Vector2.SizeInBytes, VertexAttribPointerType.Float, false, 0, 0);
			
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			
			GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count);
			
			GL.Disable(EnableCap.Blend);
			
			GL.DisableVertexAttribArray(0);
			GL.DisableVertexAttribArray(1);
		}*/
	}
}