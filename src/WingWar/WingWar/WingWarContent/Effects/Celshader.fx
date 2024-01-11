float4x4 World;
float4x4 View;
float4x4 Projection;
float ambientIntensity;
float4 Colour;
float Line;

float4x4 WorldInverseTranspose;
float3 DiffuseLightDirection;
float4 DiffuseLightColor;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Normal : NORMAL0;
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

	

	float DiffuseIntensity = ambientIntensity * 10;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.Position3D = input.Position;

	float4 normal = mul(input.Normal, WorldInverseTranspose);
    float lightIntensity = dot(normal, DiffuseLightDirection);
    output.Color = saturate(DiffuseLightColor * DiffuseIntensity * lightIntensity);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	PixelToFrame Output = (PixelToFrame)0;    
    Output.Color = input.Color;

	float4 colour;
	colour = Colour;

	float4 ambience = colour * float4(ambientIntensity, ambientIntensity, ambientIntensity, 1.0f);

    return saturate(input.Color + ambience);
}

VertexShaderOutput BottomVertexShader(VertexShaderInput input)
{
	float LineThickness = Line;

    VertexShaderOutput Output = (VertexShaderOutput)0;

    float4 position = mul(mul(mul(input.Position, World), View), Projection);
	Output.Position = position - LineThickness;

    return Output;
}

float4 BottomPixelShader(VertexShaderOutput input) : COLOR0
{
    return float4(0,0,0,1);
}

VertexShaderOutput TopVertexShader(VertexShaderInput input)
{
	float LineThickness = Line;

    VertexShaderOutput Output = (VertexShaderOutput)0;

    float4 position = mul(mul(mul(input.Position, World), View), Projection);

	Output.Position = position + LineThickness;

    return Output;
}

float4 TopPixelShader(VertexShaderOutput input) : COLOR0
{
    return float4(0,0,0,1);
}

technique Technique1
{
    //pass Pass1
    //{
    //    VertexShader = compile vs_2_0 TopVertexShader();
	//	PixelShader = compile ps_2_0 TopPixelShader();
	//	CullMode = CW;
    //}

	pass Pass1
	{
		VertexShader = compile vs_2_0 BottomVertexShader();
		PixelShader = compile ps_2_0 BottomPixelShader();
		CullMode = CW;
	}

	pass Pass2
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
		CullMode = CCW;
	}

}

technique Technique2
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 TopVertexShader();
		PixelShader = compile ps_3_0 TopPixelShader();
		CullMode = CW;
    }

	pass Pass2
	{
		VertexShader = compile vs_3_0 BottomVertexShader();
		PixelShader = compile ps_3_0 BottomPixelShader();
		CullMode = CCW;
	}

	pass Pass3
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
		CullMode = CW;
	}

}
