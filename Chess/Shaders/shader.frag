#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform vec4 tileColour;

uniform sampler2D texture0;

uniform int activeTexture; // New uniform to indicate active texture unit

void main()
{
    if (activeTexture == 0)
        outputColor = texture(texture0, texCoord);
        
    if (activeTexture < 0) // No texture is active, use tile color directly
        outputColor = tileColour;
        

    if (outputColor.a < 0.5)
        outputColor = tileColour;
}
