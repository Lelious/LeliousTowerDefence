// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Lelious/NewSinShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Frequency ("Frequency", float) = 0
        _Amplitude ("Amplityde", float) = 0
        _Speed ("Speed", float) = 0
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
            #include "AutoLight.cginc"

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
            half _Frequency;
            half _Amplitude;
            half _Speed;

            v2f vert (appdata v)
            {
                v2f o;
           
                float angle= _Time.y * 2 * _Speed;
           
                v.vertex.y =  v.uv.x * sin(v.vertex.x * _Frequency + angle);
                v.vertex.y += sin(v.vertex.z + angle);
                v.vertex.y *= v.vertex.x * 0.1f;
           
                o.vertex = UnityObjectToClipPos( v.vertex );
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 color = tex2D(_MainTex, i.uv);
                return color;
            }
            ENDCG
        }
    }
}
