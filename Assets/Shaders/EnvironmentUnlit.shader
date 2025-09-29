Shader "Lelious/EnvironmentUnlit"
{
    Properties
    {
        _Mask ("MaskTexture", 2D) = "black" {}
        _Tex1 ("GrassTex", 2D) = "black" {}
        _Tex2 ("CliffTex", 2D) = "black" {}
        _Tex3 ("FloorTex", 2D) = "black" {}
        _TexEmission ("FloorEmission", 2D) = "black" {}
        [HDR] _EmissionColorH ("Inner Emission", Color) = (0,0,0,0)
        [HDR] _EmissionColorL ("Outer Emission", Color) = (0,0,0,0)
        _Saturation ("Saturation", float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;
                float4 vertex : SV_POSITION;
            };

            sampler2D _Mask, _Tex1, _Tex2, _Tex3, _TexEmission;
            half4 _Mask_ST, _Tex1_ST, _Tex2_ST, _Tex3_ST, _TexEmission_ST;
            half _Saturation;
            fixed4 _EmissionColorH, _EmissionColorL;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Mask);
                o.uv1 = TRANSFORM_TEX(v.uv1, _Tex1);
                o.uv2 = TRANSFORM_TEX(v.uv2, _Tex2);
                o.uv3 = TRANSFORM_TEX(v.uv3, _Tex3);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 mask = tex2D(_Mask, i.uv);
                fixed4 col = tex2D(_Tex1, i.uv1) * mask.r;
                fixed4 emission = tex2D(_TexEmission, i.uv3);
                emission *= mask.b * lerp(_EmissionColorL, _EmissionColorH, emission.r);
                col += tex2D(_Tex2, i.uv2) * mask.g;
                col += tex2D(_Tex3, i.uv3) * mask.b + emission;
                col *= 0.66;
                float gray = dot(col, float3(0.299, 0.587, 0.114));
                fixed3 grayColor = float3(gray, gray, gray);
                fixed3 saturatedColor = lerp(grayColor, col, _Saturation);
                return fixed4(saturatedColor, 1.0);
                //return col;
            }
            ENDCG
        }
    }
}
