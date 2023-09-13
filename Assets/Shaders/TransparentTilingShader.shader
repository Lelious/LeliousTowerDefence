Shader "Lelious/TransparentTilingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 0, 0, 1)
        _ScrollSpeed("ScrollSpeed", float) = 0
        [Toggle] _ZWrite ("ZWrite", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("SrcBlend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("DstBlend", Float) = 0
        [HDR] _EmissionColor ("Emission Color", Color) = (0,0,0)
        _Threshold ("Threshold", Range(0., 1.)) = 1.
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
                    ZWrite [_ZWrite]
                    Blend [_SrcBlend] [_DstBlend]
        LOD 100
        Cull off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _ALPHABLEND_ON

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _EmissionColor;
            half _ScrollSpeed;
            half _Threshold;

            v2f vert (appdata v)
            {
                v2f o;
               _MainTex_ST.w -= _Time.y * _ScrollSpeed % 6;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;      
                fixed3 emi = tex2D(_MainTex, i.uv).r * _EmissionColor.rgb * _Threshold;
                col.rgb += emi;
                return col;
            }
            ENDCG
        }
    }
}
