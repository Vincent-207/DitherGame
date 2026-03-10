Shader "Effects/Threshold"
{
    Properties
    {
        // _BlitTexture is bound automatically by Blitter — do not rename
        _BG ("Background", Color) = (0,0,0,1)
        _FG ("Foreground", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "Threshold"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _BG;
                half4 _FG;
            CBUFFER_END

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                half4 col = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, input.texcoord);
                col.rgb = lerp(_BG.rgb, _FG.rgb, round(col.r));
                return col;
            }
            ENDHLSL
        }
    }
}
