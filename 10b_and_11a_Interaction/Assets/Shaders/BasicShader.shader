Shader "CSCI4160U/BasicShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Colour("Colour", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			float4 _Colour;
            float4 _MainTex_ST;    // x,y - scale; y,w - offset

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f input) : SV_Target {
				fixed4 textureColour = tex2D(_MainTex, input.uv);
				if (textureColour.a < 1.0) {
					textureColour = float4(1, 1, 1, 1);
				}
				return lerp(textureColour, _Colour, textureColour.a);
				//return textureColour * _Colour;
            }
            ENDCG
        }
    }
}
