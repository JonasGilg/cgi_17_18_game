using System;
using Engine.GUI;
using OpenTK.Graphics.OpenGL;

namespace Engine.Render.Billboard {
	public static class HudObjectMarkerRenderer {
		private static readonly int PROGRAM;
		private static readonly int FULL_TRANSFORMATION_LOCATION;

		static HudObjectMarkerRenderer() {
			PROGRAM = ShaderLoader.LoadShader(
				"Render/ObjectMarker/ObjectMarker_VS.glsl",
				"Render/ObjectMarker/ObjectMarker_FS.glsl"
			);

			GL.BindAttribLocation(PROGRAM, 0, "position");
			GL.LinkProgram(PROGRAM);
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

			GL.DrawElements(PrimitiveType.Lines, objectMarker.Indicies.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.Enable(EnableCap.DepthTest);

			GL.BindVertexArray(0);
		}
	}
}