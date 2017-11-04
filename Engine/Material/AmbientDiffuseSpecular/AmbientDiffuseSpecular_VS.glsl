#version 330
uniform mat4 model_matrix;

in vec3 in_position;
in vec3 in_normal;
in vec2 in_uv;

uniform mat4 modelview_projection_matrix;

out vec2 fragTexcoord;
out vec3 normalInterp;
out vec3 vertPosition;

void main() {
	fragTexcoord = in_uv;
	normalInterp = vec3(model_matrix * vec4(in_normal, 0.0));
    vec4 vertPosition4 = model_matrix * vec4(in_position, 1.0);
	vertPosition =  vec3(vertPosition4) / vertPosition4.w;

	gl_Position = modelview_projection_matrix * vec4(in_position, 1.0);
}




