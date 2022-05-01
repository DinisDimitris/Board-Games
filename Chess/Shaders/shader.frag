#version 330

out vec4 outputColor;

uniform vec4 tileColour;

void main()
{
    outputColor = tileColour;
}