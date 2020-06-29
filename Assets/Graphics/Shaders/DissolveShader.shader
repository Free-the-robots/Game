Shader "Custom/Dissolve" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}

        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _SliceGuide("Slice Guide (RGB)", 2D) = "white" {}
        _SliceAmount("Slice Amount", Range(0.0, 1.0)) = 0
 
        _BurnSize("Burn Size", Range(0.0, 1.0)) = 0.15
        _BurnRamp("Burn Ramp (RGB)", 2D) = "white" {}
        _BurnColor("Burn Color", Color) = (1,1,1,1)
 
        _EmissionAmount("Emission amount", float) = 2.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        //Cull Off
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
 
        sampler2D _MainTex;
        sampler2D _SliceGuide;
        sampler2D _BumpMap;

        half _Glossiness;
        half _Metallic;

        sampler2D _BurnRamp;
        fixed4 _BurnColor;
        float _BurnSize;

        #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
            UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
            UNITY_DEFINE_INSTANCED_PROP(fixed, _SliceAmount)
        UNITY_INSTANCING_BUFFER_END(Props)
        float _EmissionAmount;
 
        struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };
 
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
            half test = tex2D(_SliceGuide, IN.uv_MainTex).rgb - UNITY_ACCESS_INSTANCED_PROP(Props, _SliceAmount);
            clip(test);
             
            if (test < _BurnSize && UNITY_ACCESS_INSTANCED_PROP(Props, _SliceAmount) > 0) {
                o.Emission = tex2D(_BurnRamp, float2(test * (1 / _BurnSize), 0)) * _BurnColor * _EmissionAmount;
            }
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

            o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}