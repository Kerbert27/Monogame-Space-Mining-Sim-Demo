#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World; // World Matrix
float4x4 View; // View Matrix
float4x4 Projection; // Projection Matrix
float3 LightPosition; // in world space
float3 CameraPosition; // in world space
float Shininess; // scalar value
float3 AmbientColor;
float3 DiffuseColor;
float3 SpecularColor;
float Time;
texture DiffuseTexture;

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

texture NormalMap;
sampler NormalSampler = sampler_state
{
    Texture = <NormalMap>;
    AddressU = Wrap;
    AddressV = Wrap;
};
sampler DiffuseSampler = sampler_state
{
    Texture = <DiffuseTexture>;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 Pixel(VertexShaderOutput input) : COLOR0
{
    float3 normal = tex2D(NormalSampler, input.UV).xyz;
    normal = normalize(normal);

    //worldNorm = normalize(worldNorm);
    //return float4(worldNorm, 1);
    
    float3 viewDirection = normalize(CameraPosition - input.WorldPosition.xyz);
    float3 lightDirection = normalize(LightPosition - input.WorldPosition.xyz);
    float3 worldNorm = mul(normal, normalize(input.WorldPosition));
    float3 reflectDirection = reflect(lightDirection, normal);
    //float3 d = dot(-lightDirection, normalize(input.WorldPosition).xyz) * worldNorm;
    //return float4(d, 1);
    
    float3 diffuse = max(dot(-lightDirection, normalize(input.WorldPosition).xyz), 0) * tex2D(DiffuseSampler, input.UV);
    float specular = pow(max(dot(reflectDirection, viewDirection), 0), Shininess);
    return float4(AmbientColor + diffuse * DiffuseColor + specular * SpecularColor, 1);
}

float4 SkyboxPixel(VertexShaderOutput input) : COLOR0
{
    return tex2D(DiffuseSampler, input.UV);
}

float4 AtmospherePixel(VertexShaderOutput input) : COLOR0
{
    float3 viewDirection = normalize(CameraPosition - input.WorldPosition.xyz);
    float3 lightDirection = normalize(LightPosition - input.WorldPosition.xyz);
    float3 worldNorm = normalize(input.WorldPosition);
    
    
    return float4(worldNorm, 0.1);

}

float4 LaserPixel(VertexShaderOutput input) : COLOR0
{
    float4 c1 = float4(1, 0, 0, 1);
    float4 c2 = float4(0, 1, 0, 1);
    return lerp(c1, c2, Time % 1);

}



technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL PhongVertex();
		PixelShader = compile PS_SHADERMODEL Pixel();
	}
};

technique Skybox
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL PhongVertex();
        PixelShader = compile PS_SHADERMODEL SkyboxPixel();
    }
}

technique Laser
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL PhongVertex();
        PixelShader = compile PS_SHADERMODEL LaserPixel();
    }
}

technique Atmosphere
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL PhongVertex();
        PixelShader = compile PS_SHADERMODEL AtmospherePixel();
    }
}