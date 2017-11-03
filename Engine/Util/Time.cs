namespace Engine.Util {
	public static class Time {
		public static float DeltaTime { get; private set; }

		public static void Update(double deltaTime) {
			DeltaTime = (float) deltaTime;
		}
	}
}