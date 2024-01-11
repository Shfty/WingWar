float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

float AircraftType;
float PlayerIndex;
float ambientIntensity;
float Line;

float3 DiffuseLightDirection;
float3 LightPosition;

float4 ColorBase;
float4 ColorStripe;
float4 DiffuseLightColor;

float LightAttenuation = 5000;
float LightFalloff = 2;


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

	float DiffuseIntensity = ambientIntensity;

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
	
	float width = (input.Position3D.x);

	if (AircraftType == 0)
	{
		if (width < -5 || width > 5 || (width > -0.7 && width < 0.7))
		{
			colour = ColorStripe;
		}
		else
		{
			colour = ColorBase;
		}
	}

	if (AircraftType == 1)
	{
		if (width < -2.5 || width > 2.5)
		{
			colour = ColorStripe;
		}
		else
		{
			colour = ColorBase;
		}
	}

	float4 ambience = colour * float4(ambientIntensity, ambientIntensity, ambientIntensity, 1.0f);

	float d = distance(LightPosition, input.Position);
	float att = 1 - pow(clamp(d / LightAttenuation, 0, 1), LightFalloff);

	ambience * att;
	ambience.a = 1.0f;

    return saturate(input.Color + ambience);
}

VertexShaderOutput BottomVertexShader(VertexShaderInput input)
{
	float LineThickness = Line;

    VertexShaderOutput Output = (VertexShaderOutput)0;

    float4 position = mul(mul(mul(input.Position, World), View), Projection);
    float4 normal = mul(mul(mul(input.Normal, World), View), Projection);

	Output.Position = position - Line;

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
    float4 normal = mul(mul(mul(input.Normal, World), View), Projection);

	Output.Position = position + Line;

    return Output;
}

float4 TopPixelShader(VertexShaderOutput input) : COLOR0
{
    return float4(0,0,0,1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 TopVertexShader();
		PixelShader = compile ps_2_0 TopPixelShader();
		CullMode = CW;
    }

	pass Pass2
	{
		VertexShader = compile vs_2_0 BottomVertexShader();
		PixelShader = compile ps_2_0 BottomPixelShader();
		CullMode = CW;
	}

	pass Pass3
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
		VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
		CullMode = CW;
	}

}
