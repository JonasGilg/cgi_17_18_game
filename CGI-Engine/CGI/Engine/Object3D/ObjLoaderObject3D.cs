﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OpenTK;

namespace CGI.Engine.Object3D {
	public class ObjLoaderObject3D : BaseObject3D {
		public ObjLoaderObject3D(string filePath) {
			Positions = new List<Vector3>();
			Normals = new List<Vector3>();
			UVs = new List<Vector2>();
			Indices = new List<int>();

			var v = new List<Vector3>();
			var vt = new List<Vector2>();
			var vn = new List<Vector3>();

			var input = File.ReadLines(filePath);

			foreach (var line in input) {
				var parts = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

				if (parts.Length > 0) {
					switch (parts[0]) {
						case "v":
							v.Add(new Vector3(float.Parse(parts[1], CultureInfo.InvariantCulture),
								float.Parse(parts[2], CultureInfo.InvariantCulture), float.Parse(parts[3], CultureInfo.InvariantCulture)));
							break;
						case "vt":
							vt.Add(new Vector2(float.Parse(parts[1], CultureInfo.InvariantCulture),
								1.0f - float.Parse(parts[2], CultureInfo.InvariantCulture)));
							break;
						case "vn":
							vn.Add(new Vector3(float.Parse(parts[1], CultureInfo.InvariantCulture),
								float.Parse(parts[2], CultureInfo.InvariantCulture), float.Parse(parts[3], CultureInfo.InvariantCulture)));
							break;
						case "f":
							var triIndicesV1 = parts[1].Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
							var triIndicesV2 = parts[2].Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
							var triIndicesV3 = parts[3].Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

							AddTriangle(v[Convert.ToInt32(triIndicesV1[0]) - 1], v[Convert.ToInt32(triIndicesV2[0]) - 1],
								v[Convert.ToInt32(triIndicesV3[0]) - 1],
								vn[Convert.ToInt32(triIndicesV1[2]) - 1], vn[Convert.ToInt32(triIndicesV2[2]) - 1],
								vn[Convert.ToInt32(triIndicesV3[2]) - 1],
								vt[Convert.ToInt32(triIndicesV1[1]) - 1], vt[Convert.ToInt32(triIndicesV2[1]) - 1],
								vt[Convert.ToInt32(triIndicesV3[1]) - 1]);
							break;
					}
				}
			}

			//CreatePositionBuffer();
			//CreateUVBuffer();
			//CreateIndexBuffer();

			CreateVao();
		}
	}
}