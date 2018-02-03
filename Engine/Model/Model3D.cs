using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Model {
	public class Model3D {
		public Matrix4d Transformation = Matrix4d.Identity;

		private readonly List<Vector3> positions;

		private readonly List<Vector3> normals;
		private readonly List<Vector2> uVs;
		private readonly List<Vector3> tangents;
		private readonly List<Vector3> biTangents;

		private Vector3d min = Vector3d.Zero;
		private Vector3d max = Vector3d.Zero;

		public AxisAlignedBoundingBox AABB => AxisAlignedBoundingBox.FromMinMax(min, max);

		public double Radius(Vector3d scale) {
			var newAABB = AABB * scale;
			return Math.Max(Math.Max(newAABB.HalfDimension.X, newAABB.HalfDimension.Y), newAABB.HalfDimension.Z);
		}

		public Model3D() {
			positions = new List<Vector3>();
			normals = new List<Vector3>();
			uVs = new List<Vector2>();
			tangents = new List<Vector3>();
			biTangents = new List<Vector3>();
			Indices = new List<int>();
		}

		public readonly List<int> Indices;

		public int VAO;

		public void CreateVAO() {
			const int size = 14;

			var allData = new float[positions.Count * size];

			Parallel.For(0, positions.Count, i => {
				var pos = i * size;

				allData[pos++] = positions[i].X;
				allData[pos++] = positions[i].Y;
				allData[pos++] = positions[i].Z;

				allData[pos++] = normals[i].X;
				allData[pos++] = normals[i].Y;
				allData[pos++] = normals[i].Z;

				allData[pos++] = uVs[i].X;
				allData[pos++] = uVs[i].Y;

				allData[pos++] = tangents[i].X;
				allData[pos++] = tangents[i].Y;
				allData[pos++] = tangents[i].Z;

				allData[pos++] = biTangents[i].X;
				allData[pos++] = biTangents[i].Y;
				allData[pos] = biTangents[i].Z;
			});

			GL.GenBuffers(1, out int allBufferVBO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, allBufferVBO);

			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (allData.Length * sizeof(float)), allData,
				BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			GL.GenBuffers(1, out int indexBuffer);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);

			GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * Indices.Count), Indices.ToArray(),
				BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			GL.GenVertexArrays(1, out VAO);
			GL.BindVertexArray(VAO);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);

			GL.BindBuffer(BufferTarget.ArrayBuffer, allBufferVBO);

			GL.EnableVertexAttribArray(0);
			GL.EnableVertexAttribArray(1);
			GL.EnableVertexAttribArray(2);
			GL.EnableVertexAttribArray(3);
			GL.EnableVertexAttribArray(4);

			var strideSize = Vector3.SizeInBytes * 4 + Vector2.SizeInBytes;

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, strideSize, 0);
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, strideSize, Vector3.SizeInBytes);
			GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, true, strideSize, Vector3.SizeInBytes * 2);

			GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, true, strideSize,
				Vector3.SizeInBytes * 2 + Vector2.SizeInBytes);

			GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, true, strideSize,
				Vector3.SizeInBytes * 3 + Vector2.SizeInBytes);

			GL.BindVertexArray(0);
		}

		public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 n1, Vector3 n2, Vector3 n3, Vector2 uv1,
			Vector2 uv2, Vector2 uv3) {
			var index = positions.Count;

			positions.Add(v1);
			positions.Add(v2);
			positions.Add(v3);

			min.X = Math.Min(Math.Min(Math.Min(min.X, v1.X), v2.X), v3.X);
			min.Y = Math.Min(Math.Min(Math.Min(min.Y, v1.Y), v2.Y), v3.Y);
			min.Z = Math.Min(Math.Min(Math.Min(min.Z, v1.Z), v2.Z), v3.Z);

			max.X = Math.Max(Math.Max(Math.Max(max.X, v1.X), v2.X), v3.X);
			max.Y = Math.Max(Math.Max(Math.Max(max.Y, v1.Y), v2.Y), v3.Y);
			max.Z = Math.Max(Math.Max(Math.Max(max.Z, v1.Z), v2.Z), v3.Z);

			normals.Add(n1);
			normals.Add(n2);
			normals.Add(n3);

			uVs.Add(uv1);
			uVs.Add(uv2);
			uVs.Add(uv3);

			var edge1 = v2 - v1;
			var edge2 = v3 - v1;

			var deltaUV1 = uv2 - uv1;
			var deltaUV2 = uv3 - uv1;

			float f;
			if (Math.Abs(deltaUV1.X * deltaUV2.Y - deltaUV2.X * deltaUV1.Y) < 0.0001f) {
				f = 1.0f;
			}
			else {
				f = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV2.X * deltaUV1.Y);
			}

			var tangent = new Vector3(f * (deltaUV2.Y * edge1.X - deltaUV1.Y * edge2.X),
				f * (deltaUV2.Y * edge1.Y - deltaUV1.Y * edge2.Y),
				f * (deltaUV2.Y * edge1.Z - deltaUV1.Y * edge2.Z));
			tangent.Normalize();

			var biTangent = new Vector3(f * (-deltaUV2.X * edge1.X + deltaUV1.X * edge2.X),
				f * (-deltaUV2.X * edge1.Y + deltaUV1.X * edge2.Y),
				f * (-deltaUV2.X * edge1.Z + deltaUV1.X * edge2.Z));
			biTangent.Normalize();


			if (Vector3.Dot(Vector3.Cross(n1, tangent), biTangent) < 0.0f) {
				tangent = tangent * -1.0f;
			}


			tangents.Add(tangent);
			tangents.Add(tangent);
			tangents.Add(tangent);

			biTangents.Add(biTangent);
			biTangents.Add(biTangent);
			biTangents.Add(biTangent);

			Indices.Add(index);
			Indices.Add(index + 2);
			Indices.Add(index + 1);
		}

		public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector2 uv1, Vector2 uv2, Vector2 uv3) {
			var normal = Vector3.Cross(v2 - v1, v3 - v1).Normalized();
			AddTriangle(v1, v2, v3, normal, normal, normal, uv1, uv2, uv3);
		}

		public void AverageTangents() {
			var len = positions.Count;

			for (var i = 0; i < len - 1; i++) {
				for (var o = i + 1; o < len; o++) {
					if (positions[i] == positions[o] && normals[i] == normals[o] && uVs[i] == uVs[o]) {
						var tanI = tangents[i];
						tangents[i] += tangents[o];
						tangents[o] += tanI;

						var biTanI = biTangents[i];
						biTangents[i] += biTangents[o];
						biTangents[o] += biTanI;
					}
				}
			}
		}
	}
}