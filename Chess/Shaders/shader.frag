#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform vec4 tileColour;

uniform sampler2D texture0;

void main()
{
    outputColor = texture(texture0, texCoord);
    if (outputColor.a < 0.5)
        outputColor = tileColour;
}