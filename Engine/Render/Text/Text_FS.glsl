#version 330 core

in vec2 UV;

uniform sampler2D texSampler;

out vec4 color;

void main() {
    vec4 texValue = texture(texSampler, UV);

    if(length(texValue.rgb) < 0.5) discard;

	color = texValue;
}