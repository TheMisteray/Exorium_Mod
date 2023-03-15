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
	float4 OriginalColor = tex2D(uImage0, uv);
	float4 textArea = tex2D(uImage1, uv);

	float st = uv + float2(uProgress, uProgress);
	float normalVal = tex2D(uImage2, st).r;
	float invertedNormal = normalVal - .5;
	invertedNormal *= -1.;
	invertedNormal += .5;

	if (textArea.r > 0.)
	{
		//Dark red to bright orange
		textArea.r -= invertedNormal * 0.3;
		textArea.g -= 0.4;
		textArea.g -= normalVal * 0.6;
		textArea.b = 0;
	}

	if (uIntensity <= 60)
	{
		float4 Color = float4(1. - ((uIntensity-30.)/30.), 1. - ((uIntensity - 30.) / 30.), 1. - ((uIntensity - 30.) / 30.), 0.);
		textArea.r = min(textArea.r + Color.r, 1);
		textArea.g = min(textArea.g + Color.g, 1);
		textArea.b = min(textArea.b + Color.b, 1);
	}
	if (uIntensity <= 30)
	{
		float4 Color2 = float4(uIntensity/30., uIntensity / 30., uIntensity / 30., 0);
		OriginalColor.r = min(OriginalColor.r + Color2.r, 1);
		OriginalColor.g = min(OriginalColor.g + Color2.g, 1);
		OriginalColor.b = min(OriginalColor.b + Color2.b, 1);
		return OriginalColor;
	}

	return textArea;
}

technique BasicColorDrawing
{
	pass ScreenTextPass
	{
		PixelShader = compile ps_2_0 MainPS();
	}
};