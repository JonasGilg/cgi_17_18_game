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

		private static readonly int FULL_TRANSFORMATION_LOCATION;


		private static Color4 colorVector = new Color4(0.0f, 1.0f, 0.0f, 0.5f);


		static HudObjectMarkerRenderer() {
			PROGRAM = ShaderLoader.LoadShader("Render/ObjectMarker/ObjectMarker_VS.glsl",
				"Render/ObjectMarker/ObjectMarker_FS.glsl" /*,
				"Render/ObjectMarker/ObjectMarker_GS.glsl"*/
			);

			GL.BindAttribLocation(PROGRAM, 0, "position");
			GL.LinkProgram(PROGRAM);
			//colorLocation = GL.GetUniformLocation(PROGRAM, "colorVs");
			FULL_TRANSFORMATION_LOCATION = GL.GetUniformLocation(PROGRAM, "fullTransformation");
		}

		public static void Draw(HudObjectMarker objectMarker) {
			GL.UseProgram(PROGRAM);

			GL.BindVertexArray(objectMarker.VAO);

			var worldToView = DisplayCamera.Transformation;
			var modelToWorld = objectMarker.GameObject.TransformComponent.WorldMatrix;
			var viewToProjection = DisplayCamera.PerspectiveProjection;
			
			var fullMatrix = ((modelToWorld * worldToView).ClearRotation() * viewToProjection).ToFloat();
			GL.UniformMatrix4(FULL_TRANSFORMATION_LOCATION, false, ref fullMatrix);

			GL.Disable(EnableCap.DepthTest);
			
			GL.DrawElements(PrimitiveType.Lines, objectMarker.indicies.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.Enable(EnableCap.DepthTest);
			
			GL.BindVertexArray(0);
		}
	}
}