#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 texCoord : TEXCOORD0;
};


float4 PSMain(float2 coords: TEXCOORD0) : COLOR0
{
	float4 col = tex2D(s0, coords);
	return col;
}

technique ChromaticAberation
{
	pass P0
	{
		PixelShader = compile ps_2_0 PSMain();
	}
};