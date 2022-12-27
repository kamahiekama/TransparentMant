Shader "MappingToScreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            sampler2D _MainTex;
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
                return tex2Dproj(_MainTex, i.pos);
            }
            ENDCG
        }
    }
}