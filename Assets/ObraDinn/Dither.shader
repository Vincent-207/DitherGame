Shader "Universal Render Pipeline/Lit/Dither"
{
    Properties
    {
        // _BlitTexture is bound automatically by Blitter — do not rename
        _Dither     ("Dither",     2D) = "white" {}
        _Noise      ("Noise",      2D) = "white" {}
        _ColorRamp  ("Color Ramp", 2D) = "white" {}
        _TL         ("Direction",  Vector) = (0,0,0,0)
        _BL         ("Direction",  Vector) = (0,0,0,0)
        _TR         ("Direction",  Vector) = (0,0,0,0)
        _BR         ("Direction",  Vector) = (0,0,0,0)
        _Tiling     ("Tiling",     Float)  = 192.0
        _Threshold  ("Threshold",  Float)  = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "Dither"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            // ── Textures & Samplers ───────────────────────────────────────
            TEXTURE2D(_Dither);     SAMPLER(sampler_Dither);
            TEXTURE2D(_Noise);      SAMPLER(sampler_Noise);
            TEXTURE2D(_ColorRamp);  SAMPLER(sampler_ColorRamp);

            float4 _Dither_TexelSize;
            float4 _Noise_TexelSize;

            // ── Material properties ───────────────────────────────────────
            CBUFFER_START(UnityPerMaterial)
                float4 _BL, _TL, _TR, _BR;
                float  _Tiling;
                float  _Threshold;
            CBUFFER_END

            // ── Projection helpers ────────────────────────────────────────
            float cubeProject(TEXTURE2D_PARAM(tex, samp), float2 texel, float3 dir)
            {
                float3x3 rotDirMatrix = {
                     0.9473740, -0.1985178,  0.2511438,
                     0.2511438,  0.9473740, -0.1985178,
                    -0.1985178,  0.2511438,  0.9473740
                };
                dir = mul(rotDirMatrix, dir);

                float2 uvCoords;
                if ((abs(dir.x) > abs(dir.y)) && (abs(dir.x) > abs(dir.z)))
                    uvCoords = dir.yz;
                else if ((abs(dir.z) > abs(dir.x)) && (abs(dir.z) > abs(dir.y)))
                    uvCoords = dir.xy;
                else
                    uvCoords = dir.xz;

                return SAMPLE_TEXTURE2D(tex, samp, texel * _Tiling * uvCoords).r;
            }

            // Returns depth discontinuity (0=flat, large=edge) using linear eye depth
            float EdgeFromDepth(float2 uv, float2 delta)
            {
                float2 offsets[4] = {
                    float2( 0,  1),
                    float2( 0, -1),
                    float2( 1,  0),
                    float2(-1,  0)
                };

                float centreLinear = LinearEyeDepth(SampleSceneDepth(uv), _ZBufferParams);
                float maxDepthDelta = 0;

                for (int i = 0; i < 4; i++)
                {
                    float2 offsetUV = uv + offsets[i] * delta;
                    float neighbourLinear = LinearEyeDepth(SampleSceneDepth(offsetUV), _ZBufferParams);
                    maxDepthDelta = max(maxDepthDelta, abs(centreLinear - neighbourLinear) / centreLinear);
                }

                return maxDepthDelta;
            }

            // ── Fragment ──────────────────────────────────────────────────
            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = input.texcoord;

                half4 col = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);

                float3 dir = normalize(lerp(
                    lerp(_BL.xyz, _TL.xyz, uv.y),
                    lerp(_BR.xyz, _TR.xyz, uv.y),
                    uv.x));

                float lum = col.b;

                float2 ditherTexel = _Dither_TexelSize.xy;
                float ditherLum = (col.a < 0.5)
                    ? cubeProject(TEXTURE2D_ARGS(_Noise,  sampler_Noise),  _Noise_TexelSize.xy,  dir)
                    : cubeProject(TEXTURE2D_ARGS(_Dither, sampler_Dither), ditherTexel,           dir);

                float2 edgeData = float2(EdgeFromDepth(uv, _BlitTexture_TexelSize.xy * 2.0), 0);

                // edgeData.x = relative depth discontinuity — tune _Threshold (try 0.05–0.2)
                bool isEdge = edgeData.x > _Threshold;
                lum = isEdge ? 0.0 : lum;

                float ramp = (lum <= clamp(ditherLum, 0.1, 0.9)) ? 0.1 : 0.9;
                float3 output = float3(1,1,1) * ramp;

                return half4(output, 1.0);
            }
            ENDHLSL
        }
    }
}
