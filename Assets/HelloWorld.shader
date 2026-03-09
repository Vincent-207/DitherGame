Shader "Basics/HelloWorld"
{
    Properties
    {
        // Variable name (" Property name for humans" , Type of property) = (Default value)
        // Color is on invertval [0, 1]
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
    }
    
    SubShader
    {
        // Tags contains key value pairs of data that determins how or when to use a given value 

        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        // when unity draws, typicaly multiple passes. 
        Pass
        {
            // Now writing in HLSL
            HLSLPROGRAM
            // HLSL is a c like language.
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            float4 _BaseColor;
            struct appdata
            {
                // name is meaningless. POSITION is meaningfull
                float positionOS : POSITION;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
            }

            ENDHLSL
        }
    }
}