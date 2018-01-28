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

		private static readonly int fullTransformationLocation;
		
		
		private static Color4 colorVector = new Color4(0.0f,1.0f,0.0f,0.5f);
			

		static HudObjectMarkerRenderer() {
			
			PROGRAM = ShaderLoader.LoadShader("Render/ObjectMarker/ObjectMarker_VS.glsl",
				"Render/ObjectMarker/ObjectMarker_FS.glsl",
				"Render/ObjectMarker/ObjectMarker_GS.glsl"
				);
			
			GL.BindAttribLocation(PROGRAM, 0, "position");
			GL.LinkProgram(PROGRAM);
			//colorLocation = GL.GetUniformLocation(PROGRAM, "colorVs");
			fullTransformationLocation = GL.GetUniformLocation(PROGRAM, "fullTransformation");
			
			
		}
		public static void Draw(HudObjectMarker objectMarker) {
			Console.Out.WriteLine("MarkPosition" +objectMarker.GameObject.TransformComponent.WorldPosition + "   "+objectMarker.id);
			GL.UseProgram(PROGRAM);
			
			GL.BindVertexArray(objectMarker.VAO);

			var scale =  objectMarker.GameObject.TransformComponent.Scale;

			var model_to_world = objectMarker.GameObject.TransformComponent.WorldMatrix;
			var world_to_view = DisplayCamera.Transformation;
			var view_to_projection = DisplayCamera.PerspectiveProjection.ClearRotation();

			var fullMatrix =   view_to_projection * world_to_view * model_to_world;
				
			GL.UniformMatrix4(fullTransformationLocation,false, ref fullMatrix );
			GL.DrawElements(PrimitiveType.Lines, objectMarker.indicies.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

			GL.BindVertexArray(0);
		}
		

	}
}