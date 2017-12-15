#version 400

uniform sampler2D fbo_texture;
uniform float offset;
in vec2 f_texcoord;
out vec4 outputColor;

void main(void) {
  vec2 texcoord = f_texcoord;
  texcoord.x += sin(texcoord.y * 4*2*3.14159 + offset) / 100;
  outputColor = texture2D(fbo_texture, texcoord);
}