Shader "SciFi Weapons/Plasma Shielding" {
	Properties {
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _BumpMap ("Normal Map", 2D) = "normal" {}
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic ("Metallic", Range(0, 1)) = 0
		[NoScaleOffset] _MetallicGlossMap ("Metallic Gloss Map", 2D) = "white" {}
		[NoScaleOffset] _EmissionMap ("Emission", 2D) = "white" {}
		_EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		[Header(FX Settings)] _FXMap ("FX Texture", 2D) = "black" {}
		_FXNormal ("FX Normal", 2D) = "normal" {}
		_FXNormalStrength ("FX Normal Strength", Float) = 0
		_FXEmissionColor ("FX Emission Color", Vector) = (1,1,1,1)
		_FlowParams ("Speed, Emission 1, Emission 2, Alpha", Vector) = (1,1,1,1)
		_FXParams ("Fresnel Mult, Fresnel Pow, Distort, Alpha", Vector) = (1,1,1,1)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}