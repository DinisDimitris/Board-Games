#version 330 core

layout(location = 0) in vec3 aPosition;

layout(location = 1) in vec3 aTexCoord;

uniform mat4 view;

uniform mat4 projection;

uniform vec3 offset;

out vec2 texCoord;

void main(void)
{
    texCoord = vec2(aTexCoord.x, aTexCoord.y);
    
    gl_Position = vec4(aPosition.x + offset.x, aPosition.y + offset.y, aPosition.z, 1.0) * view * projection;
}