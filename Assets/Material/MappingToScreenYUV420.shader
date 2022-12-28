Shader "MappingToScreenYUV420"
{
    Properties
    {
		_YTex ("Y Texture", 2D) = "Black" {}
		_UVTex ("UV Texture", 2D) = "Black" {}
        _Float("Float", Float) = 1
        _Offset("Offset", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
            
           #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 pos : TEXCOORD0;
            };

			sampler2D _YTex;
			float4 _YTex_ST;
			sampler2D _UVTex;
            float _Float;
            float _Offset;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 pos = ComputeScreenPos(o.vertex);
                o.pos = float4((- pos.y) * _Float + _Offset, - pos.x, pos.z, pos.w);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
				float3x3 yuv2rgb = float3x3(
					1.164f,  1.596f,  0.0f,
					1.164f, -0.813f, -0.391f,
					1.164f,  0.0f,    2.018f);

				fixed4 Y = tex2Dproj(_YTex, i.pos);
				fixed4 UV = tex2Dproj(_UVTex, i.pos);
				float y = Y.a - (16.0f / 255.0f);
				clamp(y, 0.0, 1.0);
				float v = UV.r * (15.0f * 16.0f / 255.0f) + UV.g * (16.0f / 255.0f);
				float u = UV.b * (15.0f * 16.0f / 255.0f) + UV.a * (16.0f / 255.0f);

				float3 rgb = mul(yuv2rgb, float3(y, u - 0.5f, v - 0.5f));
				fixed4 col = fixed4(rgb.r, rgb.g, rgb.b, 1.0f);

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;

                //return tex2Dproj(_MainTex, i.pos);
            }
            ENDCG
        }
    }
}