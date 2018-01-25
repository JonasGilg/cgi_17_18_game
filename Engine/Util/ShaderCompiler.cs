using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Engine.Util {
	public class ShaderCompiler {
		public static int CreateShaderProgram(string pathVS, string pathFS) {
			var vs = File.ReadAllText(pathVS);
			var fs = File.ReadAllText(pathFS);

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

			var retProgram = GL.CreateProgram();
			GL.AttachShader(retProgram, fragmentObject);
			GL.AttachShader(retProgram, vertexObject);

			return retProgram;
		}
	}
}