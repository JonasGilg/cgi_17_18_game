#version 400

in vec2 texcoord;
uniform sampler2D sampler; 

uniform float time;

out vec4 outputColor;

void main( ) {
    outputColor = texture2D(sampler, texcoord);
}