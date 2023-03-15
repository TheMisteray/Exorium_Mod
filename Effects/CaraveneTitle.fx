sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 MainPS(float2 uv : TEXCOORD0) : COLOR0
{
	float4 color = float4(0., 0., 0., 0.);

	float4 text = tex2D(uImage0, uv);

	float textArea = tex2D(uImage1, uv);
	float st = uv + float2(0, uTime);
	float normalVal = tex2D(uImage2, st).x;
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

	return color * textArea;
}

technique BasicColorDrawing
{
	pass ScreenTextPass
	{
		PixelShader = compile ps_2_0 MainPS();
	}
};