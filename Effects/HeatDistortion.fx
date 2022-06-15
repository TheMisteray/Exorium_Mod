texture sampleTexture;
sampler2D samplerTex = sampler_state { texture = <sampleTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

texture distortTex;
sampler2D distortSamp = sampler_state { texture = <distortTex>; Filter = Linear; AddressU = wrap; AddressV = wrap; };

texture maskTex;
sampler2D maskSamp = sampler_state { texture = <maskTex>; Filter = Linear; AddressU = wrap; AddressV = wrap; };

float time, size, strength;

float4 Heat(float4 pos: SV_Position, float4 col : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
	float2 distort;

	distort.x = (tex2D(distortSamp, uv * strength + float2(0, time)).r - 0.5f) * size;
	distort.y = (tex2D(distortSamp, uv * strength + float2(0, time)).g - 0.5f) * size;
	distort *= tex2D(maskSamp, uv);
	return tex2D(TexSamp, uv + distort) * col;
}

technique HeatShader
{
	pass P0
	{
		PixelShader = compile ps_2_0 Heat();
	}
}