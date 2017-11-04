namespace Engine.Util {
	public static class Math3D {

	}

	public static class Math1D {
		public static float Clamp(float val, float min, float max) => val < min ? min : val > max ? max : val;
	}
}