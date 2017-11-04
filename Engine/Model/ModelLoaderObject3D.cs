using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OpenTK;

namespace Engine.Model {
	public static class ModelLoaderObject3D {
		private static readonly Random Random = new Random();

		public static Model3D Load(string filePath, float scale = 1.0f, bool doAverageTangets = false, bool createVAO = true,
			bool withNoise = false) {
			var model = new Model3D();
			var noise = 1.0f;
			var fastNoise = new FastNoise();
			if (withNoise) {
				fastNoise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);
				fastNoise.SetFractalType(FastNoise.FractalType.RigidMulti);
				fastNoise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Euclidean);
				fastNoise.SetCellularReturnType(FastNoise.CellularReturnType.Distance);
				fastNoise.SetFractalLacunarity(2.0f);
				fastNoise.SetFractalGain(0.5f);
				fastNoise.SetSeed(Random.Next(1000, 100000));
				var freq = (float) Random.Next(1, 5) / 10;
				fastNoise.SetFrequency(freq);
				Console.Out.WriteLine("seed: " + fastNoise.GetSeed());
				Console.Out.WriteLine("freq: " + freq);
			}


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
							
							if (withNoise) {
								noise = fastNoise.GetNoise(vec3.X, vec3.Y, vec3.Z);
								//noise = (float) (Random.NextDouble() - 0.5) / 4 + 1;
								noise = 1 + noise * 2;
								
								Console.Out.WriteLine("Vec: " + vec3 + ", Noise: " + noise + ", NewVec: " + (vec3*noise));
							}
							
							v.Add(vec3 * noise);
							break;
						case "vt":
							vt.Add(new Vector2(float.Parse(parts[1], CultureInfo.InvariantCulture),
								1.0f - float.Parse(parts[2], CultureInfo.InvariantCulture)));
							break;
						case "vn":
							vec3 = new Vector3(float.Parse(parts[1], CultureInfo.InvariantCulture),
								float.Parse(parts[2], CultureInfo.InvariantCulture),
								float.Parse(parts[3], CultureInfo.InvariantCulture));
							if (withNoise) {
								noise = fastNoise.GetNoise(vec3.X, vec3.Y, vec3.Z);
								noise = 1 + noise * 10;
							}
							
							vn.Add(Vector3.Multiply(vec3, noise));
							break;
						case "f":
							var triIndicesV1 = parts[1].Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
							var triIndicesV2 = parts[2].Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
							var triIndicesV3 = parts[3].Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);


							model.AddTriangle(v[Convert.ToInt32(triIndicesV1[0]) - 1], v[Convert.ToInt32(triIndicesV2[0]) - 1],
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