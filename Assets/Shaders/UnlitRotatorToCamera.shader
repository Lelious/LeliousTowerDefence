Shader "Lelious/UnlitTransparentCameraSpace"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScaleX ("Scale X", Float) = 1.0
        _ScaleY ("Scale Y", Float) = 1.0
        [HDR] _Color("Color", Color) = (1,1,1)
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("SrcBlend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("DstBlend", Float) = 0
    }
    SubShader
    {
        Tags 
        {
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent"
        }

        Blend [_SrcBlend] [_DstBlend]
        ZWrite Off
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
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
            };

            sampler2D _MainTex;
            half4 _Color;
            half _ScaleX;
            half _ScaleY;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.vertex = mul(UNITY_MATRIX_P, 
                           mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0))
                         + float4(v.vertex.x, v.vertex.y, 0.0, 0.0)
                         * float4(_ScaleX + sin(_Time.y * 3), _ScaleY + sin(_Time.y * 3), 1.0, 1.0));
                //
				o.uv = v.uv;
                return o;
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
