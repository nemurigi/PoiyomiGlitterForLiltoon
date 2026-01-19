// MIT License
//
// Copyright (c) 2023 Poiyomi Inc.  (original author)
// Copyright (c) 2025 nemurigi (ported to liltoon custom shader)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#include "UnityCG.cginc"
#include "Packages/jp.lilxyzw.liltoon/Shader/Includes/lil_common.hlsl"

#define PI float(3.14159265359)
#define EPSILON 0.000001f

// #define POI2D_SAMPLER_PAN(tex, samplerState, uv, pan) (UNITY_SAMPLE_TEX2D_SAMPLER(tex, texSampler, uv + _Time.x * pan))
#define POI2D_SAMPLER_PAN(tex, samplerState, uv, pan) tex.Sample(samp,uv*tex##_ST.xy+tex##_ST.zw + _Time.x * pan)

float random(float2 p)
{
	return frac(sin(dot(p, float2(12.9898, 78.2383))) * 43758.5453123);
}

float2 random2(float2 p)
{
	return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
}
			
float3 random3(float3 p)
{
	return frac(sin(float3(dot(p, float3(127.1, 311.7, 248.6)), dot(p, float3(269.5, 183.3, 423.3)), dot(p, float3(248.3, 315.9, 184.2)))) * 43758.5453);
}

float3 randomFloat3(float2 Seed, float maximum)
{
    return(.5 + float3(
        frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
        frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
        frac(sin(dot(float2(Seed), float2(12.9898, 78.233))) * 43758.5453)
    ) * .5) * (maximum);
}

float3 randomFloat3Range(float2 Seed, float Range)
{
    return(float3(
        frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
        frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
        frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
    ) * 2 - 1) * Range;
}

float3 randomFloat3WiggleRange(float2 Seed, float Range)
{
    float3 rando = (float3(
        frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
        frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
        frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
    ) * 2 - 1);
    float speed = 1 + _PoiGlitterSpeed;
    return float3(sin((_Time.x + rando.x * PI) * speed), sin((_Time.x + rando.y * PI) * speed), sin((_Time.x + rando.z * PI) * speed)) * Range;
}

float3 randomFloat3WiggleRange(float2 Seed, float Range, float wiggleSpeed)
{
	float3 rando = (float3(
	frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
	frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
	frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
	) * 2 - 1);
	float speed = 1 + wiggleSpeed;
	return float3(sin((_Time.x + rando.x * PI) * speed), sin((_Time.x + rando.y * PI) * speed), sin((_Time.x + rando.z * PI) * speed)) * Range;
}

void Unity_RandomRange_float(float2 Seed, float Min, float Max, out float Out)
{
    float randomno = frac(sin(dot(Seed, float2(12.9898, 78.233))) * 43758.5453);
    Out = lerp(Min, Max, randomno);
}

float3 HUEtoRGB(in float H)
{
	float R = abs(H * 6 - 3) - 1;
	float G = 2 - abs(H * 6 - 2);
	float B = 2 - abs(H * 6 - 4);
	return saturate(float3(R, G, B));
}

float3 HSVtoRGB(in float3 HSV)
{
	float3 RGB = HUEtoRGB(HSV.x);
	return ((RGB - 1) * HSV.y + 1) * HSV.z;
}
				
float3 RGBtoHCV(in float3 RGB)
{
	// Based on work by Sam Hocevar and Emil Persson
	float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
	float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
	float C = Q.x - min(Q.w, Q.y);
	float H = abs((Q.w - Q.y) / (6 * C + EPSILON) + Q.z);
	return float3(H, C, Q.x);
}
				
float3 RGBtoHSV(in float3 RGB)
{
	float3 HCV = RGBtoHCV(RGB);
	float S = HCV.y / (HCV.z + EPSILON);
	return float3(HCV.x, S, HCV.z);
}
				
float3 hueShift(float3 color, float hueOffset)
{
	color = RGBtoHSV(color);
	color.x = frac(hueOffset +color.x);
	return HSVtoRGB(color);
}

