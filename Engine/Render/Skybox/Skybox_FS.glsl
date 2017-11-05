#version 330
uniform samplerCube uTexture;

in vec3 eyeDirection;

out vec4 fragmentColor;

void main() {
    fragmentColor = texture(uTexture, eyeDirection);
}