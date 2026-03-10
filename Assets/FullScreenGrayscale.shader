Shader "FullScreen/Grayscale"
{
    Properties
    {
        _GrayscaleStrength ("Grayscale Strength", Range(0, 1)) = 1.0
    }

    HLSLINCLUDE

    #pragma vertex Vert

    #pragma target 2.0
    #pragma editor_sync_compilation

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

    // Luminance weights (ITU-R BT.709 standard)
    static const float3 LUMINANCE_WEIGHTS = float3(0.2126, 0.7152, 0.0722);

    CBUFFER_START(UnityPerMaterial)
        float _GrayscaleStrength;
    CBUFFER_END

    half4 Frag(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = input.texcoord;

        // Sample the source (camera) texture
        half4 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);

        // Compute luminance using BT.709 weights
        half luminance = dot(color.rgb, (half3)LUMINANCE_WEIGHTS);

        // Blend between original and grayscale based on strength
        half3 grayscale = half3(luminance, luminance, luminance);
        color.rgb = lerp(color.rgb, grayscale, _GrayscaleStrength);

        return color;
    }

    ENDHLSL

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        // Pass 0 — Full Screen Grayscale
        Pass
        {
            Name "FullScreen Grayscale"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment Frag
            ENDHLSL
        }
    }

    FallBack Off
}
