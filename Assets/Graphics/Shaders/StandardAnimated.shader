﻿Shader "Custom/StandardAnimated"
{
    Properties
    {
        //_TextureIdx("Texture Idx", float) = 0
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        //_Textures("Textures", 2DArray) = "" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Fade"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "ForceNoShadowCasting" = "True"
        }
        Lighting Off
        Blend One OneMinusSrcAlpha
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        //UNITY_DECLARE_TEX2DARRAY(_Textures);

        //UNITY_INSTANCING_BUFFER_START(Props)
        //   UNITY_DEFINE_INSTANCED_PROP(float, _TextureIdx)
        //UNITY_INSTANCING_BUFFER_END(Props)

        struct Input
        {
            fixed2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        //UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        //UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //UNITY_ACCESS_INSTANCED_PROP(Props, _TextureIdx);
            // Albedo comes from a texture tinted by color
            //fixed4 c = UNITY_SAMPLE_TEX2DARRAY(
            //    _Textures,
            //    float3(IN.uv_MainTex, UNITY_ACCESS_INSTANCED_PROP(Props, _TextureIdx))
            //);

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            //o.Alpha = tex2D (_MainTex, IN.uv_MainTex).a;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
