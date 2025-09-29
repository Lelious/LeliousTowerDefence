Shader "Unlit/LeliousUnlitCutoutTransparent"
{
    Properties
    {
        _MainTex ("Base",   2D)    = ""{}
        _Color   ("Color",  Color) = (1, 1, 1, 1)
        _Cutoff  ("Cutoff", Range(0, 1)) = 0.5
        _WaveStrength ("Wave Strength", Range(0, 10)) = 0.0
        _WaveSpeed ("Wave Speed", Range(0, 10)) = 0.0
        _RenderLeaf ("Render Leaf", int) = 1
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }
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
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            fixed4 _MainTex_ST;
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(half4, _Color)
            UNITY_INSTANCING_BUFFER_END(Props)
            fixed _Cutoff;
            fixed _WaveStrength;
            fixed _WaveSpeed;
            fixed _RenderLeaf;

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                fixed time = _Time.y * _WaveSpeed;
                fixed3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                fixed distanceFromOrigin = length(v.vertex.xyz);
                fixed waveOffset = sin(worldPos.x * distanceFromOrigin * 10.0 + time + distanceFromOrigin) * _WaveStrength * distanceFromOrigin;
                fixed waveOffsetZ = cos(worldPos.y * distanceFromOrigin * 10.0 + time + distanceFromOrigin) * _WaveStrength * distanceFromOrigin;  
                o.position = UnityObjectToClipPos(v.vertex);
                o.position.x += waveOffset;
                o.position.y += waveOffsetZ;
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                fixed4 c = _RenderLeaf > 0 ? tex2D(_MainTex, i.texcoord) : fixed4(0, 0, 0, 0);
                fixed mask = i.texcoord.x > 0.01 ? 1 : 0;
                clip(c.a - _Cutoff * mask);
                fixed4 finalColor = mask > 0 ? c * UNITY_ACCESS_INSTANCED_PROP(Props, _Color) : i.color;
                return finalColor;
            }
            ENDCG
        }
    }  
    FallBack "Diffuse"
}
