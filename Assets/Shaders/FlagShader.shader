Shader "Unlit/FlagShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed("Speed", Range(0, 100)) = 1
		_Frequency("Frequency", Range(0, 1.3)) = 1
		_Amplitude("Amplitude", Range(0, 5.0)) = 1
        _DimensionX("DimentionX", Range(0, 1)) = 0
        _DimensionZ("DimentionZ", Range(0, 1)) = 0     
        _Rand("Rand", float) = 0
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

            sampler2D _MainTex;
		    float4 _MainTex_ST;
            half _Speed;
			half _Frequency;
			half _Amplitude;
            half _DimensionX;
            half _DimensionZ;

            struct v2f 
            {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
			};

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, _Rand)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert(appdata_base v)
			{
				v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

				v.vertex.y += cos((v.vertex.x + UNITY_ACCESS_INSTANCED_PROP(Props, _Rand) + _Time.y * _Speed) * _Frequency) * _Amplitude * _DimensionX * v.vertex.x;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return tex2D(_MainTex, i.uv);
			}
            ENDCG
        }
    }
}
