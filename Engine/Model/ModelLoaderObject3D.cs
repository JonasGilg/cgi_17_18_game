using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OpenTK;

namespace Engine.Model {
	public static class ModelLoaderObject3D {
		public static Model3D Load(string filePath, float scale = 1.0f,
			bool doAverageTangets = false,
			bool createVAO = true) {
			
			var model = new Model3D();

			var v = new List<Vector3>();
			var vt = new List<Vector2>();
			var vn = new List<Vector3>();

			var input = File.ReadLines(filePath);

			foreach (var line in input) {
				var parts = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length > 0) {
					Vector3 vec3;
					switch (parts[0]) {
						case "v":
							vec3 = new Vector3(float.Parse(parts[1], CultureInfo.InvariantCulture) * scale,
								float.Parse(parts[2], CultureInfo.InvariantCulture) * scale,
								float.Parse(parts[3], CultureInfo.InvariantCulture) * scale);


							v.Add(vec3);
							break;
						case "vt":
							vt.Add(new Vector2(float.Parse(parts[1], CultureInfo.InvariantCulture),
								1.0f - float.Parse(parts[2], CultureInfo.InvariantCulture)));
							break;
						case "vn":
							vec3 = new Vector3(float.Parse(parts[1], CultureInfo.InvariantCulture),
								float.Parse(parts[2], CultureInfo.InvariantCulture),
								float.Parse(parts[3], CultureInfo.InvariantCulture));


							vn.Add(vec3);
							break;
						case "f":
							var triIndicesV1 = parts[1].Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
							var triIndicesV2 = parts[2].Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
							var triIndicesV3 = parts[3].Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

							model.AddTriangle(
								v[Convert.ToInt32(triIndicesV1[0]) - 1], v[Convert.ToInt32(triIndicesV2[0]) - 1],
								v[Convert.ToInt32(triIndicesV3[0]) - 1],
								vn[Convert.ToInt32(triIndicesV1[2]) - 1], vn[Convert.ToInt32(triIndicesV2[2]) - 1],
								vn[Convert.ToInt32(triIndicesV3[2]) - 1],
								vt[Convert.ToInt32(triIndicesV1[1]) - 1], vt[Convert.ToInt32(triIndicesV2[1]) - 1],
								vt[Convert.ToInt32(triIndicesV3[1]) - 1]);
							break;
					}
				}
			}

			if (doAverageTangets) model.AverageTangents();
			if (createVAO) model.CreateVAO();

			return model;
		}
	}
}