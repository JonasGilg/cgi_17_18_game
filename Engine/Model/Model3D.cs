using System;
using System.Collections.Generic;
using Engine.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Model {
	public class Model3D : Component.Component {
		// the transformation (position, rotation, scale) of the object
		public Matrix4d Transformation = Matrix4d.Identity;

		// lists, filled with the 3d-data
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

		public Model3D(GameObject gameObject) : base(gameObject) {
			positions = new List<Vector3>();
			normals = new List<Vector3>();
			uVs = new List<Vector2>();
			tangents = new List<Vector3>();
			biTangents = new List<Vector3>();
			Indices = new List<int>();
		}

		// the index-List
		public readonly List<int> Indices;

		// Vartex-Array-Object "VAO"
		public int VAO;

		// generates the Vartex-Array-Objekt
		public void CreateVAO() {
			// list of the complete vertex data
			var allData = new List<float>();

			// "interleaved" means position, normal and uv in one block for each vertex
			for (var i = 0; i < positions.Count; i++) {
				allData.Add(positions[i].X);
				allData.Add(positions[i].Y);
				allData.Add(positions[i].Z);

				allData.Add(normals[i].X);
				allData.Add(normals[i].Y);
				allData.Add(normals[i].Z);

				allData.Add(uVs[i].X);
				allData.Add(uVs[i].Y);

				allData.Add(tangents[i].X);
				allData.Add(tangents[i].Y);
				allData.Add(tangents[i].Z);

				allData.Add(biTangents[i].X);
				allData.Add(biTangents[i].Y);
				allData.Add(biTangents[i].Z);
			}

			// generate the VBO for the "interleaved" data
			GL.GenBuffers(1, out int allBufferVBO);

			// Buffer is "binded", following OpenGL commands refer to this buffer
			GL.BindBuffer(BufferTarget.ArrayBuffer, allBufferVBO);

			// Data is uploaded to graphics memory
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (allData.Count * sizeof(float)), allData.ToArray(),
				BufferUsageHint.StaticDraw);

			// BindBuffer to 0, so the following commands do not overwrite the current vbo (state machine)
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);


			// generating the index buffer
			GL.GenBuffers(1, out int indexBuffer);

			// Buffer is "binded", following OpenGL commands refer to this buffer
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);

			// index data is uploaded to grpahics mamory
			GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * Indices.Count), Indices.ToArray(),
				BufferUsageHint.StaticDraw);

			// BindBuffer to 0, so the following commands do not overwrite the current element buffer (state machine)
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);


			// generating the Vertex-Array-Objects
			GL.GenVertexArrays(1, out VAO);

			// Vertex-Array-Objekt is "binded", following OpenGL commands refer to this VAO. Inmportant for the folling two calls of "BindBuffer"
			GL.BindVertexArray(VAO);

			// BindBuffer commands: Are saved by our VAO.
			// Element-Buffer (indices)
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);

			// ... then our interleaved VBO.
			GL.BindBuffer(BufferTarget.ArrayBuffer, allBufferVBO);

			// three calls of GL.VertexAttribPointer do follow, must be first "enabled"
			GL.EnableVertexAttribArray(0);
			GL.EnableVertexAttribArray(1);
			GL.EnableVertexAttribArray(2);
			GL.EnableVertexAttribArray(3);
			GL.EnableVertexAttribArray(4);

			// The description of our "interleaved" data structure, the shader needs to know how tpo handle our data
			// Die assignment to the "Index", the first parameter, will be recognized by the shader
			var strideSize = Vector3.SizeInBytes * 4 + Vector2.SizeInBytes;

			// At Index 0 (so at first) we have our position data. The last parameter defines at which byte-place in the vertex block the data for the position is saved 
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, strideSize, 0);

			// At Index 1 we have our normal data. We have it after the position, which is a "Vector3" type, so the byte-place is "Vector3.SizeInBytes"
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, strideSize, Vector3.SizeInBytes);

			// At Index 2 we have our UV data. We have it after the position and the normal, which are both "Vector3" type, so the byte-place is "Vector3.SizeInBytes" * 2
			GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, true, strideSize, Vector3.SizeInBytes * 2);

			// At Index 3 tangents.
			GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, true, strideSize,
				Vector3.SizeInBytes * 2 + Vector2.SizeInBytes);

			// At Index 4 biTangents.
			GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, true, strideSize,
				Vector3.SizeInBytes * 3 + Vector2.SizeInBytes);


			// BindBuffer to 0, so the following commands do not overwrite the current VAO
			GL.BindVertexArray(0);

			// Note: The generated VAO defines a data-structure, which must be considered by the shader regarding the index-places defined by GL.VertexAttribPointer 
			// The data-format placing 0 = position; 1 = normal and 2 = uv must be used by our materials
		}


		// Adds a triangle
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

			// calculate tangents / bi-tangents
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

		// unloads from graphics memory
		public void UnLoad() {
			// tbd.
		}

		public override void Update() {
			Transformation = GameObject.TransformComponent.WorldMatrix;
			
		}
	}
}