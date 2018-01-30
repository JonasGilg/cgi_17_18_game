#version 330
precision highp float;

in vec3 in_position;
in vec3 in_normal; 
in vec2 in_uv; 

uniform mat4 modelview_projection_matrix;

void main() {
	gl_Position = modelview_projection_matrix * vec4(in_position, 1);

}


