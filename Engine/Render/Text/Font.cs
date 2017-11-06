using System.Collections.Generic;
using System.Drawing;
using OpenTK;

namespace Engine.Util {
	public class Font {
		public readonly string Name;
		
		public readonly int ImageWidth;
		public readonly int ImageHeight;
		public readonly int CellWidth;
		public readonly int CellHeight;
		
		public readonly int StartChar;

		public readonly int FontHeight;
		public readonly int FontWidth;

		public readonly Dictionary<char, int> CharWidhts;

		public readonly int TexId;
		
		public Font(int texId, string name, int imageWidth, int imageHeight, int cellWidth, int cellHeight, int startChar, int fontHeight, int fontWidth) {
			TexId = texId;
			Name = name;
			ImageWidth = imageWidth;
			ImageHeight = imageHeight;
			CellWidth = cellWidth;
			CellHeight = cellHeight;
			StartChar = startChar;
			FontHeight = fontHeight;
			FontWidth = fontWidth;
			CharWidhts = new Dictionary<char, int>();
		}

		public Rectangle GetCharDimensions(char c) {
			var charWidth = CharWidhts[c];
			var position = c - StartChar;

			var x = position * CellWidth % ImageWidth;
			var y = position * CellWidth / ImageWidth * CellHeight;
			
			return new Rectangle(x, y, charWidth, FontHeight);
		}

		public struct Rectangle {
			public readonly int X;
			public readonly int Y;
			public readonly int W;
			public readonly int H;
			
			public Rectangle(int x, int y, int w, int h) {
				X = x;
				Y = y;
				W = w;
				H = h;
			}
		}
	}
}