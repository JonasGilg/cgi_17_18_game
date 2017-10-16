using OpenTK;

namespace CGI.Engine.Light {
	public static class Light {
		public static Vector3 LightDirection;
		public static Vector4 LightAmbient;
		public static Vector4 LightDiffuse;

		public static void SetDirectionalLight(Vector3 direction, Vector4 ambient, Vector4 diffuse) {
			LightDirection = Vector3.Normalize(direction);
			LightAmbient = ambient;
			LightDiffuse = diffuse;
		}
	}
}