float3 RandomColorFromPoint(float2 rando)
{
    fixed hue = random2(rando.x + rando.y).x;
    fixed saturation = lerp(_PoiGlitterSaturationMin, _PoiGlitterSaturationMax, rando.x);
    fixed value = lerp(_PoiGlitterBrightnessMin, _PoiGlitterBrightnessMax, rando.y);
    float3 hsv = float3(hue, saturation, value);
    return HSVtoRGB(hsv);
}

float2 poiUV(float2 uv, float4 tex_st)
{
	return uv * tex_st.xy + tex_st.zw;
}

float remapClamped(float minOld, float maxOld, float x, float minNew = 0, float maxNew = 1)
{
    return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
}
			
float2 remapClamped(float2 minOld, float2 maxOld, float2 x, float2 minNew, float2 maxNew)
{
    return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
}
			
float3 remapClamped(float3 minOld, float3 maxOld, float3 x, float3 minNew, float3 maxNew)
{
    return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
}
			
float4 remapClamped(float4 minOld, float4 maxOld, float4 x, float4 minNew, float4 maxNew)
{
    return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
}

float4x4 poiRotationMatrixFromAngles(float3 angles)
{
	float angleX = radians(angles.x);
	float c = cos(angleX);
	float s = sin(angleX);
	float4x4 rotateXMatrix = float4x4(1, 0, 0, 0,
	0, c, -s, 0,
	0, s, c, 0,
	0, 0, 0, 1);
			
	float angleY = radians(angles.y);
	c = cos(angleY);
	s = sin(angleY);
	float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
	0, 1, 0, 0,
	- s, 0, c, 0,
	0, 0, 0, 1);
			
	float angleZ = radians(angles.z);
	c = cos(angleZ);
	s = sin(angleZ);
	float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
	s, c, 0, 0,
	0, 0, 1, 0,
	0, 0, 0, 1);
			
	return mul(mul(rotateXMatrix, rotateYMatrix), rotateZMatrix);
}

float2 getGlitterUV(lilFragData fd, float index)
{
    switch(index)
    {
    	case 0: return fd.uv0;
        case 1: return fd.uv1;
    	case 2: return fd.uv2;
        case 3: return fd.uv3;
    	case 4: return fd.uvPanorama;
    	case 5: return fd.positionWS.xz;
    	default: return fd.uv0;
    }
}

// PoiCam.viewDir          -> lilFragData.V
// PoiCam.distanceToVert   -> length(lilHeadDirection(lilFragData.positionWS))
// PoiMesh.normals[1]      -> lilFragData.origN
// PoiLight.direction      -> lilFragData.L
// PoiLight.rampedLightMap -> lilFragData.shadowmix
// PoiLight.nDotLSaturate  -> saturate(lilFragData.ln)
// PoiLight.attenuation    -> lilFragData.attenuation
// PoiLight.directColor    -> lilFragData.lightColor

