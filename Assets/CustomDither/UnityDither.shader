Shader "CustomEffects/PixelateShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"   
        }

        HLSLINCLUDE
        #pragma vertex vert
        #pragma fragment frag

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        static const int bayer2[2*2] = {
            0,2,
            3,1
        };

        static const int bayer4[4*4] = {
            0, 8, 2, 10,
            12, 4, 14, 6,
            3, 11, 1, 9,
            15, 7, 13, 5
        };

        static const int bayer8[8 * 8] = {
            0, 32, 8, 40, 2, 34, 10, 42,
            48, 16, 56, 24, 50, 18, 58, 26, 
            12, 44,  4, 36, 14, 46,  6, 38,
            60, 28, 52, 20, 62, 30, 54, 22, 
            3, 35, 11, 43,  1, 33,  9, 41, 
            51, 19, 59, 27, 49, 17, 57, 25,
            15, 47,  7, 39, 13, 45,  5, 37,
            63, 31, 55, 23, 61, 29, 53, 21
        };
    
        int _DitherLevel = 2;
        int _RedColorCount, _GreenColorCount, _BlueColorCount;

        float _Spread = 0.4f;

        struct Attributes
        {
            float4 positionOS : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionHCS : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        TEXTURE2D(_MainTex);
        float4 _MainTex_TexelSize;
        float4 _MainTex_ST;

        SamplerState sampler_point_clamp;

        Varyings vert(Attributes IN)
        {
            Varyings OUT;
            OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
            OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
            return OUT;
        }
       
        float GetBayer2(int x, int y) {
            return float(bayer2[(x % 2) + (y % 2) * 2]) * (1.0f / 4.0f) - 0.5f;
        }

        float GetBayer4(int x, int y) {
            return float(bayer4[(x % 4) + (y % 4) * 4]) * (1.0f / 16.0f) - 0.5f;
        }

        float GetBayer8(int x, int y) {
            return float(bayer8[(x % 8) + (y % 8) * 8]) * (1.0f / 64.0f) - 0.5f;
        }

        ENDHLSL

        Pass {
            Name "Pixelization"

            HLSLPROGRAM
            half4 frag(Varyings IN) : SV_TARGET
            {
                float2 blockCount = _ScreenParams.xy;
                float2 blockSize = 1 / _ScreenParams.xy;
                float2 halfBlockCount = 0.5 / _ScreenParams.xy;

                float2 blockPos = floor(IN.uv * blockCount);
                float2 blockCenter = blockPos * blockSize + halfBlockCount;

                float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, blockCenter);

                int x = blockCenter.x * _MainTex_TexelSize.z;
                int y = blockCenter.y * _MainTex_TexelSize.w;

                float ditherValues[3] = {0,0,0};
                ditherValues[0] = GetBayer2(x,y);
                ditherValues[1] = GetBayer4(x,y);
                ditherValues[2] = GetBayer8(x,y);

                float4 color = tex + _Spread * ditherValues[_DitherLevel];

                color.r = floor((_RedColorCount - 1.0f) * color.r + 0.5f) / (_RedColorCount - 1.0f);
                color.g = floor((_GreenColorCount - 1.0f) * color.g + 0.5f) / (_GreenColorCount - 1.0f);
                color.b = floor((_BlueColorCount - 1.0f) * color.b + 0.5f) / (_BlueColorCount - 1.0f);

                return color;
            }   
            ENDHLSL
        }
    }
}