#version 400

uniform sampler2D color_texture;
uniform sampler2D normalmap_texture;

uniform mat4 model_matrix;

uniform vec3 light_origin;
uniform vec4 light_ambient_color;
uniform vec4 light_diffuse_color;
uniform vec4 light_specular_color;
uniform vec4 camera_position;

uniform float specular_shininess;

in vec2 fragTexcoord;
in vec3 fragPosition;
in mat3 fragTBN;

in vec3 EyeDirection_cameraspace;
in vec3 LightDirection_cameraspace;

in vec3 LightDirection_tangentspace;
in vec3 EyeDirection_tangentspace;

out vec4 outputColor;

const float screenGamma = 2.2;

void main() {
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
    
    /*vec3 TextureNormal_tangentspace = normalize(texture(normalmap_texture, vec2(fragTexcoord.x, fragTexcoord.y)).rgb * 2.0 - 1.0);
    
    float distance = length(light_origin - fragPosition);

    vec3 n = TextureNormal_tangentspace;
    vec3 l = normalize(LightDirection_tangentspace);
    float cosTheta = clamp(dot(n, l), 0, 1);

    vec3 E = normalize(EyeDirection_tangentspace);
    vec3 R = reflect(-l, n);
    float cosAlpha = clamp(dot(E, R), 0, 1);
    
    outputColor = texture(color_texture, fragTexcoord) *
        (light_ambient_color +
        light_diffuse_color * cosTheta / (distance * distance) +
        light_specular_color * pow(cosAlpha, specular_shininess) / (distance * distance));*/


    /*vec3 normal = texture(normalmap_texture, fragTexcoord).rgb;
    normal = normalize(normal * 2.0 - 1.0); 
    normal = normalize(fragTBN * normal);
    
    vec3 lightDir = normalize(light_origin - fragPosition);
    
    vec4 v = normalize(camera_position - model_matrix * vec4(fragPosition, 1));
    
    vec3 h = normalize(lightDir + vec3(v));
    float ndoth = dot( normal, h );
    float specularIntensity = pow(ndoth, specular_shininess);

    float brightness = clamp(dot(normalize(normal), lightDir), 0, 1);
    
    vec4 surfaceColor = texture(color_texture, fragTexcoord);

    //				 Ambient						      + Diffuse 								          + Specular 
    // outputColor = (surfaceColor * light_ambient_color) + (surfaceColor * brightness * light_diffuse_color) + specularIntensity * light_specular_color;
    // upper line 
    outputColor = surfaceColor * (light_ambient_color +  brightness * light_diffuse_color) + specularIntensity * light_specular_color;*/
}