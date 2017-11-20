#version 400

in vec2 texcoord;
uniform sampler2D sampler; 

uniform float time;

out vec4 outputColor;

void main( ) {
    outputColor = texture2D(sampler, vec2( texcoord.x * abs(sin(time/5000)), texcoord.y * abs(cos(time/5000))));
    //outputColor = vec4(outputColor.x * sin(int(time) % 180), outputColor.y * 0.8, outputColor.z * 0.8, 1);
}