using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Engine.Util {
	public class Font {
		public readonly Bitmap Image;
		
		public readonly string Name;
		
		public readonly int ImageWidth;
		public readonly int ImageHeight;
		public readonly int CellWidth;
		public readonly int CellHeight;
		
		public readonly int StartChar;

		public readonly int FontHeight;
		public readonly int FontWidth;

		public readonly Dictionary<char, int> CharWidhts;
		
		public Font(Bitmap image, string name, int imageWidth, int imageHeight, int cellWidth, int cellHeight, int startChar, int fontHeight, int fontWidth) {
			Image = image;
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
	}

	public static class FontManager {
		private static readonly Dictionary<int, Font> _fonts = new Dictionary<int, Font>();
		private static int _fontCounter;
		
		public static int CreateFont(string imagePath, string fontPath) {
			var bitmap = new Bitmap(imagePath);

			var fontCSV = File.ReadAllLines(fontPath);
			var csvMap = new Dictionary<string, string>();
			
			foreach (var line in fontCSV) {
				var result = line.Split(',');
				csvMap[result[0]] = result[1];
			}
			
			var font = new Font(bitmap,
				csvMap["Font Name"],
				int.Parse(csvMap["Image Width"]),
				int.Parse(csvMap["Image Height"]),
				int.Parse(csvMap["Cell Width"]),
				int.Parse(csvMap["Cell Height"]),
				int.Parse(csvMap["Start Char"]),
				int.Parse(csvMap["Font Height"]),
				int.Parse(csvMap["Font Width (0 is default)"])
			);

			var counter = int.Parse(csvMap["Start Char"]);
			while (csvMap.ContainsKey("Char " + counter + " Base Width")) {
				var width = csvMap["Char " + counter + " Base Width"];
				font.CharWidhts[(char) counter] = int.Parse(width);
				counter++;
			}

			var fontNumber = _fontCounter++;
			_fonts[fontNumber] = font;
			return fontNumber;
		}

		public static Font GetFont(int font) => _fonts[font];
	}
}