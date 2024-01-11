float4x4 World;
float4x4 View;
float4x4 Projection;

float ambientIntensity;

struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
	float4 Position3D : TEXCOORD0;
};

struct PixelToFrame
{
    float4 Color        : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.Position3D = input.Position;

	output.Color.rb = 0.0f;
	output.Color.ga = 1.0f;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	PixelToFrame Output = (PixelToFrame)0;    
    Output.Color = input.Color;

	float4 base = float4(1, 1, 1, 1);
	float4 ambience = float4(ambientIntensity, ambientIntensity, ambientIntensity, 1);

    return base * ambience;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

