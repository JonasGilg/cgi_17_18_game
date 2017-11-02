#version 330
precision highp float;

#define M_PI 3.1415926535897932384626433832795

uniform sampler2D sampler; 

// "model_matrix" Matrix
uniform mat4 model_matrix;

// parameter for direktional light
uniform vec3 light_direction;
uniform vec4 light_ambient_color;
uniform vec4 light_diffuse_color;
uniform vec4 light_specular_color;
uniform vec4 camera_position;

// parameter for the specalar-intensity - "Shininess"
uniform float specular_shininess;

// input from Vertex-Shader
in vec2 fragTexcoord;
in vec3 fragNormal;
in vec4 fragPosition;

// final output
out vec4 outputColor;

void main() {
	// get rotation of our models transform matrix
	mat3 normalMatrix = mat3(model_matrix);

	// calculate normal
    vec3 normal = normalize(normalMatrix * fragNormal);
    
    //angle between surface normal and light direction
    float angle = acos(dot(light_direction, normal));

    float specularIntensity = 0;
    float brightness = 0;
    
    //if the surface isn't facing the light source the diffuse and specular part stay zero.
    if(angle < M_PI / 2) {
        // calclulate view direction
        vec4 v = normalize(camera_position - fragPosition);
    
        // calculate halfway vector
        vec3 h = normalize(light_direction + vec3(v));
        float ndoth = dot(normal, h);
        specularIntensity = pow(ndoth, specular_shininess);
    
        // calculate brightness for diffuse light
        brightness = dot(normal, light_direction);
	}
	
	// surfaceColor is color from the texture
	vec4 surfaceColor = texture2D(sampler, fragTexcoord);

	//				 Ambiente color						  + Diffuse color									  + Speculare
    // outputColor = (surfaceColor * light_ambient_color) + (surfaceColor * brightness * light_diffuse_color) + specularIntensity * light_specular_color;
	// upper line put outside the brackets
	 outputColor = surfaceColor * (light_ambient_color + brightness * light_diffuse_color) + specularIntensity * light_specular_color;
}