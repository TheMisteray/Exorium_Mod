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

float4 Heat(float4 pos : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
	float2 targetCoords = (uTargetPosition - uScreenPosition) / uScreenResolution;
	float2 centerCoords = (coords - targetCoords) * (uScreenResolution / uScreenResolution.y);
	float dotField = dot(centerCoords, centerCoords);

	float2 distort;

	distort.x = (tex2D(uImage1, coords * uColor.x + float2(0, uTime)).r - 0.5f) * uColor.y;
	distort.y = (tex2D(uImage1, coords * uColor.x + float2(0, uTime)).g - 0.5f) * uColor.y;

	if (dotField < uColor.z)
	{
		distort.x *= sin(1.57 * (1 - dotField / uColor.z));
		distort.y *= sin(1.57 * (1 - dotField / uColor.z));
	}
	else
	{
		distort.x = 0;
		distort.y = 0;
	}

	float2 sampleCoords;
	sampleCoords.x = coords.x + ((distort.x * uOpacity / uScreenResolution.x) * centerCoords.x);
	sampleCoords.y = coords.y + ((distort.y * uOpacity / uScreenResolution.y) * centerCoords.y);

	return tex2D(uImage0, sampleCoords);
}

technique Technique1
{
	pass Heat
	{
		PixelShader = compile ps_2_0 Heat();
	}
};