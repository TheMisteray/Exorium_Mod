texture sampleTexture;
sampler2D samplerTex = sampler_state { texture = <sampleTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

texture sampleTexture2;
sampler2D samplerTex2 = sampler_state { texture = <sampleTexture2>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

texture sampleTexture3;
sampler2D samplerTex3 = sampler_state { texture = <sampleTexture3>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

float uTime;
float4 sourceFrame;
float2 texSize;

const float2 centerPoint = float2(0.5, 0.5);

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
    float2 st = uv;
    float4 color = tex2D(samplerTex, st);

    float2 st2 = st + float2(uTime, uTime);

    float2 st3 = st - float2(uTime, uTime);

    color.r -= tex2D(samplerTex2, st2).x * .1;
    color.r += tex2D(samplerTex2, st3).x * .05;

    color.g -= .3;
    color.g -= tex2D(samplerTex2, st2).x * .7;
    color.g += tex2D(samplerTex2, st3).x * .35;

    color.b = 0;

    float2 centerToPixel = uv - centerPoint;

    float dist = length(centerToPixel);
    float val = tex2D(samplerTex3, uv + float2(0, uTime)).r / 15.;

    if (dist < 0.2 + val)
    {
        return color;
    }

    return float4(0., 0., 0., 0.);
}

technique BasicColorDrawing
{
    pass FlamingSpherePass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
};