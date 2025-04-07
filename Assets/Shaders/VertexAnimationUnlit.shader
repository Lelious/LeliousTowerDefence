Shader "Unlit/VertexAnimationUnlit"
{
    Properties
    {
        _AnimationTex ("Texture", 2D) = "black" {}
        _FrameRate ("FrameRate", Float) = 24
        _AnimationSpeed("AnimationSpeed", Range(0, 1)) = 1
        _AdittionalColor("AditionalColor", Color) = (1,1,1,1)
    }
    SubShader
    {
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
                float4 position : POSITION;
                fixed4 color : COLOR;
                uint id : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                fixed4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(half, _AnimationSpeed)
                UNITY_DEFINE_INSTANCED_PROP(half, _AnimationTime)
                UNITY_DEFINE_INSTANCED_PROP(half4, _AdittionalColor)
                UNITY_DEFINE_INSTANCED_PROP(half, _AnimationOffset)
            UNITY_INSTANCING_BUFFER_END(Props)

            sampler2D _AnimationTex;
            fixed4 _AnimationTex_TexelSize;
            fixed _FrameRate;

            fixed4 CalculateVertexPosition(uint id)
            {
                fixed frameCount = _AnimationTex_TexelSize.z;
	            fixed duration = frameCount / _FrameRate;
	            fixed normalizedTime = (UNITY_ACCESS_INSTANCED_PROP(Props, _AnimationSpeed) * UNITY_ACCESS_INSTANCED_PROP(Props, _AnimationTime) / duration + UNITY_ACCESS_INSTANCED_PROP(Props, _AnimationOffset)) % 1;
	            fixed positionY = (id + 0.5) * _AnimationTex_TexelSize.y;
	            fixed2 positionUv = float2(normalizedTime, positionY);
	            return tex2Dlod(_AnimationTex, fixed4(positionUv.xy, 0, 0));
            }

            v2f vert(uint id:SV_VertexID, appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                v.id = id;
                o.color = v.color;
                fixed4 pos = CalculateVertexPosition(v.id);
                o.position = UnityObjectToClipPos(pos);
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                UNITY_SETUP_INSTANCE_ID(i);
                return i.color * UNITY_ACCESS_INSTANCED_PROP(Props, _AdittionalColor);
            }
            ENDCG
        }
    }
}
