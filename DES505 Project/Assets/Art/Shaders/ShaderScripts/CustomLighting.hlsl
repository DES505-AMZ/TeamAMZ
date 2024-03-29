﻿void MainLight_half(float3 WorldPos, out half3 Direction, out half3 Color, out half DistanceAtten, out half ShadowAtten)
{
	#if SHADERGRAPH_PREVIEW
		Direction = half3(0.5, 0.5, 0);
		Color = 1;
		DistanceAtten = 1;
		ShadowAtten = 1;
	#else
		#if SHADOWS_SCREEN
			half4 clipPos = TransformWorldToHClip(WorldPos)l;
			half4 shadwoCoord = ComputeScreenPos(clipPos);
		#else
			half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
		#endif
			Light mainLight = GetMainLight(shadowCoord);
			Direction = mainLight.direction;
			Color = mainLight.color;
			DistanceAtten = mainLight.distanceAttenuation;

		#if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF)
			ShadowAtten = 1.0h;
		#endif

		#if SHADOWS_SCREEN
			ShadowAtten = SampleScreenSpaceSHadowmap(shadowCoord);
		#else
			ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
			half shadowStrength = GetMainLightShadowStrength();
			ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture,
				sampler_MainLightShadowmapTexture),
				shadowSamplingData, shadowStrength, false);
		#endif
	#endif
}

void DirectSpecular_half(half3 Specular, half Smoothness, half3 Direction, half3 Color, half3 WorldNormal, half3 WorldView, out half3 Out)
{
	#if SHADERGRAPH_PREVIEW
		Out = 0;
	#else
		Smoothness = exp2(10 * Smoothness + 1);
		WorldNormal = normalize(WorldNormal);
		WorldView = SafeNormalize(WorldView);
		Out = LightingSpecular(Color, Direction, WorldNormal, WorldView, half4(Specular, 0), Smoothness);
	#endif
}