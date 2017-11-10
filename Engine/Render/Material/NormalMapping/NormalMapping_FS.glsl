#version 400

// basic texture and normalmap
uniform sampler2D color_texture;
uniform sampler2D normalmap_texture;

// "model_matrix"
uniform mat4 model_matrix;

// parameter for directional light
uniform vec3 light_origin;
uniform vec4 light_ambient_color;
uniform vec4 light_diffuse_color;
uniform vec4 light_specular_color;
uniform vec4 camera_position;

// parameter for "shininess"
uniform float specular_shininess;

// input from Vertex-Shader
in vec2 fragTexcoord;
in mat3 fragTBN;
in vec3 fragPosition;

// the final color
out vec4 outputColor;

const float screenGamma = 2.2;

void main() {
	// calculate normal from texture
    vec3 normal = texture(normalmap_texture, fragTexcoord).rgb;
	normal = normalize(normal * 2.0 - 1.0); 
	normal = normalize(fragTBN * normal); 

	vec3 lightDir = normalize(light_origin - fragPosition);
    
    float lambertian = max(dot(lightDir, normal), 0.0);
    float specular = 0.0;
    
    if (lambertian > 0.0) {
        vec3 viewDir = normalize(-fragPosition);
        vec3 halfDir = normalize(lightDir + viewDir);
        float specAngle = max(dot(halfDir, normal), 0.0);
        specular = pow(specAngle, specular_shininess);
    }
        
    vec4 colorLinear = light_ambient_color +
                       lambertian * light_diffuse_color +
                       specular * light_specular_color;
                           
    vec4 surfaceColor = texture2D(color_texture, fragTexcoord);
    vec4 colorGammaCorrected = pow(colorLinear, vec4(1.0 / screenGamma));
        
    outputColor = surfaceColor * colorGammaCorrected;
}