Shader "psx/unlit" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
	SubShader
	{
		Tags{ "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				fixed4 pos : SV_POSITION;
				half4 color : COLOR0;
				half4 colorFog : COLOR1;
				float2 uv_MainTex : TEXCOORD0;
				half3 normal : TEXCOORD1;
			};

			float4 _MainTex_ST;
			uniform half4 unity_FogStart;
			uniform half4 unity_FogEnd;

			float4 _Color;
			float4 _EmissionColor;

			v2f vert(appdata_full v)
			{
				v2f o;

				//Vertex snapping
				float4 snapToPixel = UnityObjectToClipPos(v.vertex);
				float4 vertex = snapToPixel;
				vertex.xyz = snapToPixel.xyz / snapToPixel.w; //Vertex in 3D space
				vertex.x = floor(160 * vertex.x) / 160;
				vertex.y = floor(120 * vertex.y) / 120;
				vertex.xyz *= snapToPixel.w; //Vertex back in 2D space
				o.pos = vertex;

				//Vertex lighting 
				o.color = v.color * _Color * UNITY_LIGHTMODEL_AMBIENT + _EmissionColor;

				float distance = length(mul(UNITY_MATRIX_MV,v.vertex));

				//Standard texture mapping
				o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
				//Affine Texture Mapping - keep the line above
				//float4 affinePos = vertex;//vertex;				
				//o.uv_MainTex *= distance + (vertex.w * (UNITY_LIGHTMODEL_AMBIENT.a * 8)) / distance / 2;
				//o.normal = distance + (vertex.w * (UNITY_LIGHTMODEL_AMBIENT.a * 8)) / distance / 2;

				//Fog
				float4 fogColor = unity_FogColor;

				float fogDensity = (unity_FogEnd - distance) / (unity_FogEnd - unity_FogStart);
				o.normal.g = fogDensity;
				o.normal.b = 1;

				o.colorFog = fogColor;
				o.colorFog.a = clamp(fogDensity,0,1);

				//Cut out polygons
				//if (distance > unity_FogStart.z + unity_FogColor.a * 255)
				//{
				//	o.pos.w = 0;
				//}

				return o;
			}

			sampler2D _MainTex;

			float4 frag(v2f IN) : SV_Target
			{
				//Affine Texture Mapping version
				//half4 c = tex2D(_MainTex, IN.uv_MainTex / IN.normal.r) * IN.color;
				half4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
				half4 color = c * (IN.colorFog.a);
				color.rgb += IN.colorFog.rgb * (1 - IN.colorFog.a);
				return color;
			}
			ENDCG
		}
	}
		
	//Modified from Unity's standard cutout shader
	SubShader{
		Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
		LOD 100

		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				half4 color : COLOR0;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Cutoff;

			v2f vert(appdata_full v)
			{
				v2f o;

				//Vertex snapping
				float4 snapToPixel = UnityObjectToClipPos(v.vertex);
				float4 vertex = snapToPixel;
				vertex.xyz = snapToPixel.xyz / snapToPixel.w;
				vertex.x = floor(160 * vertex.x) / 160;
				vertex.y = floor(120 * vertex.y) / 120;
				vertex.xyz *= snapToPixel.w;
				o.vertex = vertex;

				//Vertex lighting 
				o.color = v.color * UNITY_LIGHTMODEL_AMBIENT;

				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				//Fog
				UNITY_TRANSFER_FOG(o,o.vertex);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
				clip(col.a - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}