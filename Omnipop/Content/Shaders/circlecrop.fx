#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float2 center = float2(0.5f, 0.5f); // Center of the circle in normalized coordinates
float radius = 0.5f; // Radius of the circle in normalized coordinates

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Calculate the distance from the center of the texture
    float2 position = input.TextureCoordinates - center;
    float dist = length(position);

    // Discard pixels outside the circle
    if (dist > radius)
    {
        discard;
    }

    return tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};