#version 330
precision highp float;

const float M_PI = 3.1415926535897932384626433832795;
const float M_PI_HALF = M_PI / 2;

const float screenGamma = 2.2;

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
    float lambertian = max(dot(light_direction, fragNormal), 0.0);
    float specular = 0.0;
    
    if(lambertian > 0.0) {
      vec4 viewDir = normalize(-fragPosition);
    
      vec3 halfDir = normalize(light_direction + vec3(viewDir));
      float specAngle = max(dot(halfDir, fragNormal), 0.0);
      specular = pow(specAngle, specular_shininess);
    }
    
    vec4 colorLinear = light_ambient_color +
                       lambertian * light_diffuse_color +
                       specular * light_specular_color;
                       
    // apply gamma correction (assume ambientColor, diffuseColor and specColor
    // have been linearized, i.e. have no gamma correction in them)
    vec4 colorGammaCorrected = pow(colorLinear, vec4(1.0 / screenGamma));
    
    vec4 surfaceColor = texture2D(sampler, fragTexcoord);
    
    // use the gamma corrected color in the fragment
    outputColor = surfaceColor * colorGammaCorrected;
}