Shader "CSCI4160U/BloodSpatterEffect"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
		_BloodTex("Blood Texture", 2D) = "white" {}
		_BloodBump("Blood Bump", 2D) = "bump" {}
		_Distortion("Blood Distortion", Range(0, 2)) = 0
		_BloodAmount("Blood Amount", Range(0, 1)) = 0
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			sampler2D _MainTex;
			sampler2D _BloodTex;
			sampler2D _BloodBump;
			float _Distortion;
			float _BloodAmount;

            fixed4 frag (v2f i) : SV_Target {
				half2 bump = UnpackNormal(tex2D(_BloodBump, i.uv)).xy;
				fixed4 bloodColour = tex2D(_BloodTex, i.uv);
				fixed4 srcColour = tex2D(_MainTex, i.uv + bump * bloodColour.a * _Distortion);
				bloodColour.a = saturate(bloodColour.a + (_BloodAmount * 2 - 1));
				return lerp(srcColour, bloodColour, bloodColour.a);
            }
            ENDCG
        }
    }
}
