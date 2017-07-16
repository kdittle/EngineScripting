// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutlineShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}

		_OutlineColor("Color", Color) = (0, 0, 0, 0)
		_OutlineWidth("Outline Width", Range(0.0, 1.0)) = .1
	}
	SubShader 
	{
		Tags 
		{ 
			"RenderType"="Opaque" 
		}
		
		Pass
		{
			//Pass the drawing outline
			Cull Front

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#include "UnityCG.cginc"
			
			#pragma vertex vert
			#pragma fragment frag

			uniform float _OutlineWidth;
			uniform float4 _OutlineColor;
			uniform float4 _MainTex_ST;
			uniform sampler2D _MainTex;

			struct v2f
			{
				float4 pos : POSITION;
				float4 color : COLOR;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(norm.xy);
				o.pos.xy += offset * _OutlineWidth;
				o.color = _OutlineColor;
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				return i.color;
			}

			ENDCG
		}

		Pass
		{
			//Pass drawing object
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag

			uniform float4 _MainTex_ST;
			uniform sampler2D _MainTex;

			struct v2f
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORDO;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				return tex2D(_MainTex, _MainTex_ST.xy * i.uv.xy + _MainTex_ST.zw);
			}

			ENDCG
		}
	}
	FallBack Off
}
