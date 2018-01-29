#version 330 core

layout(location = 0) in vec3 position;
uniform mat4 fullTransformation;

void main() {   
   
    gl_Position = fullTransformation * vec4(position, 1.0);
}
