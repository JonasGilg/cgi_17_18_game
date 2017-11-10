#version 330
uniform sampler2D sampler; 

uniform mat4 model_matrix;

uniform vec3 light_origin;
uniform vec4 light_ambient_color;
uniform vec4 light_diffuse_color;
uniform vec4 light_specular_color;

uniform float specular_shininess;

in vec2 fragTexcoord;
in vec3 normalInterp;
in vec3 vertPosition;

out vec4 outputColor;

const float screenGamma = 2.2;

void main() {
    vec3 normal = normalize(normalInterp);
    vec3 lightDir = normalize(light_origin - vertPosition);

    float lambertian = max(dot(lightDir, normal), 0.0);
    float specular = 0.0;
    
    if (lambertian > 0.0) {
        vec3 viewDir = normalize(-vertPosition);
        vec3 halfDir = normalize(lightDir + viewDir);
        float specAngle = max(dot(halfDir, normal), 0.0);
        specular = pow(specAngle, specular_shininess);
    }
    
    vec4 colorLinear = light_ambient_color +
                       lambertian * light_diffuse_color +
                       specular * light_specular_color;
                       
    vec4 surfaceColor = texture2D(sampler, fragTexcoord);
    vec4 colorGammaCorrected = pow(colorLinear, vec4(1.0 / screenGamma));
    
    outputColor = surfaceColor * colorGammaCorrected;
}