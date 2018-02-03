using OpenTK;

namespace Engine {
	public static class Light {
		public static Vector3d LightOrigin;

		public static Vector4 LightAmbient;
		public static Vector4 LightDiffuse;
		public static Vector4 LightSpecular;

		public static void SetDirectionalLight(Vector4 ambient, Vector4 diffuse, Vector4 specular) {
			LightAmbient = ambient;
			LightDiffuse = diffuse;
			LightSpecular = specular;
		}

		public static void SetSpotLight(Vector3d origin, Vector4 ambient, Vector4 diffuse, Vector4 specular) {
			LightOrigin = origin;
			LightAmbient = ambient;
			LightDiffuse = diffuse;
			LightSpecular = specular;
		}
	}
}