Shader "Unlit/RotationScrollUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _BaseColor ("BaseColor", Color) = (0,0,0,1)
        _RotationSpeed("RotationSpeed", float) = 1.0
        _uvScrollSpeed("UV_Scroll_Speed", Vector) = (0, 0, 0, 0)
        [HDR] _Color("Color", Color) = (1,1,1,1)
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("SrcBlend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("DstBlend", Float) = 0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend [_SrcBlend] [_DstBlend]
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing   

            #include "UnityCG.cginc"

            struct appdata
            {
                half4 vertex : POSITION;
                half2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                half2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _Color;
            half4 _BaseColor;
            half4 _uvScrollSpeed;
            half _RotationSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.uv.xy *= _Time.yy * _uvScrollSpeed.zz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv.xy, _MainTex);
                o.uv.xy = o.uv * 2 - 1;
                half c = cos(_Time.y * _RotationSpeed);
                half s = sin(_Time.y * _RotationSpeed);

                half2x2 mat = half2x2(c,-s,s,c);
                o.uv.xy = mul(mat, o.uv.xy);
                o.uv.xy = o.uv * 0.5 + 0.5;
                o.uv.xy += frac(_Time.yy * _uvScrollSpeed.xy * _MainTex_ST.xy);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                half4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
