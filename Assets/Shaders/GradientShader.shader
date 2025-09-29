Shader "Unlit/GradientShader"
{
    Properties
    {
        _EmissionColorH ("Emission Color High", Color) = (0,0,0,0)
        _EmissionColorL ("Emission Color Low", Color) = (0,0,0,0)
        _OffsetWeight ("Color Offset Weight", Range(0, 2)) = 0.0
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
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _EmissionColorH;
            float4 _EmissionColorL;
            fixed _OffsetWeight;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.color = lerp(_EmissionColorH, _EmissionColorL, v.uv.y * _OffsetWeight);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}
