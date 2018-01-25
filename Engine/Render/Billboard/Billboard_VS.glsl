#version 150
in vec4 gxl3d_Position;
in vec4 gxl3d_TexCoord0;

// GLSL Hacker automatic uniforms:
uniform mat4 gxl3d_ModelViewProjectionMatrix;
uniform mat4 gxl3d_ModelViewMatrix;
uniform mat4 gxl3d_ProjectionMatrix;
uniform mat4 gxl3d_ViewMatrix;
uniform mat4 gxl3d_ModelMatrix;

uniform int spherical; // 1 for spherical; 0 for cylindrical

out vec4 Vertex_UV;
void main()
{
  //mat4 modelView = gxl3d_ViewMatrix*gxl3d_ModelMatrix;
  mat4 modelView = gxl3d_ModelViewMatrix;
  
  // First colunm.
  modelView[0][0] = 1.0; 
  modelView[0][1] = 0.0; 
  modelView[0][2] = 0.0; 

  if (spherical == 1)
  {
    // Second colunm.
    modelView[1][0] = 0.0; 
    modelView[1][1] = 1.0; 
    modelView[1][2] = 0.0; 
  }

  // Thrid colunm.
  modelView[2][0] = 0.0; 
  modelView[2][1] = 0.0; 
  modelView[2][2] = 1.0; 
  
  vec4 P = modelView * gxl3d_Position;
  gl_Position = gxl3d_ProjectionMatrix * P;
  
  Vertex_UV = gxl3d_TexCoord0;
}
