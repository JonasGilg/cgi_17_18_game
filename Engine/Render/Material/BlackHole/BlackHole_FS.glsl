#version 400

uniform sampler2D sampler;
uniform vec2 position;
uniform float radius;
uniform float ratio;
uniform float distance;

in vec2 texcoord;

out vec4 outputColor;

void main() {
    vec2 offset = texcoord - position; // We shift our pixel to the desired position
    vec2 calcRatio = vec2(ratio, 1.0f); // determines the aspect ratio
    float rad = length(offset / calcRatio); // the distance from the conventional "center" of the screen.
    float deformation = 1/pow(rad*pow(distance,0.5),2)*radius*2;
    
    offset =offset*(1-deformation);
    
    offset += position;
    
    outputColor = texture2D(sampler, offset);
    //if (rad*distance<pow(2*radius/distance,0.5)*distance) {outputColor.y+=0.2;} // verification of compliance with the Einstein radius
    //if (rad*distance<radius){outputColor.x=0;outputColor.y=0;outputColor.z=0;} // check radius BH
    
}