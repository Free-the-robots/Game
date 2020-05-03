Shader "Custom/Billboard"
{
Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _ScaleX ("Scale X", Float) = 1.0
        _ScaleY ("Scale Y", Float) = 1.0
        _Rotation ("Rotation", Vector) = (0,0,0,0)
    }
 
    SubShader
    {
        Tags
        { 
            "Queue"="Geometry"
            "SortingLayer"="Resources_Sprites" 
            "IgnoreProjector"="True" 
            "RenderType"="Opaque" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
 
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
 
        Pass
        {
        CGPROGRAM
            //#pragma shader_feature IGNORE_ROTATION_AND_SCALE
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
 
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
 
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };
             
            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float _ScaleX;
            float _ScaleY;
            float4 _Rotation;

            fixed3x3 RotationMatrix(float3 euler){

                float3x3 rotMatrixX;
                const float deg_to_rad = 0.017453292;
                float cosx = cos(euler.x*deg_to_rad);
                float sinx = sin(euler.x*deg_to_rad);
                rotMatrixX[0].xyz = float3(1, 0, 0);
                rotMatrixX[1].xyz = float3(0, cosx, sinx);
                rotMatrixX[2].xyz = float3(0, - sinx, cosx);
                float3x3 rotMatrixY;
                float cosy = cos(euler.y*deg_to_rad);
                float siny = sin(euler.y*deg_to_rad);
                rotMatrixY[0].xyz = float3(cosy, 0, siny);
                rotMatrixY[1].xyz = float3(0, 1, 0);
                rotMatrixY[2].xyz = float3(-siny, 0, cosy);
                float3x3 rotMatrixZ;
                float cosz = cos(euler.z*deg_to_rad);
                float sinz = sin(euler.z*deg_to_rad);
                rotMatrixZ[0].xyz = float3(cosz, sinz, 0);
                rotMatrixZ[1].xyz = float3(-sinz, cosz, 0);
                rotMatrixZ[2].xyz = float3(0, 0, 1);

                return mul(mul(rotMatrixZ,rotMatrixY),rotMatrixX);
            }

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.texcoord = IN.texcoord.xy;
                OUT.color = IN.color * _Color;

                // The world position of the center of the object
                float3 worldPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
 
                // Distance between the camera and the center
                float3 dist = _WorldSpaceCameraPos - worldPos;
 
                // atan2(dist.x, dist.z) = atan (dist.x / dist.z)
                // With atan the tree inverts when the camera has the same z position`
                float angle = atan2(dist.x, dist.z);
                float3x3 rotMatrixX;
                float cosinusX = cos(1.57);
                float sinusX = sin(1.57);
       
                // Rotation matrix in Y
                rotMatrixX[0].xyz = float3(1, 0, 0);
                rotMatrixX[1].xyz = float3(0, cosinusX, sinusX);
                rotMatrixX[2].xyz = float3(0, - sinusX, cosinusX);
 
                float3x3 rotMatrix;
                float cosinus = cos(angle);
                float sinus = sin(angle);
       
                // Rotation matrix in Y
                rotMatrix[0].xyz = float3(cosinus, 0, sinus);
                rotMatrix[1].xyz = float3(0, 1, 0);
                rotMatrix[2].xyz = float3(- sinus, 0, cosinus);
 
                // The position of the vertex after the rotation
                float4 newPos = float4(mul(rotMatrix, mul(IN.vertex * float4(_ScaleX, _ScaleY, 0, 0),RotationMatrix(_Rotation.xyz))), 1);
 
                OUT.vertex = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, newPos));

                return OUT;
            }
 
            fixed4 SampleSpriteTexture (float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);
 
#if ETC1_EXTERNAL_ALPHA
                // get the color from an external texture (usecase: Alpha support for ETC1 on android)
                color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA
 
                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                c.rgb *= c.a;
                return c;
            }
        ENDCG
        }
    }
}