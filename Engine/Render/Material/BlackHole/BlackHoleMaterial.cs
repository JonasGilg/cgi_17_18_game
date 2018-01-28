﻿using System;
using Engine.Model;
using Engine.Render;
using Engine.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public class BlackHoleMaterial : BaseMaterial {
		public int modelviewProjectionMatrixLocation;
		public int modelMatrixLocation;
		public int cameraPositionLocation;

		public BlackHoleMaterial() {
			CreateShaderProgram("Render/Material/BlackHole/BlackHole_VS.glsl",
				"Render/Material/BlackHole/BlackHole_FS.glsl");


			// GL.BindAttribLocation, gibt an welcher Index in unserer Datenstruktur welchem "in" Parameter auf unserem Shader zugeordnet wird
			// folgende Befehle müssen aufgerufen werden...
			GL.BindAttribLocation(Program, 0, "in_position");
			GL.BindAttribLocation(Program, 1, "in_normal");
			GL.BindAttribLocation(Program, 2, "in_uv");

			// ...bevor das Shader-Programm "gelinkt" wird.
			GL.LinkProgram(Program);

			// Die Stelle an der im Shader der per "uniform" der Input-Paremeter "modelview_projection_matrix" definiert wird, wird ermittelt.
			modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");
			modelMatrixLocation = GL.GetUniformLocation(Program, "model_matrix");
			cameraPositionLocation = GL.GetUniformLocation(Program, "camera_pos");
		
		}

		protected override void PreDraw() {
			GL.UseProgram(Program);

		}

		protected override void PostDraw() {
			GL.BindVertexArray(0);
		}

		protected override void Draw(Model3D model, MaterialSettings materialSettings) {
			GL.BindTexture(TextureTarget.Texture2D, materialSettings.ColorTexture);
			GL.BindVertexArray(model.VAO);

			var modelviewProjection = (model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection).ToFloat();
			GL.UniformMatrix4(modelviewProjectionMatrixLocation, false, ref modelviewProjection);
			IO.PrintAsync("mvp: " + modelviewProjection);
			var modelmatrix = model.Transformation.ToFloat();
			GL.UniformMatrix4(modelMatrixLocation, false, ref modelmatrix);
			IO.PrintAsync("model: " + modelmatrix);
			GL.Uniform3(cameraPositionLocation, DisplayCamera.Position.ToFloat());
			IO.PrintAsync("camera: " + DisplayCamera.Position.ToFloat());

			GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}
	}
}