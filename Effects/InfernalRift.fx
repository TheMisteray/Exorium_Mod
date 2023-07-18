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

float4 MainPS(float4 pos : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
	float targetX = (uTargetPosition.x - uScreenPosition.x) / uScreenResolution.x;
	float xDist = (coords.x - targetX) * (uScreenResolution / uScreenResolution.y);

	if (xDist < uColor.x * uProgress && xDist > uColor.x * -uProgress)
	{
		//Red inside
		float4 color = float4(1, 1, 1, 1);

		float2 st = coords;
		float2 st2 = st + float2(uIntensity, uIntensity);
		float2 st3 = st - float2(uIntensity, uIntensity);

		color.r -= tex2D(uImage1, st2).x * .1;
		color.r += tex2D(uImage1, st3).x * .05;

		color.g -= .3;
		color.g -= tex2D(uImage1, st2).x * .7;
		color.g += tex2D(uImage1, st3).x * .35;

		color.b = 0;

		return color;
	}
	else if (xDist < (uColor.x * uProgress) + uColor.y && xDist > (uColor.x * -uProgress) - uColor.y)
	{
		return float4(0, 0, 0, 0);
	}
	else
	{
		float2 sampleCoords;
		if (xDist < 0)//Grab from direction to opening
			sampleCoords.x = coords.x + ((uColor.x * uProgress) * .5f);
		else
			sampleCoords.x = coords.x - ((uColor.x * uProgress) * .5f);
		sampleCoords.y = coords.y;

		return tex2D(uImage0, sampleCoords);
	}
}

technique RiftDraw
{
	pass Rift
	{
		PixelShader = compile ps_2_0 MainPS();
	}
};