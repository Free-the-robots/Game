Shader "CustomShader/Shield"
{
	//afficher les property dans l'editor
	Properties
	{
		_Color("Color Shield", COLOR) = (0,0,0,0)
		_PulseTex("Hex texture", 2D) = "white" {}
		_PulseIntensity ("Pulse Intensity", float) = 3.0
		_PulseTimeScale ("Time Scale",float) = 2.0
		_PulsePosScale ("Speed pulse from pivot", float) = 50.0
		_PulseEdge ("Pulse Edge", 2D) = "white" {}
		_EdgeIntensity("Pulse Edge Intensity", float) = 2.0
		_EdgeColor ("Edge color ", COLOR) = (0,0,0,0)
		_EdgeTexture ("Edge Texture", 2D) = "white" {}
		_EdgeTextureIntensity ("Edge Texture Intensity", float) = 10.0
		_EdgeExponent ("Exponent gradient", float) = 6.0
	}
	SubShader
	{
		Pass 
		{
			Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
			Cull Off
			Blend SrcAlpha One
			HLSLPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0; 
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 vertexObjPos : TEXCOORD1;
			};

			float4 _Color;
			sampler2D _PulseTex;
			float4 _PulseTex_ST;
			float _PulseIntensity;
			float _PulseTimeScale;
			float _PulsePosScale;
			sampler2D _PulseEdge;
			float4 _PulseEdge_ST;
			float _EdgeIntensity;
			float4 _EdgeColor;
			sampler2D _EdgeTexture;
			float _EdgeTextureIntensity;
			float _EdgeExponent;
			sampler2D _CameraDepthNormalsTexture;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _PulseTex);
				o.vertexObjPos = v.vertex;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//final output
				fixed4 pulseTex = tex2D(_PulseTex, i.uv);
				fixed4 pulseEdge = tex2D(_PulseEdge, i.uv);
				fixed4 edgeLine = tex2D(_EdgeTexture, i.uv);
				fixed4 edgePulseHex = pulseEdge * _EdgeColor * _EdgeIntensity;
				fixed4 pulseTerm = pulseTex * _Color * _PulseIntensity * abs(sin(_Time.y * _PulseTimeScale - abs(i.vertexObjPos.x) * _PulsePosScale + pulseTex));
				fixed4 edgeLineColor = pow(edgeLine.a, _EdgeExponent) * _Color * _EdgeTextureIntensity;
				return fixed4(_Color.rgb + pulseTerm.rgb + edgePulseHex + edgeLineColor, _Color.a);
			}

			ENDHLSL
		}
	}
}
