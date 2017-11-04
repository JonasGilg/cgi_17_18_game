using Engine.Model;
using Engine.Render;

namespace Engine.Material {
	public abstract class BaseMaterial {
		protected int Program;

		protected void CreateShaderProgram(string pathVs, string pathFs) {
			Program = ShaderLoader.LoadShader(pathVs, pathFs);

			// hint: "Program" is not linked yet
		}

		public abstract void Draw(Model3D model, int textureId, float shininess = 0f, int normalMap = -1);
	}
}