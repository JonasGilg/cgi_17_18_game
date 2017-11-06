#version 330 core

in vec2 vertexPosition;
in vec2 vertexUV;

out vec2 UV;

void main(){
	gl_Position =  vec4(vertexPosition, 0, 1);
	UV = vertexUV;
}