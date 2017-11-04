using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Engine.Render {
	public static class ShaderLoader {
		public static int LoadShader(string pathVs, string pathFs) {
			// shader files are read (text)
			var vs = File.ReadAllText(pathVs);
			var fs = File.ReadAllText(pathFs);

			// vertex and fragment shaders are created
			var vertexObject = GL.CreateShader(ShaderType.VertexShader);
			var fragmentObject = GL.CreateShader(ShaderType.FragmentShader);

			// compiling vertex-shader 
			GL.ShaderSource(vertexObject, vs);
			GL.CompileShader(vertexObject);
			GL.GetShaderInfoLog(vertexObject, out var info);
			GL.GetShader(vertexObject, ShaderParameter.CompileStatus, out var statusCode);

			if (statusCode != 1)
				throw new ApplicationException(info);

			// compiling fragment shader
			GL.ShaderSource(fragmentObject, fs);
			GL.CompileShader(fragmentObject);
			GL.GetShaderInfoLog(fragmentObject, out info);
			GL.GetShader(fragmentObject, ShaderParameter.CompileStatus, out statusCode);

			if (statusCode != 1)
				throw new ApplicationException(info);

			// final shader program is created using rhw fragment and the vertex program
			var program = GL.CreateProgram();
			GL.AttachShader(program, fragmentObject);
			GL.AttachShader(program, vertexObject);

			return program;
		}
	}
}