// void applyGlitter(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiLight poiLight, in PoiMods poiMods)
void applyGlitter(inout lilFragData fd LIL_SAMP_IN_FUNC(samp))
{
	// Scale
	float2 st = frac(getGlitterUV(fd, _PoiGlitterUV) + _PoiGlitterUVPanning.xy * _Time.x) * _PoiGlitterFrequency;

	// Tile the space
	float2 i_st = floor(st);
	float2 f_st = frac(st);
	
	float m_dist = 10.;  // minimun distance
	float2 m_point = 0;  // minimum point
	float2 randoPoint = 0;
	float2 dank;
	for (int j = -1; j <= 1; j++)
	{
		for (int i = -1; i <= 1; i++)
		{
			float2 neighbor = float2(i, j);
			float2 pos = random2(i_st + neighbor);
			float2 rando = pos;
			pos = 0.5 + 0.5 * sin(_PoiGlitterJitter * 6.2831 * pos);
			float2 diff = neighbor + pos - f_st;
			float dist = length(diff);
			
			if (dist < m_dist)
			{
				dank = diff;
				m_dist = dist;
				m_point = pos;
				randoPoint = rando;
			}
		}
	}
	
	float randomFromPoint = random(randoPoint);
	
	float size = _PoiGlitterSize;
	UNITY_BRANCH
	if (_PoiGlitterRandomSize)
	{
		size = remapClamped(0, 1, randomFromPoint, _PoiGlitterMinMaxSize.x, _PoiGlitterMinMaxSize.y);
	}
	
	// Assign a color using the closest point position
	//color += dot(m_point, float2(.3, .6));
	
	// Add distance field to closest point center
	// color.g = m_dist;
	
	// Show isolines
	//color -= abs(sin(40.0 * m_dist)) * 0.07;
	
	// Draw cell center
	half glitterAlpha = 1;
	switch(_PoiGlitterShape)
	{
		case 0: //circle
		glitterAlpha = 1 - saturate((m_dist - size) / clamp(fwidth(m_dist), 0.0001, 1.0));
		break;
		case 1: //sqaure
		float jaggyFix = pow(length(lilHeadDirection(fd.positionWS)), 2) * _PoiGlitterJaggyFix;
		
		UNITY_BRANCH
		if (_PoiGlitterRandomRotation == 1 || _PoiGlitterTextureRotation != 0)
		{
			float2 center = float2(0, 0);
			float randomBoy = 0;
			UNITY_BRANCH
			if (_PoiGlitterRandomRotation)
			{
				randomBoy = random(randoPoint);
			}
			float theta = radians((randomBoy + _Time.x * _PoiGlitterTextureRotation) * 360);
			float cs = cos(theta);
			float sn = sin(theta);
			dank = float2((dank.x - center.x) * cs - (dank.y - center.y) * sn + center.x, (dank.x - center.x) * sn + (dank.y - center.y) * cs + center.y);
			glitterAlpha = (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.x))) * (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.y)));
		}
		else
		{
			glitterAlpha = (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.x))) * (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.y)));
		}
		break;
	}
	
	float3 finalGlitter = 0;
	
	// half3 glitterColor = poiThemeColor(poiMods, _PoiGlitterColor, _PoiGlitterColorThemeIndex);
	float3 glitterColor = _PoiGlitterColor;
	
	float3 norm = fd.origN;
	float3 randomRotation = 0;
	switch(_PoiGlitterMode)
	{
		case 0:
		UNITY_BRANCH
		if (_PoiGlitterSpeed > 0)
		{
			randomRotation = randomFloat3WiggleRange(randoPoint, _PoiGlitterAngleRange, _PoiGlitterSpeed);
		}
		else
		{
			randomRotation = randomFloat3Range(randoPoint, _PoiGlitterAngleRange);
		}
		
		float3 glitterReflectionDirection = normalize(mul(poiRotationMatrixFromAngles(randomRotation), norm));
		finalGlitter = lerp(0, _PoiGlitterMinBrightness * glitterAlpha, glitterAlpha) + max(pow(saturate(dot(lerp(glitterReflectionDirection, fd.V, _PoiGlitterBias), fd.V)), _PoiGlitterContrast), 0);
		finalGlitter *= glitterAlpha;
		break;
		case 1:
		float offset = random(randoPoint);
		float brightness = sin((_Time.x + offset) * _PoiGlitterSpeed) * _PoiGlitterFrequencyLinearEmissive - (_PoiGlitterFrequencyLinearEmissive - 1);
		finalGlitter = max(_PoiGlitterMinBrightness * glitterAlpha, brightness * glitterAlpha * smoothstep(0, 1, 1 - m_dist * _PoiGlitterCenterSize * 10));
		break;
		case 2:
		if (_PoiGlitterSpeed > 0)
		{
			randomRotation = randomFloat3WiggleRange(randoPoint, _PoiGlitterAngleRange, _PoiGlitterSpeed);
		}
		else
		{
			randomRotation = randomFloat3Range(randoPoint, _PoiGlitterAngleRange);
		}
		
		float3 glitterLightReflectionDirection = normalize(mul(poiRotationMatrixFromAngles(randomRotation), norm));
		
		#ifdef POI_PASS_ADD
		glitterAlpha *= saaturate(fd.ln) * fd.attenuation;
		#endif
		#ifdef UNITY_PASS_FORWARDBASE
		glitterAlpha *= saturate(fd.ln);
		#endif
		
		float3 halfDir = normalize(fd.L + fd.V);
		float specAngle = max(dot(halfDir, glitterLightReflectionDirection), 0.0);
		
		finalGlitter = lerp(0, _PoiGlitterMinBrightness * glitterAlpha, glitterAlpha) + max(pow(specAngle, _PoiGlitterContrast), 0);

		glitterColor *= fd.lightColor;
		finalGlitter *= glitterAlpha;
		
		break;
	}
	
	glitterColor *= lerp(1, fd.col.rgb, _PoiGlitterUseSurfaceColor);
	#if defined(PROP_PoiGlitterCOLORMAP) || !defined(OPTIMIZER_ENABLED)
	glitterColor *= POI2D_SAMPLER_PAN(_PoiGlitterColorMap, samp, poiUV(getGlitterUV(fd, _PoiGlitterColorMapUV), _PoiGlitterColorMap_ST), _PoiGlitterColorMapPan).rgb;
	#endif
	float2 uv = remapClamped(-size, size, dank, 0, 1);
	UNITY_BRANCH
	if (_PoiGlitterRandomRotation == 1 || _PoiGlitterTextureRotation != 0 && !_PoiGlitterShape)
	{
		float2 fakeUVCenter = float2(.5, .5);
		float randomBoy = 0;
		UNITY_BRANCH
		if (_PoiGlitterRandomRotation)
		{
			randomBoy = random(randoPoint);
		}
		float theta = radians((randomBoy + _Time.x * _PoiGlitterTextureRotation) * 360);
		float cs = cos(theta);
		float sn = sin(theta);
		uv = float2((uv.x - fakeUVCenter.x) * cs - (uv.y - fakeUVCenter.y) * sn + fakeUVCenter.x, (uv.x - fakeUVCenter.x) * sn + (uv.y - fakeUVCenter.y) * cs + fakeUVCenter.y);
	}
	
	#if defined(PROP_PoiGlitterTEXTURE) || !defined(OPTIMIZER_ENABLED)
	float4 glitterTexture = POI2D_SAMPLER_PAN(_PoiGlitterTexture, samp, poiUV(uv, _PoiGlitterTexture_ST), _PoiGlitterTexturePan);
	#else
	float4 glitterTexture = 1;
	#endif
	//float4 glitterTexture = _PoiGlitterTexture.SampleGrad(sampler_MainTex, frac(uv), ddx(uv), ddy(uv));
	glitterColor *= glitterTexture.rgb;
	#if defined(PROP_PoiGlitterMASK) || !defined(OPTIMIZER_ENABLED)
	float glitterMask = POI2D_SAMPLER_PAN(_PoiGlitterMask, samp, poiUV(getGlitterUV(fd, _PoiGlitterMaskUV), _PoiGlitterMask_ST), _PoiGlitterMaskPan);
	#else
	float glitterMask = 1;
	#endif

	glitterMask *= lerp(1, fd.shadowmix, _PoiGlitterHideInShadow);
	
	#ifdef POI_BLACKLIGHT
	if (_BlackLightMaskGlitter != 4)
	{
		glitterMask *= blackLightMask[_BlackLightMaskGlitter];
	}
	#endif
	
	if (_PoiGlitterRandomColors)
	{
		glitterColor *= RandomColorFromPoint(random2(randoPoint.x + randoPoint.y));
	}
	
	UNITY_BRANCH
	if (_PoiGlitterHueShiftEnabled)
	{
		glitterColor.rgb = hueShift(glitterColor.rgb, _PoiGlitterHueShift + _Time.x * _PoiGlitterHueShiftSpeed);
	}
	
	UNITY_BRANCH
	if (_PoiGlitterBlendType == 1)
	{
		fd.col.rgb = lerp(fd.col.rgb, finalGlitter * glitterColor * _PoiGlitterBrightness, finalGlitter * glitterTexture.a * glitterMask);
		fd.col.rgb += finalGlitter * glitterColor * max(0, (_PoiGlitterBrightness - 1) * glitterTexture.a) * glitterMask;
	}
	else
	{
		fd.col.rgb += finalGlitter * glitterColor * _PoiGlitterBrightness * glitterTexture.a * glitterMask;
	}
}
