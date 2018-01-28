#version 400

uniform sampler2D sampler;

in vec2 texcoord;
in float R;

out vec4 outputColor;

void main() {
    
    vec4 col = texture2D(sampler, texcoord);
    outputColor = mix(col, vec4(1), R);
    
}