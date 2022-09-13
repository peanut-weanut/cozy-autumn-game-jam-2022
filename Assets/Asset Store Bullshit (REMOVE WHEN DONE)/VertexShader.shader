Shader "Custom/VertexShader" {
    Properties{
        _Tint ("Tint", Color) = (150, 70, 0, 1)
    }
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf vertex:vert finalcolor:colortint
		#pragma target 3.0

		struct Input {
			float4 vertColor;
		};
            fixed4 _Tint;
        void colortint (Input IN, SurfaceOutput o, inout fixed4 color){
            color *= _Tint/255;
        }
		void vert(inout appdata_full v, out Input o){
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertColor = v.color + (_Tint/255);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = IN.vertColor.rgb;
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}