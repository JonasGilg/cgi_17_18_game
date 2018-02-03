using OpenTK;

namespace Engine.Output {
	public static class Display {
		public static int DisplayWidth => DisplayDevice.Default.Width;
		public static int DisplayHeight => DisplayDevice.Default.Height;
		public static double AspectRatio => (double) DisplayWidth / DisplayHeight;
		public static Vector2d ResolutionVector => new Vector2d(DisplayWidth, DisplayHeight);
	}
}