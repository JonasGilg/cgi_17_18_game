#version 330
in vec4 in_position;

uniform mat4 mvp;


smooth out vec3 eyeDirection;

void main() {
    vec4 wvp_pos = mvp * in_position;
    eyeDirection = in_position.xyz;
    gl_Position = wvp_pos.xyww;
} 