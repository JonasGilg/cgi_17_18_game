#version 330 core

in vec3 in_position;

uniform mat4 depthMVP;

void main() {
	gl_Position = depthMVP * vec4(in_position, 1);
}

