Shader "Unlit/ProgressPathUnlit"
{
    Properties
    {
        _ProgressScrollTex ("ProgressScrollTex", 2D) = "black" {}
        [HDR] _LockColor("Lock_Color", Color) = (0,0,0,0)
        [HDR] _UnlockColor("Unlock_Color", Color) = (0,0,0,0)
        [HDR] _TouchedColor("Touched_Color", Color) = (0,0,0,0)
        _Progress("Progress", Float) = 0
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

            sampler2D _ProgressScrollTex;
            float4 _ProgressScrollTex_ST;
            fixed4 _LockColor;
            fixed4 _UnlockColor;           
            fixed4 _TouchedColor;
            
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, _Progress)
                UNITY_DEFINE_INSTANCED_PROP(fixed, _IsTouched)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _ProgressScrollTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                half progressMask = UNITY_ACCESS_INSTANCED_PROP(Props, _Progress) > i.uv.y;
                fixed4 texCol = tex2D(_ProgressScrollTex, i.uv);
                fixed4 col = UNITY_ACCESS_INSTANCED_PROP(Props, _IsTouched) > 0 ? _TouchedColor : progressMask > 0 ? texCol + _UnlockColor : _LockColor;
                return col;
            }
            ENDCG
        }
    }
}
