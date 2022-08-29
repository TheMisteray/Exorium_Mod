texture sampleTexture;
sampler2D samplerTex = sampler_state { texture = <sampleTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

texture sampleTexture2;
sampler2D samplerTex2 = sampler_state { texture = <sampleTexture2>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

float uTime;
float uProgress;

const float2 centerPoint = float2(0.5, 0.5);

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
	float2 st = uv;
	float4 color = tex2D(samplerTex, st);

	float2 centerToPixel = st - centerPoint;

	float2 dummy = { 0, 1 };
	dummy *= length(centerToPixel);

	float angleBetween = acos(dot(dummy, centerToPixel) / (length(dummy) * length(centerToPixel)));
	if (centerToPixel.x > 0)
		angleBetween *= -1;

	//Super jank way to do this but I was getting tired of this

	float rotationAmount = uTime + angleBetween;

	matrix<float, 2, 2> rotate = { cos(rotationAmount), -sin(rotationAmount),
								sin(rotationAmount), cos(rotationAmount) };


	float2 st2 = centerPoint + mul(dummy, rotate);

	float finalR = (tex2D(samplerTex2, st2).x + uProgress);
	finalR = finalR % 1;
	finalR = abs(finalR - .5f) * 2;

	color.r -= (finalR + .4f);
	color.g = color.r * 0.175f;
	color.b = color.r * 0.85f;

	return color;
}

technique BasicColorDrawing
{
	pass PortalPass
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
};