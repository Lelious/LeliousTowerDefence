Shader "Lelious/UnlitRotationScrolling"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadeTex("ShadeTex", 2D) = "white" {}
        [HDR] _Color("Color", Color) = (1,1,1,1)
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("SrcBlend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("DstBlend", Float) = 0
        _RotationSpeed("RotationSpeed", float) = 1.0
        _uvScrollSpeed("UV_Scroll_Speed", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        //ZTest Off
        Cull Off
        Blend [_SrcBlend] [_DstBlend]
        ColorMask RGB
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
                half2 uv1 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                half4 vertex : SV_POSITION;
                half2 uv : TEXCOORD0;
                half2 uv1 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            sampler2D _ShadeTex;
            half4 _MainTex_ST;
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(half4, _Color)
            UNITY_INSTANCING_BUFFER_END(Props)
            half2 _uvScrollSpeed;
            half _RotationSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv.xy, _MainTex);
                o.uv1 = o.uv;
                o.uv.xy = o.uv * 2 - 1;
                half c = cos(_Time.y * _RotationSpeed);
                half s = sin(_Time.y * _RotationSpeed);

                half2x2 mat = half2x2(c,-s,s,c);
                o.uv.xy = mul(mat, o.uv.xy);
                o.uv.xy = o.uv * 0.5 + 0.5;
                o.uv.xy += frac(_Time.yy * _uvScrollSpeed.xy * _MainTex_ST.xy);

                o.uv1.x = o.uv.x;
                return o;
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                half offset = i.uv.y - frac(_Time.y * _uvScrollSpeed.y * _MainTex_ST.y);
                half4 col = tex2D(_MainTex, i.uv) * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                col.a = i.vertex.z;
                half4 col1 = tex2D(_ShadeTex, i.uv1);
                return col * col1;
            }
            ENDCG
        }
    }
}
