#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform vec4 tileColour;

uniform sampler2D texture0;
uniform sampler2D texture1; // Add more texture uniforms as needed

uniform int activeTexture; // New uniform to indicate active texture unit

void main()
{
    if (activeTexture == 0)
        outputColor = texture(texture0, texCoord);
    else if (activeTexture == 1)
        outputColor = texture(texture1, texCoord);
    // Add more conditions for additional textures

    if (outputColor.a < 0.5)
        outputColor = tileColour;
}
