Shader "Unlit/VertexPaintedWithScroll"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _uvScrollSpeed("UVScrollSpeed", float) = 0
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
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed _uvScrollSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.color = v.color;
                o.uv.xy = TRANSFORM_TEX(v.uv.xy, _MainTex);
                o.uv.y -= _Time.y * _uvScrollSpeed;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv) + i.color;
                return col;
            }
            ENDCG
        }
    }
}
