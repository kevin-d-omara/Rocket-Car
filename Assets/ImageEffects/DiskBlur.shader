Shader "Hidden/DiskBlur"
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
				#pragma fragment diskBlur
				#include "DOF.cginc"
			ENDCG
		}

		Pass
		{

			CGPROGRAM		
				#pragma vertex vert   
				#pragma fragment postBlur 
				#define DEPTHCHECK
				#include "DOF.cginc"
			ENDCG
		}



	}
}
