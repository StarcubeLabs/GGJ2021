﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/**
 * @author dizgid Kenji Inokuchi / http://www.dizgid.com/
 *
 * MatCap Shader
 * Ported from spherical-environment-mapping
 * http://www.clicktorelease.com/blog/creating-spherical-environment-mapping-shader
 *
 */

Shader "GGJ2021/Inokuchi/Advanced MatCap"
{
	Properties
	{
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MatCap ("MatCap (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
	}
	
	Subshader
	{
		Tags { "RenderType"="Opaque" "Queue" = "Transparent" "DisableBatching"="True"}
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			//Tags { "LightMode" = "Always" }
			
			// http://developer.download.nvidia.com/GPU_Programming_Guide/GPU_Programming_Guide_Japanese.pdf
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members uv_BumpMap)
#pragma exclude_renderers d3d11
				#pragma vertex vert
				#pragma fragment frag
				#pragma alpha
				#pragma fragmentoption ARB_precision_hint_fastest
				//#pragma fragmentoption ARB_precision_hint_nicest
				#include "UnityCG.cginc"
                #include "Lighting.cginc"
                #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
				#pragma multi_compile_fog
                // shadow helper functions and macros
                #include "AutoLight.cginc"
				
				struct v2f
				{
					float4 pos	: SV_POSITION;
					float2 cap	: TEXCOORD0;
					float2 uv_BumpMap;
                    SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                    fixed3 diff : COLOR0;
                    fixed3 ambient : COLOR1;
				};
				
				v2f vert (appdata_base v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos (v.vertex);
					
//					half2 capCoord;
//					// Inverse transpose of model*view matrix
//					capCoord.x = dot(UNITY_MATRIX_IT_MV[0].xyz, v.normal);
//					capCoord.y = dot(UNITY_MATRIX_IT_MV[1].xyz, v.normal);
//					o.cap = capCoord * 0.5 + 0.5;

					float4 p = float4( v.vertex );

					float3 e = normalize( mul( UNITY_MATRIX_MV , p) );
					float3 n = normalize( mul( UNITY_MATRIX_MV , float4(v.normal, 0.)) );

					float3 r = reflect( e, n );
						float m = 2. * sqrt( 
						pow( r.x, 2. ) + 
						pow( r.y, 2. ) + 
						pow( r.z + 1., 2. ) 
					);
					half2 capCoord;
					capCoord = r.xy / m + 0.5;
					o.cap = capCoord;
                    half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                    half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                    o.diff = nl * _LightColor0.rgb;
                    o.ambient = ShadeSH9(half4(worldNormal,1));
                    // compute shadows data
                    TRANSFER_SHADOW(o)
					return o;
				}
				
				uniform float4 _Color;
				uniform sampler2D _MatCap;
				
				float4 frag (v2f i) : COLOR
				{
					float3 base = tex2D(_MatCap, i.cap).rgb * _Color;
					return float4(base, 1.);
				}
			ENDCG
		}
	}
}