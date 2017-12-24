using System.Collections.Generic;
using System.IO;
using Engine.Texture;

namespace Engine.Render {
	public static class FontManager {
		private static readonly Dictionary<int, Font> FONTS = new Dictionary<int, Font>();

		public static int CreateFont(string imagePath, string fontPath) {
			var fontNumber = TextureManager.LoadTexture(imagePath);

			var fontCSV = File.ReadAllLines(fontPath);
			var csvMap = new Dictionary<string, string>();

			foreach (var line in fontCSV) {
				var result = line.Split(',');
				csvMap[result[0]] = result[1];
			}

			var font = new Font(
				fontNumber,
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

			FONTS[fontNumber] = font;
			return fontNumber;
		}

		public static Font GetFont(int font) => FONTS[font];
	}
}