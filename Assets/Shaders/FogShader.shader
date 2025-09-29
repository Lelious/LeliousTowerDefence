Shader "Unlit/FogShader"
{
   Properties
   {
        _FogTex ("Fog Texture", 2D) = "white" {}
        _Tiling ("Tiling", Float) = 0.1
        _Color ("Color", Color) = (1,1,1,1)
   }

   SubShader
   {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
            };

            sampler2D _FogTex;
            float4 _FogTex_ST;
            float _Tiling;
            float4 _Color;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS = TransformWorldToHClip(positionWS);
                OUT.positionWS = positionWS;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 uv = IN.positionWS.xz * _Tiling;
                float4 tex = tex2D(_FogTex, uv);
                float4 col = _Color * tex;

                return col;
            }
            ENDHLSL
        }
   }
}
