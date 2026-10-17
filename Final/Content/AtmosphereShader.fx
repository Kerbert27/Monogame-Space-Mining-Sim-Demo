#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;
float3 cameraPosition;
float4 origin;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 UV : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
};

VertexShaderOutput PhongVertex(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.WorldPosition = mul(input.Position, World);
    float4 viewPosition = mul(output.WorldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.UV = input.UV;
    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float4 n = normalize(input.WorldPosition - origin);
	if(n.y < 0.1)
        return n;
    else
        return float4(0, 0, 0, 0);

}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL PhongVertex();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};