Shader "Lelious/UnlitTransparentCameraSpace"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("_Scale", Vector) = (1,1,0,0)
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
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            half4 _Color;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(half4, _Scale)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
                 UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.vertex = mul(UNITY_MATRIX_P, 
                           mul(UNITY_MATRIX_MV, half4(0.0h, 0.0h, 0.0h, 1.0h))
                         + half4(v.vertex.x, v.vertex.y, 0.0h, 0.0h)
                         * half4(UNITY_ACCESS_INSTANCED_PROP(Props, _Scale.x), UNITY_ACCESS_INSTANCED_PROP(Props, _Scale.y), 1.0h, 1.0h));
                
				o.uv = v.uv;
                return o;
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
