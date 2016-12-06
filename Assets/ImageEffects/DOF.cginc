
#ifndef __DOF__
#define __DOF__

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


v2f vert(appdata v)
{
	v2f o;
	o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.uv;
	return o;
}





sampler2D _MainTex;
float4 _MainTex_TexelSize;
sampler2D _CameraDepthTexture;
sampler2D _OriginalFrame;
StructuredBuffer<float2> _Kernel;
float _BlurRadius;
const float MAX_BLUR_RADIUS = 10.0f;
float _Aperture;
float _Visualize;
// x nearplane, y far plane, z near blur plane, w far blur plane
float4 _ClipPlanes;
float _FocalDistance;

float getLuma(float3 color) {

	float3 luma = float3(0.2126, 0.7152, .0722);
	return dot(luma, color);

}

float2 getCoC(float2 uv) {
	return max(.5,pow(1- Linear01Depth(tex2D(_CameraDepthTexture,uv).r),2)) *_MainTex_TexelSize.xy*_Aperture;
}

 
float getCoC2(float2 uv) {
	float depth = Linear01Depth(tex2D(_CameraDepthTexture, uv).r)* (_ClipPlanes.y - _ClipPlanes.x);
	return min(6,(depth - _FocalDistance) * _Aperture / _FocalDistance);
	
}

 
float checkFar(float2 uv) {
	float depth = Linear01Depth(tex2D(_CameraDepthTexture, uv).r)* (_ClipPlanes.y);

	return  depth > _ClipPlanes.w;
}

float checkInFocus(float2 uv) {
	float depth = Linear01Depth(tex2D(_CameraDepthTexture, uv).r)* (_ClipPlanes.y );

	return depth > _ClipPlanes.z && depth < _ClipPlanes.w;
}


float getBlurScale(float2 uv) {
	float depth = clamp(Linear01Depth(tex2D(_CameraDepthTexture, uv).r),.01,1)* (_ClipPlanes.y);

	return    min(depth / _ClipPlanes.z, 1-((depth - _ClipPlanes.w) / (_ClipPlanes.y - _ClipPlanes.w)));// min(, abs((depth - _ClipPlanes.w) / (_ClipPlanes.y - _ClipPlanes.w)));
}


fixed4 diskBlur(v2f i) : SV_Target
{
#ifdef DEPTHCHECK
	if (checkInFocus(i.uv)) return float4(0.0,0.0,0.0,0.0);
#endif
	float3 result = float3(0.0,0.0,0.0);
	float3 brightest = float3(0, 0, 0);
	float bluma = 0.0;
	float w = 0.0;

	for (int j = 0; j < 65; j++) {
		
		float2 offset = _Kernel[j] *getCoC(i.uv);
		#ifdef DEPTHCHECK
			if (checkInFocus(i.uv + offset))
				continue;
		#endif
		float3 color = tex2D(_MainTex, i.uv + offset).rgb;
		result += color;
		w += getLuma(color);
		if (getLuma(color)> bluma) {
			bluma = getLuma(color);
			brightest = color;
		}
	}

	return float4(lerp(brightest, tex2D(_MainTex, i.uv),0.1), .2);// float4(((result / 64) + brightest*1.0) / 2, 1);


} 



 

fixed4 postBlur(v2f i) : SV_Target
{

	float3 result = tex2D(_MainTex,i.uv);
	float3 brightest = float3(0, 0, 0);
	float bluma = 0.0;
	
	for (int j = -1; j < 2; j++) {

		for (int k = -1; k < 2; k++) {
			float2 offset = float2(j, k)* _MainTex_TexelSize;
			result += tex2D(_MainTex, i.uv + offset);
		}
	}
	
#ifdef DEPTHCHECK
	float power = lerp(0.0, 10, _Aperture);
	float blurScale = pow(clamp(getBlurScale(i.uv), 0, 1),power);
	float4 final = lerp(float4(result / 10, 1), tex2D(_OriginalFrame, i.uv), pow(clamp(getBlurScale(i.uv), 0, 1), power));
	
		return lerp(float4(getCoC(i.uv),1,1), final, _Visualize);
#endif
	return float4((result / 9.0),1); 
	
}
 
 

 




#endif
