Shader "Custom/Cage"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_BorderColor("Border Color", Color) = (0, 0, 0, 1)
		_Border("Border Width", float) = 0.05
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		LOD 100
		Cull Front
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

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
				float4 vertex : POSITION;
			};

			half4 _Color;
			half4 _BorderColor;
			float _Border;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			inline bool inBorder(float2 uv) {
				return uv.x < _Border || uv.x >(1 - _Border) || uv.y < _Border || uv.y > (1 - _Border);
			}

			fixed4 frag(v2f i, UNITY_VPOS_TYPE screenPos : POSITION) : SV_Target
			{
				if (inBorder(i.uv))
					return fixed4(_Color.rgb * (1 - _BorderColor.a) + _BorderColor.rgb * _BorderColor.a, 1);

				return _Color;
			}
			ENDCG
		}
	}
}
