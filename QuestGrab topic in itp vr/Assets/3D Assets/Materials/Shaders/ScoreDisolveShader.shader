Shader "Lit/ScoreDisolveShader"
{
    Properties
    {
        _BaseMap ("Texture", 2D) = "white" {}
        _BaseColor ("Color", Color) = (1, 1, 1, 1)
        _NoiseMap ("Noise Texture", 2D) = "white" {}
        _RoughnessMap ("Roughness Map", 2D) = "white" {}
        _DissolveThreshold ("Dissolve Threshold", Range(0, 1)) = 0
        _DissolveEdgeWidth ("Edge Width", Range(0, 1)) = 0.1
        _DissolveEdgeColor ("Edge Color", Color) = (1, 0, 0, 1)
        [HDR]_EdgeEmissionColor("Edge Emission Color", Color) = (1, 1, 1, 1)
        _EdgeEmissionPower("Edge Emission Power", Range(1, 10)) = 2
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _Metallic ("Metallic", Range(0, 1)) = 0
        _RoughnessIntensity ("Roughness Intensity", Range(0, 1)) = 1
        _AmbientIntensity ("Ambient Intensity", Range(0, 1)) = 0.2
        _SpecularIntensity ("Specular Intensity", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
        }
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // VRÖ§³Ö
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma multi_compile _ STEREO_INSTANCING_ON
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_MULTIVIEW_ON
            
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
                UNITY_VERTEX_INPUT_INSTANCE_ID
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
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_NoiseMap);
            SAMPLER(sampler_NoiseMap);
            TEXTURE2D(_RoughnessMap);
            SAMPLER(sampler_RoughnessMap);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float _DissolveThreshold;
                float _DissolveEdgeWidth;
                float4 _DissolveEdgeColor;
                float4 _EdgeEmissionColor;
                float _EdgeEmissionPower;
                float _Smoothness;
                float _Metallic;
                float _RoughnessIntensity;
                float _AmbientIntensity;
                float _SpecularIntensity;
            CBUFFER_END
            
            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.normalWS = normalInput.normalWS;
                output.shadowCoord = GetShadowCoord(vertexInput);
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                
                #ifdef LIGHTMAP_ON
                    output.lightmapUV = input.lightmapUV.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                #else
                    output.lightmapUV = float2(0, 0);
                #endif
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                half noise = SAMPLE_TEXTURE2D(_NoiseMap, sampler_NoiseMap, input.uv).r;
                half roughness = SAMPLE_TEXTURE2D(_RoughnessMap, sampler_RoughnessMap, input.uv).r * _RoughnessIntensity;
                
                half dissolveAlpha = noise - _DissolveThreshold;
                clip(dissolveAlpha);
                
                half edgeLerp = saturate(dissolveAlpha / _DissolveEdgeWidth);
                half4 surfaceColor = lerp(_DissolveEdgeColor, baseColor, edgeLerp);
                
                half emissionMask = 1 - edgeLerp;
                half3 emission = _EdgeEmissionColor.rgb * pow(emissionMask, _EdgeEmissionPower);
                
                float3 normalWS = normalize(input.normalWS);
                float3 positionWS = input.positionWS;
                float3 viewDirWS = normalize(GetWorldSpaceViewDir(positionWS));
                
                float3 ambient = unity_AmbientSky.rgb * _AmbientIntensity;
                
                float4 shadowCoord = TransformWorldToShadowCoord(positionWS);
                Light mainLight = GetMainLight(shadowCoord);
                float shadow = mainLight.shadowAttenuation;
                
                float3 halfDir = normalize(mainLight.direction + viewDirWS);
                float NdotH = saturate(dot(normalWS, halfDir));
                
                float roughnessAdjustedSpecular = pow(NdotH, (1 - roughness) * 100.0) * _SpecularIntensity * shadow;
                
                InputData inputData = (InputData)0;
                inputData.positionWS = positionWS;
                inputData.normalWS = normalWS;
                inputData.viewDirectionWS = viewDirWS;
                inputData.shadowCoord = shadowCoord;
                inputData.bakedGI = ambient;
                
                #ifdef LIGHTMAP_ON
                    inputData.bakedGI = SampleLightmap(input.lightmapUV, normalWS);
                #endif
                
                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = surfaceColor.rgb;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness * (1 - roughness);
                surfaceData.normalTS = float3(0, 0, 1);
                surfaceData.occlusion = 1;
                surfaceData.emission = roughnessAdjustedSpecular * mainLight.color + emission;
                surfaceData.alpha = surfaceColor.a;
                
                half4 finalColor = UniversalFragmentPBR(inputData, surfaceData);
                finalColor.rgb = MixFog(finalColor.rgb, input.fogCoord);
                
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
            
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma multi_compile _ STEREO_INSTANCING_ON
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_MULTIVIEW_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float3 _LightDirection;

            TEXTURE2D(_NoiseMap);
            SAMPLER(sampler_NoiseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float _DissolveThreshold;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
                
                output.positionCS = positionCS;
                output.uv = input.texcoord;
                return output;
            }

            half4 frag(Varyings input) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                half noise = SAMPLE_TEXTURE2D(_NoiseMap, sampler_NoiseMap, input.uv).r;
                clip(noise - _DissolveThreshold);
                return 0;
            }
            ENDHLSL
        }
    }
}