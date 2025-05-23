// TileShader.fx
sampler2D TextureSampler : register(s0);

float4x4 MatrixTransform;
float4 LightDirection; // pass as a parameter (or use a float3 if you prefer)

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color    : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

struct PixelShaderInput
{
    float4 Position : SV_POSITION;
    float4 Color    : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

PixelShaderInput VertexMain(VertexShaderInput input)
{
    PixelShaderInput output;
    output.Position = mul(input.Position, MatrixTransform);
    output.TexCoord = input.TexCoord;
    output.Color = input.Color; // pass through the color
    return output;
}

float4 PixelMain(PixelShaderInput input) : COLOR0
{
    // sample the texture
    float4 texColor = tex2D(TextureSampler, input.TexCoord);

    // a very simple lighting computation:
    float lightFactor = saturate(dot(normalize(LightDirection.xyz), float3(0,0,1)));    
    // Combine the base texture with the input color and lighting value
    float4 finalColor = texColor * input.Color * lightFactor;
    return finalColor;
}

technique TileTechnique
{
    pass P0
    {
        VertexShader = compile vs_4_0_level_9_1 VertexMain();
        PixelShader  = compile ps_4_0_level_9_1 PixelMain();
    }
}
