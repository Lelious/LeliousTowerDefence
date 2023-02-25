Shader "Lelious/HorizontalGradientSky"
{

    Properties
    {
        _Color ("Top Color", Color) = (1,1,1,1)
        _Color1 ("Bottom Color Color", Color) = (1,1,1,1)
        _Texture ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
        {
            "RenderType"="Opaque" 
        }
        Pass
        {
            HLSLPROGRAM

            #pragma vertex vertexProgram
            #pragma fragment fragmentProgram
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct vertexInput
            {                
                float4 positionOS : POSITION;
            };

            struct fragmentInput
            {                
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD2;
            };

            struct fragmentOutput
            {                
                float4 color : SV_Target;
            };
           
            float4 _Color;
            float4 _Color1;
            TEXTURE2D(_Texture);
            SAMPLER(sampler_Texture);
            float4 _Texture_ST;

            fragmentInput vertexProgram(vertexInput data)
            {
                fragmentInput output;
                output.positionCS = TransformObjectToHClip(data.positionOS);
                return output;
            }

            fragmentOutput fragmentProgram(fragmentInput input)
            {

                float2 screenUV = input.positionCS.xy / input.positionCS.w;
                //fixed4 c = tex2D (_MainTex, input.uv_MainTex) * lerp(_Color, _Color1, screenUV.y);
                fragmentOutput output;
                output.color = lerp(_Color, _Color1, screenUV.y);
                //o.Albedo = c.rgb;
                //o.Alpha = c.a;
                return output;
            }

            //void surf (Input IN, inout SurfaceOutputStandard o)
            //{
                
            //}
        
            ENDHLSL
        }        
    }
    FallBack "VertexLit"
}
