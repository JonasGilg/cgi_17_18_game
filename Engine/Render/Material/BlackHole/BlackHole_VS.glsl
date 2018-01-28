#version 400

// input aus der VAO-Datenstruktur
in vec3 in_position;
in vec3 in_normal;
in vec2 in_uv; 

// "modelview_projection_matrix" wird als Parameter erwartet, vom Typ Matrix4
uniform mat4 modelview_projection_matrix;
uniform mat4 model_matrix;
uniform vec3 camera_pos;

// "texcoord" wird an den Fragment-Shader weitergegeben, daher als "out" deklariert
out vec2 texcoord;
out float R;


void main() {
	
    texcoord = in_uv;

    vec3 posWorld = (model_matrix * vec4(in_position,1)).xyz;
    vec3 normWorld = normalize(mat3(model_matrix) * in_normal);

    vec3 I = normalize(posWorld - camera_pos);
    
    float bias = 0.0;
    float scale = 1.5;
    float power = 5.0;
    R =  bias + scale * pow(1.0 + dot(I, normWorld), power);
    
	// in gl_Position die finalan Vertex-Position geschrieben ("modelview_projection_matrix" * "in_position")
	gl_Position = modelview_projection_matrix * vec4(in_position, 1);
}