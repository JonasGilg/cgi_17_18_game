
#version 330 core
layout(location = 0) in vec3 position;

uniform mat4 MVP;
uniform vec3 CameraRight_worldspace;
uniform vec3 CameraUp_worldspace;
uniform vec3 particleCenter_worldspace;
uniform vec3 BillboardSize;
    
    
    
void main()
{   
    
    vec3 vertexPosition = particleCenter_worldspace
                      + CameraRight_worldspace * position.x * BillboardSize.x
                      + CameraUp_worldspace * position.y * BillboardSize.y;
    
    gl_Position = MVP * vec4(vertexPosition,1);
    gl_PointSize = 10.0f;
    
}
