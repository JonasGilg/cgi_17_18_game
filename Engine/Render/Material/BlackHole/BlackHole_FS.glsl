#version 400

uniform sampler2D sampler; 

in vec2 texcoord;

out vec4 outputColor;

void main()
{
    outputColor = texture2D(sampler, texcoord);
}