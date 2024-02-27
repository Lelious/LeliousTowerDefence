Shader "Lelious/HealthBarShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Fill("Fill", float) = 0
    }
        SubShader{
            Tags { "Queue" = "Overlay" }
            LOD 100

            Pass {
                ZTest Off

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

            #pragma multi_compile_fog

            #pragma multi_compile_instancing


            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(half, _Fill)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert(appdata v) 
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                float fill = UNITY_ACCESS_INSTANCED_PROP(Props, _Fill);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target 
            {
                half healthbarMask = _Fill > i.uv.x;
                half3 healthbarColor = tex2D(_MainTex, half2(_Fill, i.uv.y));
                return half4(healthbarColor * healthbarMask, 1);
            }
            ENDCG
        }
    }
}
