Shader "Hidden/GridDrawer"
{
	Properties
	{
		_Grid ("X Y LEFT Z W RIGHT", Vector) = (0,0,0,0)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 _Grid;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _Grid; ;// tex2D(_MainTex, i.uv);
				
				return col;
			}
			ENDCG
		}
	}
}
