Shader "Custom/HologramShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		_HologramColor("Hologram Color", Color) = (0, 0, 0, 0)
		_HologramIntensity("Hologram Intensity", Range(-1.0, 1.0)) = 1.0
	}
	SubShader 
	{
		Tags 
		{ 
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha:fade nolighting

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalMap;

		struct Input 
		{
			float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
		};

		fixed4 _Color;

		float _HologramIntensity;
		fixed4 _HologramColor;

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			float border = 1 - (abs(dot(IN.viewDir, IN.worldNormal)));
			float alpha = (border * (1 - _HologramIntensity) + _HologramIntensity);
			o.Alpha = c.a * alpha;
		}
		ENDCG
	}
	//FallBack "Diffuse"
}
