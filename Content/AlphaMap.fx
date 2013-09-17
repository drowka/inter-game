// this is the texture we are trying to render
uniform extern texture ScreenTexture;  
sampler screen = sampler_state 
{
	// get the texture we are trying to render.
    Texture = <ScreenTexture>;
};

// this is the alpha mapp texture, we set this from the C# code.
uniform extern texture MaskTexture;  
sampler mask = sampler_state
{
    Texture = <MaskTexture>;
};

// here we do the real work. 
float4 PixelShaderFunction(float2 inCoord: TEXCOORD0) : COLOR
{
	// we retrieve the color in the original texture at 
	// the current coordinate remember that this function 
	// is run on every pixel in our texture.
    float4 color = tex2D(screen, inCoord);

	//color.a = !(color.g > 0.7 && color.r < 0.3 && color.b < 0.3);
	color.a = 1 - exp((pow(1-color.g, 3) + pow(color.r,3) + pow(color.b, 3))*-2);
	
    return color;
}
 
technique
{
    pass P0
    {
		// The xbox can only run pixel shader 2.0 and for this 
		// purpose that is plenty for us too.
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}