using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Engine.Util {
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
			
			var font = new Font(
				bitmap,
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