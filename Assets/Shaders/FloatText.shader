Shader "Unlit/FloatText"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FirstNum("FirstNum", float) = 0
        _SecondNum("SecondNum", float) = 0
        _ThirdNum("ThirdNum", float) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back
            ZTest Off
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
                half2 uv2 : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                half2 uv : TEXCOORD0;
                half2 uv1 : TEXCOORD1;
                half2 uv2 : TEXCOORD2;
                half4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(half, _FirstNum)
                UNITY_DEFINE_INSTANCED_PROP(half, _SecondNum)
                UNITY_DEFINE_INSTANCED_PROP(half, _ThirdNum)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv1 = v.uv1;
                o.uv2 = v.uv2;

                o.uv.x /=3.33;
                o.uv1.x /=3.33;
                o.uv2.x /=3.33;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                half uvMask0 = 0.1 > i.uv.x;
                half uvMask1 = 0.2 > i.uv1.x;
                half uvMask2 = 0.3 > i.uv2.x;
                fixed4 col = tex2D(_MainTex, i.uv + half2(0 + UNITY_ACCESS_INSTANCED_PROP(Props, _FirstNum), 0));
                fixed4 col1 = tex2D(_MainTex, i.uv1 + half2(-0.1 + UNITY_ACCESS_INSTANCED_PROP(Props, _SecondNum), 0));
                fixed4 col2 = tex2D(_MainTex, i.uv2 + half2(-0.2 + UNITY_ACCESS_INSTANCED_PROP(Props, _ThirdNum), 0));
                col *= step(0, i.uv.x);
                col1 *= step(0.1, i.uv1.x);
                col2 *= step(0.2, i.uv2.x);
                return col * uvMask0 + col1 * uvMask1 + col2 * uvMask2;
            }
            ENDCG
        }
    }
}
