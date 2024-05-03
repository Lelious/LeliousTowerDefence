Shader "Lelious/HealthBarShader" {
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _Fill("Fill", float) = 0
        _ScaleX ("Scale X", Float) = 1.0
        _ScaleY ("Scale Y", Float) = 1.0
    }
        SubShader
        {
            Tags { "Queue" = "Overlay" }
            LOD 100
            Pass 
            {

            ZTest Off

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            half _ScaleX;
            half _ScaleY;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(half, _Fill)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert(appdata v) 
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.vertex = mul(UNITY_MATRIX_P, 
                           mul(UNITY_MATRIX_MV, half4(0.0, 0.0, 0.0, 1.0))
                         + half4(v.vertex.x, v.vertex.y, 0.0, 0.0)
                         * half4(_ScaleX, _ScaleY, 1.0, 1.0));
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target 
            {
                UNITY_SETUP_INSTANCE_ID(i);
                half fill = UNITY_ACCESS_INSTANCED_PROP(Props, _Fill);
                half healthbarMask = UNITY_ACCESS_INSTANCED_PROP(Props, _Fill) > i.uv.x;
                half3 healthbarColor = tex2D(_MainTex, half2(fill, i.uv.y));
                return half4(healthbarColor * healthbarMask, 1);
            }
            ENDCG
        }
    }
}
