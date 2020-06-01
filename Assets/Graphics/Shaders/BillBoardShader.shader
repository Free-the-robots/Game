Shader "Custom/Billboard"
{
Properties
    {
        _TextureIdx("Texture Idx", float) = 0
        _Textures("Textures", 2DArray) = "" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _ScaleX ("Scale X", Float) = 1.0
        _ScaleY ("Scale Y", Float) = 1.0
        _Rotation ("Rotation", Vector) = (0,0,0,0)
    }
 
    SubShader
    {
        Tags
        { 
            "Queue"="Geometry+1"
            "IgnoreProjector"="True" 
            "RenderType"="Opaque" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "ForceNoShadowCasting" = "True"
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
             
            //fixed4 _Color;
            //sampler2D _MainTex;

            UNITY_DECLARE_TEX2DARRAY(_Textures);

            UNITY_INSTANCING_BUFFER_START(Props)
               UNITY_DEFINE_INSTANCED_PROP(fixed, _TextureIdx)
               UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
               UNITY_DEFINE_INSTANCED_PROP(fixed2, _Scale)
            UNITY_INSTANCING_BUFFER_END(Props)

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
                OUT.color = IN.color * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);

/*
                // The world position of the center of the object
                float3 worldPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
 
                // Distance between the camera and the center
                float3 dist = _WorldSpaceCameraPos - worldPos;
 
                // With atan the tree inverts when the camera has the same z position`
                float angle = atan2(dist.x, dist.z);
 
                float3x3 rotMatrix;
                float cosinus = cos(angle);
                float sinus = sin(angle);
       
                // Rotation matrix in Y
                rotMatrix[0].xyz = float3(cosinus, 0, sinus);
                rotMatrix[1].xyz = float3(0, 1, 0);
                rotMatrix[2].xyz = float3(- sinus, 0, cosinus);
 
                // The position of the vertex after the rotation

                float3 mPos = mul(RotationMatrix(_Rotation.xyz),IN.vertex.xyz * float3(_ScaleX, _ScaleY, 0));
                float4 newPos = float4(mul(rotMatrix, mPos),1);
                OUT.vertex = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, newPos));//mul(unity_ObjectToWorld, newPos));
*/

                float4x4 modelView = UNITY_MATRIX_MV;
                // Column 0:
                modelView[0][0] = 1;
                modelView[0][1] = 0;
                modelView[0][2] = 0;

                // Column 1:
                modelView[1][0] = 0;
                modelView[1][1] = 1;
                modelView[1][2] = 0;

                // Column 2:
                modelView[2][0] = 0;
                modelView[2][1] = 0;
                modelView[2][2] = 1;

                float3 mPos = mul(RotationMatrix(_Rotation.xyz),IN.vertex.xyz * float3(UNITY_ACCESS_INSTANCED_PROP(Props, _Scale).x, UNITY_ACCESS_INSTANCED_PROP(Props, _Scale).y, 0));
                OUT.vertex = mul(UNITY_MATRIX_P,mul(modelView,float4(mPos,1)));

                return OUT;
            }
 
            fixed4 SampleSpriteTexture (float2 uv)
            {
                fixed4 color = UNITY_SAMPLE_TEX2DARRAY(_Textures,float3(uv, UNITY_ACCESS_INSTANCED_PROP(Props, _TextureIdx)));
                //fixed4 color = tex2D (_MainTex, uv);
 
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