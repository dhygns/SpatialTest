Shader "Hidden/EdgeCutter3D"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags{ "Queue" = "transparent" "RenderType" = "Transparent" }
		// No culling or depth
		Cull Back ZWrite On
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

	float rand(float3 co)
	{
		return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
	}

	v2f vert(appdata v)
	{
		const float PI = 3.141592;
		v2f o;
		v.vertex.z = v.vertex.z + sin(v.uv.x * PI) * sin(_Time.y) * 0.2;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		return o;
	}

	sampler2D _MainTex;

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.uv);
	float alpha =
		smoothstep(0.5, 0.45, abs(i.uv.x - 0.5)) *
		smoothstep(0.5, 0.45, abs(i.uv.y - 0.5));
	alpha = 1.0f;// smoothstep(0.49, 0.5, alpha);

	// just invert the colors
	//col.rgb = 1 - col.rgb;
	col.a *= alpha;
	return col;
	}
		ENDCG
	}
	}
}
