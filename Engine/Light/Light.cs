using OpenTK;

namespace Engine {
	public static class Light {
		public static Vector3 LightDirection;
		public static Vector4 LightAmbient;
		public static Vector4 LightDiffuse;
		public static Vector4 LightSpecular;

		public static void SetDirectionalLight(Vector3 direction, Vector4 ambient, Vector4 diffuse, Vector4 specular) {
			LightDirection = Vector3.Normalize(direction);
			LightAmbient = ambient;
			LightDiffuse = diffuse;
			LightSpecular = specular;
		}
	}
}