// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FadeWithDistance"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _Center("Center Point", Vector) = (0, 0, 0)
        _MaxDistance2("Max Visibility Distance Squared", float) = 1
        _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            float _MaxDistance2;
            fixed4 _Color;
            float3 _Center;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
                fixed4 color : COLOR;
            };

            float dist2(float3 center, float3 p) {
                float3 diff = center - p;
                return dot(diff, diff);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float3 pos = mul(unity_ObjectToWorld, v.vertex);
                float normDistance = 1 - (dist2(_Center, pos) / _MaxDistance2);
                o.color = fixed4(_Color.xyz, _Color.w * normDistance);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return i.color * col;
            }

            ENDCG
        }
    }
}
