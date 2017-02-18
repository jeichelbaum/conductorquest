Shader "INW/GreyScaleColoring"
{
	Properties
	{
		_Emission("Emission", Range(0,4)) = 1.0
		_MainTex("Texture", 2D) = "white" {}
	
		_Color1("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color2("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color3("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color4("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color5("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color6("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color7("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color8("Color", Color) = (1.0, 1.0, 1.0, 1.0)

	}
		SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100
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
		float4 vertex : SV_POSITION;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float _Emission;

	float4 _Color1;
	float4 _Color2;
	float4 _Color3;
	float4 _Color4;
	float4 _Color5;
	float4 _Color6;
	float4 _Color7;
	float4 _Color8;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{

		fixed4 col = tex2D(_MainTex, i.uv);

	if (col.r < 0.024) {
		col = _Color1 * float4(_Emission, _Emission, _Emission, col.a);
	}
	else if (col.r < 0.112) {
		col = _Color2 * float4(_Emission, _Emission, _Emission, col.a);
	}
	else if (col.r < 0.225) {
		col = _Color3 * float4(_Emission, _Emission, _Emission, col.a);
	}
	else if (col.r < 0.355) {
		col = _Color4 * float4(_Emission, _Emission, _Emission, col.a);
	}
	else if (col.r < 0.456) {
		col = _Color5 * float4(_Emission, _Emission, _Emission, col.a);
	}
	else if (col.r <= 0.509) {
		col = _Color6 * float4(_Emission, _Emission, _Emission, col.a);
	}
	else if (col.r <= 0.602) {
		col = _Color7 * float4(_Emission, _Emission, _Emission, col.a);
	}
	else if (col.r <= 0.81) {
		col = _Color8 * float4(_Emission, _Emission, _Emission, col.a);
	}

		
		return col;
		}
			ENDCG
		}
	}
}
