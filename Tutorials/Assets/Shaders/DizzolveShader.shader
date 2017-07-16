// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DizzolveShader" 
{
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		_DissolveMap("Dissolve Map", 2D) = "white" {}
		_DissolveVal("Dissolve Value", Range(0.0, 1.0)) = 1.0
		_LineWidth("Line Width", Range(0.0, 0.2)) = 0.1
		_LineColor("Line Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader 
	{
		Tags { "Queue" = "Transparent" "RenderType"="Transparent"}
		//LOD 200

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			Lighting Off
			ZWrite Off
			Fog {Mode Off}

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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _DissolveMap;

			float _DissolveVal;
			float _LineWidth;
			float4 _LineColor;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float dissolveCut = tex2D(_DissolveMap, i.uv).r;
				fixed4 col = tex2D(_MainTex, i.uv);

				if(dissolveCut < _DissolveVal)
					discard;

				if(dissolveCut < col.a && dissolveCut < _DissolveVal + _LineWidth)
				{
					col = lerp(_LineColor, 0.0, (dissolveCut - _DissolveVal) / _LineColor);
				}

				return col;
			}
			ENDCG
		}
	}
	//FallBack "Diffuse"
}
