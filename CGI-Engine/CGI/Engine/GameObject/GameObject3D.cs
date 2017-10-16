using OpenTK;

namespace CGI.Engine.GameObject {
	public class GameObject3D {
		public Matrix4 ModelMatrix;

		public GameObject3D(Matrix4 modelMatrix = new Matrix4()) {
			ModelMatrix = modelMatrix;
		}
	}
}