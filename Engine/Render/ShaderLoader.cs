using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Engine.Render {
	public static class ShaderLoader {
		public static int LoadShader(string pathVs, string pathFs, string pathGs = null) {
			var vs = File.ReadAllText(pathVs);
			var fs = File.ReadAllText(pathFs);

			var vertexObject = GL.CreateShader(ShaderType.VertexShader);
			var fragmentObject = GL.CreateShader(ShaderType.FragmentShader);

			GL.ShaderSource(vertexObject, vs);
			GL.CompileShader(vertexObject);
			GL.GetShaderInfoLog(vertexObject, out var info);
			GL.GetShader(vertexObject, ShaderParameter.CompileStatus, out var statusCode);

			if (statusCode != 1)
				throw new ApplicationException(info);

			GL.ShaderSource(fragmentObject, fs);
			GL.CompileShader(fragmentObject);
			GL.GetShaderInfoLog(fragmentObject, out info);
			GL.GetShader(fragmentObject, ShaderParameter.CompileStatus, out statusCode);

			if (statusCode != 1)
				throw new ApplicationException(info);

			var program = GL.CreateProgram();
			GL.AttachShader(program, fragmentObject);
			GL.AttachShader(program, vertexObject);

			if (pathGs != null) {
				var gs = File.ReadAllText(pathGs);
				var geometryObject = GL.CreateShader(ShaderType.GeometryShader);

				GL.ShaderSource(geometryObject, gs);
				GL.CompileShader(geometryObject);
				GL.GetShaderInfoLog(geometryObject, out info);
				GL.GetShader(geometryObject, ShaderParameter.CompileStatus, out statusCode);

				if (statusCode != 1)
					throw new ApplicationException(info);

				GL.AttachShader(program, geometryObject);
			}

			return program;
		}
	}
}