using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Engine.GUI;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Render.Billboard {
	public static class HudObjectMarkerRenderer {
		private static int PROGRAM;

		private static readonly int colorLocation,
			modelViewMatrixLocation,
			cameraRightLocation,
			cameraUpLocation,
			billboardSizeLocation,
			particleCenterLocation;
		
		
		private static Color4 colorVector = new Color4(0.0f,1.0f,0.0f,0.5f);
			

		static HudObjectMarkerRenderer() {
			
			PROGRAM = ShaderLoader.LoadShader("Render/ObjectMarker/ObjectMarker_VS.glsl",
				"Render/ObjectMarker/ObjectMarker_FS.glsl",
				"Render/ObjectMarker/ObjectMarker_GS.glsl"
				);
			GL.LinkProgram(PROGRAM);
			
			//colorLocation = GL.GetUniformLocation(PROGRAM, "colorVs");
			modelViewMatrixLocation = GL.GetUniformLocation(PROGRAM, "MVP");
			cameraRightLocation = GL.GetUniformLocation(PROGRAM, "CameraRight_worldspace");
			cameraUpLocation = GL.GetUniformLocation(PROGRAM, "CameraUp_worldspace");
			particleCenterLocation = GL.GetUniformLocation(PROGRAM, "particleCenter_worldspace");
			billboardSizeLocation = GL.GetUniformLocation(PROGRAM, "BillboardSize");
		}
		public static void Draw(HudObjectMarker objectMarker) {
			GL.UseProgram(PROGRAM);

			//create model
			double radiusd = objectMarker.GameObject.Radius;
			Vector3d position = objectMarker.GameObject.TransformComponent.WorldPosition;
			var points = createPoints(position, radiusd);
			var allData = new List<float>();
			for (var i = 0; i < points.Length; i++) {
				allData.Add((float) points[i].X);
				allData.Add((float) points[i].Y);
				allData.Add((float) points[i].Z);
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

//			// Buffer is "binded", following OpenGL commands refer to this buffer
//			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
//
//			// index data is uploaded to grpahics mamory
//			GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * allData.Count), allData.ToArray(),
//				BufferUsageHint.DynamicDraw);
//
//			// BindBuffer to 0, so the following commands do not overwrite the current element buffer (state machine)
//			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			
			
			// generating the Vertex-Array-Objects
			GL.GenVertexArrays(1, out int VAO);

			// Vertex-Array-Objekt is "binded", following OpenGL commands refer to this VAO. Inmportant for the folling two calls of "BindBuffer"
			GL.BindVertexArray(VAO);

			// BindBuffer commands: Are saved by our VAO.
			// Element-Buffer (indices)
			//GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);

			// ... then our interleaved VBO.
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, allBufferVBO);
			
			GL.EnableVertexAttribArray(1);

			var strideSize = Vector3d.SizeInBytes;
			
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, strideSize, 0);
			
			
				
			
				//loading mvp
			var modelMatrix = objectMarker.GameObject.TransformComponent.WorldMatrix;
			var viewMatrix = DisplayCamera.Transformation;
			var mvp =  DisplayCamera.PerspectiveProjection * viewMatrix * modelMatrix;
			var CameraRight_worldspace = new Vector3((float)viewMatrix.M11, (float)viewMatrix.M21, (float)viewMatrix.M31);
			var CameraUp_worldspace = new Vector3((float)viewMatrix.M12,(float)viewMatrix.M22,(float)viewMatrix.M32);
			var BillboardSize = new Vector3((float)radiusd+1,(float)radiusd+1,0);
			GL.UniformMatrix4(modelViewMatrixLocation, false, ref mvp);
			GL.Uniform3(cameraRightLocation, CameraRight_worldspace);
			GL.Uniform3(cameraUpLocation, CameraUp_worldspace);
			GL.Uniform3(particleCenterLocation, objectMarker.GameObject.TransformComponent.WorldPosition.ToFloat());
			GL.Uniform3(billboardSizeLocation, BillboardSize);
			
					
				

				
			GL.DrawElements(PrimitiveType.Lines, allData.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
			

			//FREE VAO
			GL.BindVertexArray(0);
		}

		private static Vector3d[] createPoints(Vector3d position, double radius) {
			

			var radiusFrac = radius * 2 / 3;
			
			 
			//4 edge points
			Vector3d[] resultEdges = new Vector3d[4];
			
			resultEdges[0] = new Vector3d(
				position.X + radius,
				position.Y + radius,
				position.Z
			);
			
			resultEdges[1] = new Vector3d(
				position.X - radius,
				position.Y + radius,
				position.Z
			);
			resultEdges[2] = new Vector3d(
				position.X - radius,
				position.Y - radius,
				position.Z
			);
			
			resultEdges[3] = new Vector3d(
				position.X + radius,
				position.Y - radius,
				position.Z
			);
			
			/*
			//8 outer lines
			Vector3d[] resultEdges = new Vector3d[16];
			//line1 top-left-top
			resultEdges[0] = new Vector3d(
				position.X - radius,
				position.Y + radius,
				0
				);
			
			resultEdges[1] = new Vector3d(
				position.X - radiusFrac,
				position.Y + radius,
				0
			);
			
			//line2 top-right-top
			resultEdges[2] = new Vector3d(
				position.X + radiusFrac,
				position.Y + radius,
				0
			);
			resultEdges[3] = new Vector3d(
				position.X + radius,
				position.Y + radius,
				0
			);
			
			//line 3 top-right-bottom
			resultEdges[4] = new Vector3d(
				position.X + radius,
				position.Y + radius,
				0
			);
			resultEdges[5] = new Vector3d(
				position.X + radius,
				position.Y + radiusFrac,
				0
			);
			
			//line 4 bottom-right-top
			resultEdges[6] = new Vector3d(
				position.X + radius,
				position.Y - radiusFrac,
				0
			);
			resultEdges[7] = new Vector3d(
				position.X + radius,
				position.Y - radius,
				0
			);
			
			//line 5 bottom-right-bottom
			resultEdges[8] = new Vector3d(
				position.X + radius,
				position.Y - radius,
				0
			);
			resultEdges[9] = new Vector3d(
				position.X + radiusFrac,
				position.Y - radius,
				0
			);
			
			//line 6 bottom-left-bottom
			resultEdges[10] = new Vector3d(
				position.X - radiusFrac,
				position.Y - radius,
				0
			);
			resultEdges[11] = new Vector3d(
				position.X - radius,
				position.Y - radius,
				0
			);
			
			//line 7 bottom-left-top
			resultEdges[12] = new Vector3d(
				position.X - radius,
				position.Y - radius,
				0
			);
			resultEdges[13] = new Vector3d(
				position.X - radius,
				position.Y - radiusFrac,
				0
			);
			
			//line 8 top-left-bottom
			resultEdges[14] = new Vector3d(
				position.X - radius,
				position.Y + radiusFrac,
				0
			);
			resultEdges[15] = new Vector3d(
				position.X - radius,
				position.Y + radius,
				0
			);
			*/
			
			
			
			

			return resultEdges;
		}
		
		private struct MarkerEdge {
			public Vector3d edgePoint;
			public Vector3d subPoint1;
			public Vector3d subPoint2;
		}
	}
}