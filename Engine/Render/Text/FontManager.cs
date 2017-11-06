using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Engine.Texture;
using OpenTK.Graphics.OpenGL;

namespace Engine.Util {
	public static class FontManager {
		private static readonly Dictionary<int, Font> Fonts = new Dictionary<int, Font>();
		
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

			Fonts[fontNumber] = font;
			return fontNumber;
		}

		public static Font GetFont(int font) => Fonts[font];
	}
}