texture sampleTexture;
sampler2D s1 = sampler_state { texture = <sampleTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

texture sampleTexture2;
sampler2D s2 = sampler_state { texture = <sampleTexture2>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

texture sampleTexture3;
sampler2D s3 = sampler_state { texture = <sampleTexture3>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

float uTime;
float4 sourceFrame;
float2 texSize;

const float2 centerPoint = float2(0.5, 0.5);

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
    float2 coords = uv;
    float4 color = tex2D(s1, coords);

    float2 coords2 = coords + float2(0, uTime);
    float2 coords3 = coords - float2(0, uTime);

    color.g = 0;
    color.b = 0;

    color.r -= tex2D(s2, coords2).x * 1.5;
    color.r -= tex2D(s2, coords3).x * .5;

    float2 centerToPixel = uv - centerPoint;

    float dist = length(centerToPixel);
    float val = tex2D(s3, uv + float2(0, uTime)).r / 10.;

    if (dist < 0.2 + val)
    {
        return color;
    }

    return float4(0., 0., 0., 0.);
}

technique BasicColorDrawing
{
    pass ShadePass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
};