sampler worldTexture : register(s0);
sampler overlayTexture : register(s1);
float globalTime;
float4 trailColor;
matrix uWorldViewProjection;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float3 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    output.Position = mul(input.Position, uWorldViewProjection);
    output.Position.z = 0;
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;
    
    output.TextureCoordinates.y = (output.TextureCoordinates.y - 0.5) / input.TextureCoordinates.z + 0.5;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // For most purposes, these are the two primitive data constructs you'll want to work with.
    // In this context, The X component of the UV vector corresponds to the length along the primitive (The start = 0, the end = 1, and everything in between), and the
    // Y component corresponds to the horizontal position on the primitives.
    float4 color = input.Color;
    float2 uv = input.TextureCoordinates;
    
    if (abs(uv.y - 0.5) < abs(.5 - uv.x / 2))
        return trailColor;
    else
        return tex2D(worldTexture, uv);
}

technique Technique1
{
    pass AutoloadPass
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
