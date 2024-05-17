texture sampleTexture;
sampler2D samplerTex = sampler_state { texture = <sampleTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

texture noiseTexture;
sampler2D noiseTex = sampler_state { texture = <noiseTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

texture gradientTexture;
sampler2D gradientTex= sampler_state { texture = <gradientTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };;

float4 brighter_color : hint_color = float4(1.0, 0.4, 0.0, 1.0);
float4 middle_color : hint_color = float4(0.95, 0.2, 0.0, 1.0);
float4 darker_color : hint_color = float4(0.64, 0.2, 0.05, 1.0);

float spread = 0.5;
float uTime;
float uOpacity;

float4 MainPS(float2 uv : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(samplerTex, uv);
	float2 uv2 = uv + float2(0.0, uTime);

	float noise_value = tex2D(noiseTex, uv2);
	// .yx bc free gradient software can't do vertical
	float gradient_value = tex2D(gradientTex, uv.yx).x;

	gradient_value -= smoothstep(spread, spread + 0.5, length(uv + float2(-0.5, -0.5)) / spread);

	float step1 = smoothstep(noise_value, 1, gradient_value);
	float step2 = smoothstep(noise_value, 1, gradient_value - 0.2);
	float step3 = smoothstep(noise_value, 1, gradient_value - 0.4);

	float3 bd_color = lerp(brighter_color.rgb, darker_color.rgb, step1 - step2);

	color = float4(bd_color, step1) * color;
	color.rgb = lerp(color.rgb, middle_color.rgb, step2 - step3);
	if (color.a > 0)
		color.a = max(color.a - (uOpacity / 255), 0);

	return color;
}

technique BasicColorDrawing
{
	pass FlamesPass
	{
		PixelShader = compile ps_2_0 MainPS();
	}
};