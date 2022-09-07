#version 330 core

layout(location = 0) in vec3 aPosition;

uniform mat4 view;

uniform mat4 projection;

uniform vec3 offset;

void main(void)
{
    gl_Position = vec4(aPosition.x + offset.x, aPosition.y + offset.y, aPosition.z, 1.0) * view * projection;
}