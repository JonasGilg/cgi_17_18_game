using System.Collections.Generic;
using OpenTK;

namespace CGI.Engine.Object3D {
	public class CubeObject3D : BaseObject3D {
		public CubeObject3D() {
			Positions = new List<Vector3>();
			UVs = new List<Vector2>();
			Normals = new List<Vector3>();
			Indices = new List<int>();

			AddTriangle(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1),
				new Vector3(1, 1, 1), new Vector3(1, 1, 1),
				new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1));


			AddTriangle(new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 2, 0), new Vector3(1, 1, 1),
				new Vector3(1, 1, 1), new Vector3(1, 1, 1),
				new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1));


			CreateVao();
		}
	}
}