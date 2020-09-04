// Adapted to CG from a shader by David Lipps aka DaveTheDev @ EXPWorlds
// v2.0.0 for Godot 3.2

Shader "Custom/Cel"
{
    Properties
    {
		_BaseColor ("Base Color", Color) = (1, 1, 1, 1)
		_ShadeColor ("Shade Color", Color) = (1, 1, 1, 1)
		_RimTint ("Rim Tint", Color) = (0.75, 0.75, 0.75, 0.75)

		_ShadeThreshold ("Shade Threshold", Range (-1.0, 1.0)) = 0.0
		_ShadeSoftness ("Shade Softness", Range (0.0, 1.0)) = 0.01

		[Toggle]
		_UseRim ("Use Rim", Float) = 0
		_RimThreshold ("Rim Threshold", Range (0.0, 1.0)) = 0
		_RimSoftness ("Rim Softness", Range (0.0, 1.0)) = 0.05
		_RimSpread ("Rim Spread", Range (0.0, 1.0)) = 0.5

		_ShadowThreshold ("Shadow Threshold", Range (0.0, 1.0)) = 0.7
		_ShadowSoftness ("Shadow Softness", Range (0.0, 1.0)) = 0.1

        _BaseTex ("Base Texture", 2D) = "white" {}
		_ShadeTex ("Shade Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;

				SHADOW_COORDS(2)
            };

            sampler2D _BaseTex;
            float4 _BaseTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _BaseTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);

				TRANSFER_SHADOW(o)

                return o;
            }

			fixed4 _BaseColor;
			fixed4 _ShadeColor;
			fixed4 _RimTint;

			float _ShadeThreshold;
			float _ShadeSoftness;

			float _UseRim;
			float _RimThreshold;
			float _RimSoftness;
			float _RimSpread;

			float _ShadowThreshold;
			float _ShadowSoftness;

			sampler2D _ShadeTex;

            fixed4 frag (v2f i) : SV_Target
            {
				float3 normal = normalize(i.worldNormal);

                float NdotL = dot(normal, _WorldSpaceLightPos0);
				float is_lit = step(_ShadeThreshold, NdotL);
				fixed4 base = tex2D(_BaseTex, i.uv) * _BaseColor;
				fixed4 shade = tex2D(_ShadeTex, i.uv) * _ShadeColor;
				fixed4 diffuse = base;

				float shade_value = smoothstep(_ShadeThreshold - _ShadeSoftness, _ShadeThreshold + _ShadeSoftness, NdotL);
				diffuse = lerp(shade, base, shade_value);

				float shadow_value = smoothstep(_ShadowThreshold - _ShadowSoftness, _ShadowThreshold + _ShadowSoftness, SHADOW_ATTENUATION(i));
				shade_value = min(shade_value, shadow_value);
				diffuse = lerp(shade, base, shade_value);
				is_lit = step(_ShadowThreshold, shade_value);

				return diffuse;
            }
            ENDCG
        }
    }
}
