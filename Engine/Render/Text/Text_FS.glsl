#version 330 core

in vec2 UV;

uniform sampler2D texSampler;

out vec4 color;

void main() {
    vec3 texValue = texture(texSampler, UV).rgb;
    float length = length(texValue);
	color = vec4(texValue, length);
}