#version 400

in vec3 in_position;
in vec3 in_normal; 
in vec2 in_uv;
in vec3 in_tangent;
in vec3 in_bitangent;

uniform mat4 modelview_projection_matrix;
uniform mat4 model_matrix;
uniform mat3 model_view_3x3_matrix;

uniform mat4 view_matrix;
uniform vec3 light_origin;

out vec2 fragTexcoord;
out vec3 fragPosition;
out mat3 fragTBN;

out vec3 EyeDirection_cameraspace;
out vec3 LightDirection_cameraspace;

out vec3 LightDirection_tangentspace;
out vec3 EyeDirection_tangentspace;

void main() {
	vec3 T = normalize(vec3(model_matrix * vec4(in_tangent, 0.0)));
    vec3 B = normalize(vec3(model_matrix * vec4(in_bitangent, 0.0)));
    vec3 N = normalize(vec3(model_matrix * vec4(in_normal, 0.0)));
    fragTBN = mat3(T, B, N);

	fragTexcoord = in_uv;

    vec4 vertPosition4 = model_matrix * vec4(in_position, 1.0);
	fragPosition =  vec3(vertPosition4) / vertPosition4.w;

	gl_Position = modelview_projection_matrix * vec4(in_position, 1);
	
	/*gl_Position =  modelview_projection_matrix * vec4(in_position, 1);
    
    vec4 vertPosition4 = model_matrix * vec4(in_position, 1.0);
    fragPosition =  vec3(vertPosition4) / vertPosition4.w;
    
    vec3 vertexPosition_cameraspace = (view_matrix * model_matrix * vec4(in_position, 1)).xyz;
    EyeDirection_cameraspace = vec3(0, 0, 0) - vertexPosition_cameraspace;

    vec3 LightPosition_cameraspace = (view_matrix * vec4(light_origin, 1)).xyz;
    LightDirection_cameraspace = LightPosition_cameraspace + EyeDirection_cameraspace;
    
    fragTexcoord = in_uv;
    
    vec3 vertexTangent_cameraspace = model_view_3x3_matrix * in_tangent;
    vec3 vertexBitangent_cameraspace = model_view_3x3_matrix * in_bitangent;
    vec3 vertexNormal_cameraspace = model_view_3x3_matrix * in_normal;
    
    fragTBN = transpose(mat3(
        vertexTangent_cameraspace,
        vertexBitangent_cameraspace,
        vertexNormal_cameraspace	
    ));

    LightDirection_tangentspace = fragTBN * LightDirection_cameraspace;
    EyeDirection_tangentspace =  fragTBN * EyeDirection_cameraspace;*/
}


