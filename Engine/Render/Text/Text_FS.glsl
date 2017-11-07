#version 330 core

in vec2 UV;

uniform sampler2D texSampler;

out vec4 color;

void main() {
    vec4 texValue = texture(texSampler, UV);

    //if(texValue.rgb.length() < 3.0) discard;

	color = texValue;
}