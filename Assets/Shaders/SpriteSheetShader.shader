Shader "Unlit/SpriteSheetShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Columns("Columns", Float) = 1
        _Rows("Rows", Float) = 1
        _FramesPerSecond("FramesPerSecond", Float) = 0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        ZTest Off
        Blend One OneMinusSrcAlpha
        ColorMask RGB
        LOD 100

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
                float2 uv1 : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
            };

            sampler2D _MainTex;
            half _Columns;
            half _Rows;
            half _FramesPerSecond;

            half4 SetUvs(half4 uv)
            {
                half2 size = half2(1.0h / _Columns, 1.0h / _Rows);
                half totalFrames = _Columns * _Rows;
                half2 index;

                index.x = _Time.y * _FramesPerSecond;
                index.y = _Time.y * _FramesPerSecond + 1.0h;

                uint2 indexX = index.xy % _Columns;
				half2 indexY = floor((index.xy % totalFrames) / _Columns);
                half4 offset = half4(size.x * indexX.x, -size.y * indexY.x, size.x * indexX.y, -size.y * indexY.y);
                half4 newUV; 

                newUV.xy = uv.xy * size;
                newUV.zw = uv.zw * size;
				newUV.yw += size.y * (_Rows - 1);

				return newUV + offset;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                half4 uvs = SetUvs(half4(v.uv.x, v.uv.y, v.uv1.x, v.uv1.y));
				o.uv = uvs.xy;
				o.uv1 = uvs.zw;
                return o;
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col1 = tex2D(_MainTex, i.uv1);
                return lerp(col, col1, (_Time.y * _FramesPerSecond) % 1);
            }
            ENDCG
        }
    }
}
