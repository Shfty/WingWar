float4x4 View;
float4x4 Projection;

float ambientIntensity;

struct VertexShaderInput
{
	// PER VERTEX DATA
    float4 Position : POSITION0;
	float3 Normal : NORMAL0;

	// PER INSTANCE DATA
	float4x4 World : TEXCOORD5;
	float4 InstanceColour : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Colour : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	// Use per instance World matrix to get World Pos
    float4 worldPosition = mul(input.Position, transpose(input.World));

	// Transform with camera view and projection to get screen pos
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.Colour = saturate(input.InstanceColour * ambientIntensity);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return input.Colour;
}

technique InstancePositionColour
{
	pass Pass1
    {
		CullMode = CW;

        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
