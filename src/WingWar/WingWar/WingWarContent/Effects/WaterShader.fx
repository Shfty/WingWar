float4x4 World;
float4x4 View;
float4x4 Projection;

float4 Color;
float ambientIntensity;

float xWaveLength;
float xWaveHeight;

float xTime;
float xWindForce;
float3 xWindDirection;
Texture xWaterBumpMap;

sampler WaterBumpMapSampler = sampler_state { texture = <xWaterBumpMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;

    float2 BumpMapSamplingPos        : TEXCOORD2;
    float4 Position3D                : TEXCOORD4;
};

struct PixelToFrame
{
    float4 Color        : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input, float4 inPos : POSITION, float2 inTex: TEXCOORD)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.BumpMapSamplingPos = inTex/xWaveLength;
	output.Position3D = mul(input.Position, World);

	float3 windDir = normalize(xWindDirection);    
    float3 perpDir = cross(xWindDirection, float3(0,1,0));
    float ydot = dot(inTex, xWindDirection.xz);
    float xdot = dot(inTex, perpDir.xz);
    float2 moveVector = float2(xdot, ydot);
    moveVector.y += xTime*xWindForce;    
    output.BumpMapSamplingPos = moveVector/xWaveLength; 

	output.Color.rb = 0.0f;
	output.Color.ga = 1.0f;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	PixelToFrame Output = (PixelToFrame)0;    
    
	float4 bumpColor = tex2D(WaterBumpMapSampler, input.BumpMapSamplingPos);
	float2 perturbation = xWaveHeight*(bumpColor.rg - 0.5f) * 2.0f;
	float4 dullColor = float4(0.3f, 0.3f, 0.5f, 0.2f);   

	Output.Color = lerp(bumpColor, dullColor, 0.2f);
	Output.Color.g * ambientIntensity;
	Output.Color.r * ambientIntensity;
	Output.Color.a = 0.2f;

	Output.Color * 0.01f;
	return Output.Color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();

		CullMode = CW;
    }
}