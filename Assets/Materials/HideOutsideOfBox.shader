// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/HideOutsideOfBox"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _A("Box Corner A", Vector) = (0, 0, 0)
        _B("Box Corner B", Vector) = (1, 1, 1)
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

            half4 _Color;
            float4 _A;
			float4 _B;
			sampler2D _MainTex;
			float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : POSITION;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD0;
			};

			bool isInsideBox(float3 vertex) {
				return vertex.x > _A.x && vertex.y > _A.y && vertex.z > _A.z
					  //&& vertex.x < _B.x && vertex.y < _B.y && vertex.z < _B.z
					;
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = UnityObjectToViewPos(v.vertex);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				half4 texColor = tex2D(_MainTex, i.uv);
//				if (i.worldPos.x >= 0 && i.worldPos.x <= 1)
				if (isInsideBox(i.worldPos))
					return _Color * texColor;

				return half4(0, 0, 0, 0) * texColor;
            }

            ENDCG
        }
    }
}
