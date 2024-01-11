
float4x4 View;
float4x4 Projection;

float ambientIntensity;
float3 DiffuseLightDirection;
float4 DiffuseLightColor;

float lineThickness;

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

	float4 ambience = ambientIntensity + 0.1f;

	float DiffuseIntensity = ambience * 10;

	// Use per instance World matrix to get World Pos
    float4 worldPosition = mul(input.Position, transpose(input.World));

	// Transform with camera view and projection to get screen pos
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.Colour = input.InstanceColour;
	ambience = output.Colour * ambience;
	output.Colour.rgb = saturate(output.Colour * ambience);

	float4 normal = mul(input.Normal, -worldPosition);
    float lightIntensity = dot(normal, DiffuseLightDirection);
	float4 lighting = saturate(DiffuseLightColor * DiffuseIntensity * lightIntensity);

	lighting = saturate(lighting + ambience);

	output.Colour.rgb = saturate(output.Colour * lighting);
	output.Colour.a = input.InstanceColour.a;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return input.Colour;
}

VertexShaderOutput BottomVertexShader(VertexShaderInput input)
{
    VertexShaderOutput Output = (VertexShaderOutput)0;

	float4 position = mul(mul(mul(input.Position, transpose(input.World)), View), Projection);
	Output.Position = position - lineThickness;
	Output.Colour = float4(0,0,0, input.InstanceColour.a);

    return Output;
}

float4 BottomPixelShader(VertexShaderOutput input) : COLOR0
{
    return input.Colour;
}

technique InstancePositionColour
{
	pass Pass1
    {
		VertexShader = compile vs_3_0 BottomVertexShader();
		PixelShader = compile ps_3_0 BottomPixelShader();
		CullMode = CW;
    }

    pass Pass2
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
		CullMode = CCW;
    }
}
