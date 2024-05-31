Shader "Lelious/URPUnlitShaderTextureScrolling"
{
    Properties
    { 
        _MainTex("Texture", 2D) = "white" {}
        _ShadeTex("ShadeTex", 2D) = "white" {}
        [HDR] _Color("Color", Color) = (1,1,1,1)
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("SrcBlend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("DstBlend", Float) = 0
        _RotationSpeed("RotationSpeed", float) = 1.0
        _uvScrollSpeed("UV_Scroll_Speed", Vector) = (0, 0, 0, 0)
    }

    SubShader
    {

        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        ZWrite Off
        //ZTest Off
        Cull Off
        Blend [_SrcBlend] [_DstBlend]
        ColorMask RGB
        LOD 100

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multicompile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                half4 positionOS : POSITION;
                half2 uv : TEXCOORD0;
                half2 uv1 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                half4 positionHCS  : SV_POSITION;
                half2 uv : TEXCOORD0;
                half2 uv1 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            TEXTURE2D(_ShadeTex);
            SAMPLER(sampler_ShadeTex);
            SAMPLER(sampler_MainTex);
            half2 _uvScrollSpeed;
            half _RotationSpeed;
            half4 _uvStretchSpeed;

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                half4 _MainTex_ST;
                half4 _ShadeTex_ST;
                half4 _BackTex_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.uv1 = OUT.uv;
                OUT.uv.xy = OUT.uv * 2 - 1;

                half c = cos(_Time.y * _RotationSpeed);
                half s = sin(_Time.y * _RotationSpeed);

                half2x2 mat = half2x2(c,-s,s,c);
                OUT.uv.xy = mul(mat, OUT.uv.xy);
                OUT.uv.xy = OUT.uv * 0.5 + 0.5;
                OUT.uv.xy += frac(_Time.yy * _uvScrollSpeed.xy * _MainTex_ST.xy);
                OUT.uv1.x = OUT.uv.x;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                half offset = IN.uv.y - frac(_Time.y * _uvScrollSpeed.y * _MainTex_ST.y);
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _Color;
                half4 col1 = SAMPLE_TEXTURE2D(_ShadeTex, sampler_ShadeTex, IN.uv1);
                return col * col1;
            }
            ENDHLSL
        }
    }
}
