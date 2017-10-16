using System;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace CGI.Engine.Material {
	public class BaseMaterial {
		private int _vertexObject;
		private int _fragmentObject;

		protected int Program;

		protected void CreateShaderProgram(string pathVs, string pathFs) {
			// shader files are read (text)
			var vs = File.ReadAllText(pathVs);
			var fs = File.ReadAllText(pathFs);

			// vertex and fragment shaders are created
			_vertexObject = GL.CreateShader(ShaderType.VertexShader);
			_fragmentObject = GL.CreateShader(ShaderType.FragmentShader);

			// compiling vertex-shader 
			GL.ShaderSource(_vertexObject, vs);
			GL.CompileShader(_vertexObject);
			GL.GetShaderInfoLog(_vertexObject, out var info);
			GL.GetShader(_vertexObject, ShaderParameter.CompileStatus, out var statusCode);

			if (statusCode != 1)
				throw new ApplicationException(info);

			// compiling fragment shader
			GL.ShaderSource(_fragmentObject, fs);
			GL.CompileShader(_fragmentObject);
			GL.GetShaderInfoLog(_fragmentObject, out info);
			GL.GetShader(_fragmentObject, ShaderParameter.CompileStatus, out statusCode);

			if (statusCode != 1)
				throw new ApplicationException(info);

			// final shader program is created using rhw fragment and the vertex program
			Program = GL.CreateProgram();
			GL.AttachShader(Program, _fragmentObject);
			GL.AttachShader(Program, _vertexObject);

			// hint: "Program" is not linked yet
		}
	}
}