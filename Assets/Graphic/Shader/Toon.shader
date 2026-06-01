Shader "Unlit/Toon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutLineWidth ("Outline Width", Range(0, 1)) = 0.05   // increased range
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            Name "Main"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // === ADD THESE ===
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD2;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD0;
                float4 shadowCoord : TEXCOORD3;
            };

            TEXTURE2D(_MainTex); 
            SAMPLER(sampler_MainTex);
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
            CBUFFER_END

            Varyings vert (Attributes v)
            {
                Varyings o;
                VertexPositionInputs positions = GetVertexPositionInputs(v.vertex.xyz);
                
                o.positionCS = TransformObjectToHClip(v.vertex.xyz);
                o.positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.normalWS = TransformObjectToWorldNormal(v.normal);
                o.shadowCoord = GetShadowCoord(positions);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
               
                return o;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS);

                half3 ambient = SampleSH(IN.normalWS);

                // Main Light + Shadows (this already includes shadow attenuation)
                Light mainLight = GetMainLight(shadowCoord);
                half NdotL = saturate(dot(IN.normalWS, mainLight.direction));
                half shadow = mainLight.shadowAttenuation;           // 1 = lit, 0 = shadowed

                half3 diffuse = tex.rgb * mainLight.color * NdotL * shadow;

                half3 finalColor = ambient * tex.rgb + diffuse;

                return half4(finalColor, 1);
            }
            ENDHLSL
        }

        // ====================== OUTLINE PASS ======================
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Cull Front
            ZWrite On
            ZTest LEqual


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float4 bakedNormal : COLOR;     // smooth normal from vertex color
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutLineWidth;
            CBUFFER_END

            Varyings vert (Attributes v)
            {
                Varyings o;

                // === IMPORTANT: Decode baked normal ===
                float3 smoothNormalOS = v.bakedNormal.rgb * 2.0 - 1.0;   // [0,1] → [-1,1]

                // Transform to world space
                float3 normalWS = TransformObjectToWorldNormal(smoothNormalOS);

                // Transform to clip space direction
                float3 normalCS = normalize(TransformWorldToHClipDir(normalWS));

                // Extrude
                float4 posCS = TransformObjectToHClip(v.vertex.xyz);
         
                posCS.xyz += normalCS * _OutLineWidth * posCS.w * 0.01; 

                o.positionCS = posCS;
                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
        
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Off               
        
            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
        
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
        
            ENDHLSL
        }
    }
}