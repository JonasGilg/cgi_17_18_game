using System;
using Engine.GUI;
using Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Render.Billboard {
	public static class HudObjectMarkerRenderer {
		private static int PROGRAM;

		static HudObjectMarkerRenderer() {
			
			PROGRAM = ShaderLoader.LoadShader("Render/Billboard/Billboard_VS.glsl", "Render/Billboard/Billboard_FS.glsl");
			GL.LinkProgram(PROGRAM);
		}

		public static void Draw(HudObjectMarker objectMarker) {
			
			
		}
	}
}