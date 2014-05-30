// http://forum.unity3d.com/threads/93189-Two-sided-cutout-shader-for-U3-wanted
// http://answers.unity3d.com/questions/187630/writing-a-double-sided-shader-how-to-reverse-the-n.html

Shader "Diffuse_DoubleSide" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200
	Cull Back
	
	CGPROGRAM
	#pragma surface surf Lambert
	
	sampler2D _MainTex;
	fixed4 _Color;
	
	struct Input {
		float2 uv_MainTex;
	};
	
	void surf (Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	
	Cull Front
	Offset 0, 1
	CGPROGRAM
	#pragma surface surf Lambert vertex:vert
	
	
	void vert (inout appdata_full v) {
		v.normal = -v.normal;
	}
      
	sampler2D _MainTex;
	fixed4 _Color;
	
	struct Input {
		float2 uv_MainTex;
	};
	
	void surf (Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
}

Fallback "VertexLit"
}
