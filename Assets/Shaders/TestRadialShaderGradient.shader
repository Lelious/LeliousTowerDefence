Shader "Unlit/TestRadialShaderGradient"
{
    Properties
    {
        _Segments("Segments", Float) = 0
        _Saturation("Saturation", Float) = 0
        _Value("Value", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 500

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : TEXCOORD1;
            };

            float _Segments;
            float _Saturation;
            float _Value;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2.0 - 1.0;
                return o;
            }

            float3 hsv2rgb(float3 c)
            {
                float4 K = float4(1.0, 2.0/3.0, 1.0/3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float radius = length(uv);


                float angle = atan2(uv.y, uv.x);
                if(angle < 0) angle += 6.2831853;

                float hue = 1.0 - angle / 6.2831853;

                hue = floor(hue * _Segments) / _Segments;

                float3 pureColor = hsv2rgb(float3(hue, _Saturation, _Value));

                float3 colorMixed;

                float r1 = 0.3;
                float r2 = 0.7;

                if(radius < r1)
                {
                    float t = radius / r1;
                    colorMixed = lerp(float3(1.0, 1.0, 1.0), pureColor, t);
                }
                else if(radius < r2)
                {
                    colorMixed = pureColor;
                }
                else
                {
                    float t = (clamp(radius, 0, 1) - r2) / (1.0 - r2);
                    colorMixed = lerp(pureColor, float3(0,0,0), t * t);
                }

    return fixed4(colorMixed, 1.0);
            }
            ENDCG
        }
    }
}
