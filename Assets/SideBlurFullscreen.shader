Shader "Custom/URP/SideBlurFullscreen"
{
    Properties
    {
        [Header(Blur Settings)]
        _BlurRadius ("Blur Radius", Range(0, 10)) = 3.0
        _BlurIntensity ("Blur Intensity", Range(0, 1)) = 1.0
        _BlurColor ("Blur Tint Color", Color) = (1, 1, 1, 1)
        _ColorBlend ("Color Blend Amount", Range(0, 1)) = 0.0
        
        [Header(Border Settings)]
        _BorderWidth ("Border Width (Pixels)", Range(0, 500)) = 200.0
        _FeatherSize ("Feather Size (Pixels)", Range(0, 300)) = 100.0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalPipeline"
        }
        
        LOD 100
        ZWrite Off
        ZTest Always
        Blend Off
        Cull Off

        Pass
        {
            Name "BorderBlurPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _BlurRadius;
            float _BlurIntensity;
            float _BorderWidth;
            float _FeatherSize;
            float4 _BlurColor;
            float _ColorBlend;

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                float2 uv = input.texcoord;
                float2 texelSize = float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y);
                
                // this calculates distance from all edges (in pixels)
                float2 pixelPos = uv * _ScreenParams.xy;
                
                // handles distance to each edge
                float distToLeft = pixelPos.x;
                float distToRight = _ScreenParams.x - pixelPos.x;
                float distToTop = pixelPos.y;
                float distToBottom = _ScreenParams.y - pixelPos.y;
                
                // finds the minimum distance to any edge
                float distToEdgeX = min(distToLeft, distToRight);
                float distToEdgeY = min(distToTop, distToBottom);
                float distToEdge = min(distToEdgeX, distToEdgeY);
                
                // creates a smooth border mask
                // 1.0 = at edge (full blur), 0.0 = center (no blur)
                float borderStart = _BorderWidth;
                float borderEnd = _BorderWidth - _FeatherSize;
                float edgeMask = 1.0 - smoothstep(borderEnd, borderStart, distToEdge);
                
                half4 originalColor = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);
                
   
                if (edgeMask < 0.001)
                {
                    return originalColor;
                }
                // horizontal blur
                half4 colorH = half4(0, 0, 0, 0);
                float2 offset;
                
                offset = float2(-6.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.002;
                
                offset = float2(-5.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.028;
                
                offset = float2(-4.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.065;
                
                offset = float2(-3.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.121;
                
                offset = float2(-2.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.175;
                
                offset = float2(-1.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.195;
                
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv) * 0.228;
                
                offset = float2(1.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.195;
                
                offset = float2(2.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.175;
                
                offset = float2(3.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.121;
                
                offset = float2(4.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.065;
                
                offset = float2(5.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.028;
                
                offset = float2(6.0 * _BlurRadius * texelSize.x, 0);
                colorH += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.002;
                
                // vertical blur
                half4 colorV = half4(0, 0, 0, 0);
                
                offset = float2(0, -6.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.002;
                
                offset = float2(0, -5.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.028;
                
                offset = float2(0, -4.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.065;
                
                offset = float2(0, -3.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.121;
                
                offset = float2(0, -2.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.175;
                
                offset = float2(0, -1.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.195;
                
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv) * 0.228;
                
                offset = float2(0, 1.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.195;
                
                offset = float2(0, 2.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.175;
                
                offset = float2(0, 3.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.121;
                
                offset = float2(0, 4.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.065;
                
                offset = float2(0, 5.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.028;
                
                offset = float2(0, 6.0 * _BlurRadius * texelSize.y);
                colorV += SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, saturate(uv + offset)) * 0.002;
                
                // combines horizontal and vertical blur
                half4 blurredColor = (colorH + colorV) * 0.5;
                
                // applies the tint of color
                blurredColor.rgb = lerp(blurredColor.rgb, blurredColor.rgb * _BlurColor.rgb, _ColorBlend);
                
                // mixes original and blurred (edge max + intensity basis)
                float finalMask = saturate(edgeMask * _BlurIntensity);
                half4 finalColor = lerp(originalColor, blurredColor, finalMask);
                
                return finalColor;
            }
            ENDHLSL
        }
    }
}
