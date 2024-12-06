Shader "Lit/ScoreDisolveShader"
{
    Properties
    {
        _BaseMap ("Texture", 2D) = "white" {}
        _BaseColor ("Color", Color) = (1, 1, 1, 1)
        _NoiseMap ("Noise Texture", 2D) = "white" {}
        _DissolveThreshold ("Dissolve Threshold", Range(0, 1)) = 0
        _DissolveEdgeWidth ("Edge Width", Range(0, 1)) = 0.1
        _DissolveEdgeColor ("Edge Color", Color) = (1, 0, 0, 1)
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _Metallic ("Metallic", Range(0, 1)) = 0
        _AmbientIntensity ("Ambient Intensity", Range(0, 1)) = 0.2
        _SpecularIntensity ("Specular Intensity", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float2 lightmapUV : TEXCOORD1;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 lightmapUV : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
                float3 normalWS : TEXCOORD3;
                float4 shadowCoord : TEXCOORD4;
                float fogCoord : TEXCOORD5;
            };
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_NoiseMap);
            SAMPLER(sampler_NoiseMap);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float _DissolveThreshold;
                float _DissolveEdgeWidth;
                float4 _DissolveEdgeColor;
                float _Smoothness;
                float _Metallic;
                float _AmbientIntensity;
                float _SpecularIntensity;
            CBUFFER_END
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);
                
                OUT.positionCS = vertexInput.positionCS;
                OUT.positionWS = vertexInput.positionWS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.normalWS = normalInput.normalWS;
                OUT.shadowCoord = GetShadowCoord(vertexInput);
                OUT.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                
                #ifdef LIGHTMAP_ON
                    OUT.lightmapUV = IN.lightmapUV.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                #else
                    OUT.lightmapUV = float2(0, 0);
                #endif
                
                return OUT;
            }
            
            half4 frag(Varyings IN) : SV_Target
            {
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                half noise = SAMPLE_TEXTURE2D(_NoiseMap, sampler_NoiseMap, IN.uv).r;
                
                half dissolveAlpha = noise - _DissolveThreshold;
                clip(dissolveAlpha);
                
                half edgeLerp = saturate(dissolveAlpha / _DissolveEdgeWidth);
                half4 surfaceColor = lerp(_DissolveEdgeColor, baseColor, edgeLerp);
                
                float3 normalWS = normalize(IN.normalWS);
                float3 positionWS = IN.positionWS;
                float3 viewDirWS = normalize(GetWorldSpaceViewDir(positionWS));
                
                // 环境光照
                float3 ambient = unity_AmbientSky.rgb * _AmbientIntensity;
                
                // 主光源和阴影
                float4 shadowCoord = TransformWorldToShadowCoord(positionWS);
                Light mainLight = GetMainLight(shadowCoord);
                float shadow = mainLight.shadowAttenuation;
                
                float3 halfDir = normalize(mainLight.direction + viewDirWS);
                float NdotH = saturate(dot(normalWS, halfDir));
                float specular = pow(NdotH, _Smoothness * 100.0) * _SpecularIntensity * shadow;
                
                // 光照计算
                InputData inputData = (InputData)0;
                inputData.positionWS = positionWS;
                inputData.normalWS = normalWS;
                inputData.viewDirectionWS = viewDirWS;
                inputData.shadowCoord = shadowCoord;
                inputData.bakedGI = ambient;
                
                #ifdef LIGHTMAP_ON
                    inputData.bakedGI = SampleLightmap(IN.lightmapUV, normalWS);
                #endif
                
                // 表面属性
                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = surfaceColor.rgb;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness;
                surfaceData.normalTS = float3(0, 0, 1);
                surfaceData.occlusion = 1;
                surfaceData.emission = specular * mainLight.color;
                surfaceData.alpha = surfaceColor.a;
                
                half4 finalColor = UniversalFragmentPBR(inputData, surfaceData);
                finalColor.rgb = MixFog(finalColor.rgb, IN.fogCoord);
                
                return finalColor;
            }
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float3 _LightDirection;
            float4 _ShadowBias;

            TEXTURE2D(_NoiseMap);
            SAMPLER(sampler_NoiseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float _DissolveThreshold;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
                
                output.positionCS = positionCS;
                output.uv = input.texcoord;
                return output;
            }

            half4 frag(Varyings input) : SV_TARGET
            {
                half noise = SAMPLE_TEXTURE2D(_NoiseMap, sampler_NoiseMap, input.uv).r;
                clip(noise - _DissolveThreshold);
                return 0;
            }
            ENDHLSL
        }
    }
}