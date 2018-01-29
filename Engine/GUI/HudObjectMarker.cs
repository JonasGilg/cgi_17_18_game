﻿using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.GUI {
	public class HudObjectMarker {
		public readonly GameObject GameObject;
		public static readonly double MIN_SIZE = 50.0;
		public readonly int ID;
		public int VAO;

		private const float EDGE_LENGTH = 0.8f;

		private readonly float[] verticies = {
			1.0f, 1.0f, 0.0f,
			1.0f, EDGE_LENGTH, 0.0f,
			1.0f, -EDGE_LENGTH, 0.0f,
			1.0f, -1.0f, 0.0f,

			1.0f, -1.0f, 0.0f,
			EDGE_LENGTH, -1.0f, 0.0f,
			-EDGE_LENGTH, -1.0f, 0.0f,
			-1.0f, -1.0f, 0.0f,

			-1.0f, -1.0f, 0.0f,
			-1.0f, -EDGE_LENGTH, 0.0f,
			-1.0f, EDGE_LENGTH, 0.0f,
			-1.0f, 1.0f, 0.0f,

			-1.0f, 1.0f, 0.0f,
			-EDGE_LENGTH, 1.0f, 0.0f,
			EDGE_LENGTH, 1.0f, 0.0f,
			1.0f, 1.0f, 0.0f
		};

		public readonly uint[] indicies = {
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
		};


		public int VertexBufferSize() => (verticies.Length * Vector3.SizeInBytes);

		public int IndexBufferSize() => (indicies.Length * sizeof(uint));


		public HudObjectMarker(int id, GameObject gameObject) {
			this.ID = id;
			GameObject = gameObject;

			createVAO();
		}

		public void createVAO() {
			// generate the VBO for the "interleaved" data
			GL.GenBuffers(1, out int allBufferVBO);

			// Buffer is "binded", following OpenGL commands refer to this buffer
			GL.BindBuffer(BufferTarget.ArrayBuffer, allBufferVBO);

			// Data is uploaded to graphics memory
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (verticies.Length * sizeof(float)), verticies,
				BufferUsageHint.StaticDraw);

			// BindBuffer to 0, so the following commands do not overwrite the current vbo (state machine)
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			// generating the index buffer
			GL.GenBuffers(1, out int indexBuffer);

			// Buffer is "binded", following OpenGL commands refer to this buffer
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);

			// index data is uploaded to grpahics mamory
			GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * indicies.Length), indicies,
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

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);

			GL.BindVertexArray(0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
	}
}