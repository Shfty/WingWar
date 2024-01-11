float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;
float3 DiffuseLightDirection;
float ambientIntensity;

float4 DiffuseLightColor;
float4 base = float4(0.4, 0.8, 0.4, 1);

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

	float DiffuseIntensity = ambientIntensity * 3;

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


	float height = (input.Position3D.y / 100) * 2;
	float4 Colour;

	if (input.Position3D.y >= -7.0f)
	{
		Colour = base + float4(height, height, height, 0) / 2.0f;
		Colour.a = 1.0f;
	}
	else if (input.Position3D.y <= -9.0f)
	{
		Colour = float4(0.7, 0.6, 0.4, 1);
	}
	else
	{
		Colour = float4(0.9, 0.8, 0.6, 1);
	}

	float4 ambience = Colour * float4(ambientIntensity, ambientIntensity, ambientIntensity, 1.0f);

    return saturate(input.Color + ambience);
}

VertexShaderOutput TopVertexShader(VertexShaderInput input)
{
    VertexShaderOutput Output;

	float LineThickness = 10.0f;
	float DiffuseIntensity = ambientIntensity * 2;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    Output.Position = mul(viewPosition, Projection) + LineThickness;
	Output.Position3D = input.Position;
	Output.Color = float4(1,1,1,1);

    return Output;
}

float4 TopPixelShader(VertexShaderOutput Input) : COLOR0
{
	float4 LColour = float4(0,0,0,1);
	LColour.a = 1.0f;
	float4 ambientLineColour = LColour * ambientIntensity;
	ambientLineColour.a = 1.0f;
	ambientLineColour = saturate(ambientLineColour);
	
    return ambientLineColour;
}

VertexShaderOutput BottomVertexShader(VertexShaderInput input)
{
    VertexShaderOutput Output;

	float LineThickness = 10.0f;
	float DiffuseIntensity = ambientIntensity * 2;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    Output.Position = mul(viewPosition, Projection) - LineThickness;
	Output.Position3D = input.Position;
	Output.Color = float4(1,1,1,1);

    return Output;
}

float4 BottomPixelShader(VertexShaderOutput Input) : COLOR0
{
	float4 LColour = float4(0,0,0,1);
	LColour.a = 1.0f;
	float4 ambientLineColour = LColour * ambientIntensity;
	ambientLineColour.a = 1.0f;
	ambientLineColour = saturate(ambientLineColour);
	
    return ambientLineColour;
}

technique Technique1
{
	pass Pass1
    {
		VertexShader = compile vs_2_0 TopVertexShader();
		PixelShader = compile ps_2_0 TopPixelShader();
		CullMode = CCW;
    }

	pass Pass2
	{
		VertexShader = compile vs_2_0 BottomVertexShader();
		PixelShader = compile ps_2_0 BottomPixelShader();
		CullMode = CCW;
	}

	pass Pass3
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
		CullMode = CW;
	}	 
}
