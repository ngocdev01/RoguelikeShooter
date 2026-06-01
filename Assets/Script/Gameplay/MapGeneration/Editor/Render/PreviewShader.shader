Shader "MapGeneration/PreviewShader"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [Opacity] _Opacity("Opacity", Float) = 0
    }

    SubShader
    {
       
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }  
        Blend SrcAlpha OneMinusSrcAlpha  
        ZWrite Off  
        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_instancing 
            #include "UnityCG.cginc"

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            UNITY_INSTANCING_BUFFER_START(Props)  
                UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor) 
            UNITY_INSTANCING_BUFFER_END(Props)

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);  
                UNITY_INITIALIZE_OUTPUT(Varyings, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT)
                OUT.positionHCS = UnityObjectToClipPos(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 col = UNITY_ACCESS_INSTANCED_PROP(Props, _BaseColor);
                return col;
            }
            ENDHLSL
        }
    }
}
