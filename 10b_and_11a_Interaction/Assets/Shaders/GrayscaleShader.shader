Shader "CSCI4160U/GrayscaleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_EffectAmount ("Effect Amount", Range(0.0, 1.0)) = 0.0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
			float4 _MainTex_ST;
			float _EffectAmount;

			v2f vert (appdata input)
            {
                v2f output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }

            fixed4 frag (v2f input) : SV_Target {
				fixed4 colour = tex2D(_MainTex, input.uv);
				float lum = Luminance(colour.rgb);
				fixed4 grayscale = float4(lum, lum, lum, colour.a);
				return lerp(colour, grayscale, _EffectAmount);
            }
            ENDCG
        }
    }
}
