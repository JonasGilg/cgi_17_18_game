#version 330 core
out vec4 outputColor;

in vec3 TexCoords;

uniform samplerCube skybox;

void main() {
    outputColor = texture(skybox, TexCoords);
}