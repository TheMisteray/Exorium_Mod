texture sampleTexture;
sampler2D samplerTex = sampler_state { texture = <sampleTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

texture sampleTexture2;
sampler2D samplerTex2 = sampler_state { texture = <sampleTexture2>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

float uTime;

float4 MainPS(float2 uv : TEXCOORD0) : COLOR0
{
	float4 color = float4(0., 0., 0., 0.);

	float4 text = tex2D(samplerTex, uv);

	float st = uv + float2(0, uTime);
	float normalVal = tex2D(samplerTex2, st).x;
	float invertedNormal = normalVal - .5;
	invertedNormal *= -1.;
	invertedNormal += .5;

	//Dark red to bright orange
	text.r -= invertedNormal * 0.3;
	text.g -= 0.4;
	text.g -= normalVal * 0.6;

	if (text.b = 1)
	{
		text.b = 0;
		color = text;
	}

	return color;
}

technique BasicColorDrawing
{
	pass ScreenTextPass
	{
		PixelShader = compile ps_2_0 MainPS();
	}
};