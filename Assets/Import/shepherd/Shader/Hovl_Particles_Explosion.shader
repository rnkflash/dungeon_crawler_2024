Shader "Hovl/Particles/Explosion" {
	Properties {
		_Noise ("Noise", 2D) = "white" {}
		_FinalEmission ("Final Emission", Float) = 1
		_Color ("Color", Vector) = (1,1,1,1)
		_GlowColor ("Glow Color", Vector) = (1,1,0,1)
		_Opacity ("Opacity", Range(0, 1)) = 1
		_NoisespeedXYNoisepowerZGlowpowerW ("Noise speed XY Noise power Z Glow power W", Vector) = (0.314,0.427,0.001,4)
		_MotionVector ("MotionVector", 2D) = "white" {}
		_MainTex ("MainTex", 2D) = "white" {}
		_TilingXY ("Tiling XY", Vector) = (8,8,0,0)
		_MotionAmount ("MotionAmount", Float) = 0.001
		[MaterialToggle] _Usedepth ("Use depth?", Float) = 0
		_Depthpower ("Depth power", Float) = 1
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
}