using System;
using Engine.Model;
using Engine.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
    public class SunLavaflowMaterial : BaseMaterial{
        private readonly int modelviewProjectionMatrixLocation;
        public int timeLocation;

        public SunLavaflowMaterial() {
            // Shader-Programm wird aus den externen Files generiert...
            CreateShaderProgram("Render/Material/SunLavaflow/SunLavaflow_VS.glsl",
                "Render/Material/SunLavaflow/SunLavaflow_FS.glsl");

            // GL.BindAttribLocation, gibt an welcher Index in unserer Datenstruktur welchem "in" Parameter auf unserem Shader zugeordnet wird
            // folgende Befehle müssen aufgerufen werden...
            GL.BindAttribLocation(Program, 0, "in_position");
            GL.BindAttribLocation(Program, 1, "in_normal");
            GL.BindAttribLocation(Program, 2, "in_uv");

            // ...bevor das Shader-Programm "gelinkt" wird.
            GL.LinkProgram(Program);

            // Die Stelle an der im Shader der per "uniform" der Input-Paremeter "modelview_projection_matrix" definiert wird, wird ermittelt.
            modelviewProjectionMatrixLocation = GL.GetUniformLocation(Program, "modelview_projection_matrix");
            
            timeLocation = GL.GetUniformLocation(Program, "time");
            
        }
        
        
        public override void Draw(Model3D model, int textureId, float shininess = 0, int normalMap = -1) {
            // Textur wird "gebunden"
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            // das Vertex-Array-Objekt unseres Objekts wird benutzt
            GL.BindVertexArray(model.VAO);

            // Unser Shader Programm wird benutzt
            GL.UseProgram(Program);

            // Die Matrix, welche wir als "modelview_projection_matrix" übergeben, wird zusammengebaut:
            // Objekt-Transformation * Kamera-Transformation * Perspektivische Projektion der kamera.
            // Auf dem Shader wird jede Vertex-Position mit dieser Matrix multipliziert. Resultat ist die Position auf dem Screen.
            var modelviewProjection =
                (model.Transformation * DisplayCamera.Transformation * DisplayCamera.PerspectiveProjection).ToFloat();

            // Die Matrix wird dem Shader als Parameter übergeben
            GL.UniformMatrix4(modelviewProjectionMatrixLocation, false, ref modelviewProjection);
            GL.Uniform1(timeLocation, (float) Time.TotalTime);
            Console.WriteLine(Time.TotalTime);
            

            // Das Objekt wird gezeichnet
            GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            GL.BindVertexArray(0);
        }
    }
}