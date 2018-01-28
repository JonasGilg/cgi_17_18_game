
#version 330 core
layout(location = 0) in vec3 position;

uniform mat4 fullTransformation;

    
    
    
void main()
{   
    
    
    gl_Position = fullTransformation * vec4(position.x,position.y,0.0,1.0);
    gl_PointSize = 10.0;
    
}
