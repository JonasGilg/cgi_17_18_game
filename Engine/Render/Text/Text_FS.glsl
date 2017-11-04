#version 330 core

in vec2 UV;

out vec4 color;

uniform vec4 textColor;
uniform sampler2D texSampler;

const float cutoff = 0.1;

void main(){
    vec4 tex = texture(texSampler, UV);
    float intensity = tex.length();
    
    if(intensity < cutoff) discard;
    
	color = textColor * vec4(1, 1, 1, intensity);
}