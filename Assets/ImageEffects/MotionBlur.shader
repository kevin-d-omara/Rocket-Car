Shader "Hidden/MotionBlur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _MainTex_TexelSize;
			sampler2D_half _CameraMotionVectorsTexture;

			uniform float _velocityScale;
			uniform int _SampleSize;

			static const int MAX_SAMPLES = 50;
			
			fixed4 frag (v2f i) : SV_Target
			{
				
				

				float2 veluv = float2(i.uv.x,i.uv.y);
				float2 velocity = tex2D(_CameraMotionVectorsTexture, veluv).xy;
				

				velocity *= _velocityScale; 
				

				float speed = length(velocity);

				int samples = 70;


				float3 result = tex2D(_MainTex, i.uv).xyz; 
					
				for (int j = 1; j < 70; j++) {
					 
					float2 offset = velocity * ((float)j / (float)(70 -1)-.5);
					result += tex2D(_MainTex, i.uv +offset).xyz;
				}
			
				result = result / (float)70;

				return fixed4(result ,1);
			}
			ENDCG
		}
	}
}
