Shader "Unlit/Line" { 
	Properties { _Color("Color", Color) = (0, 1, 1, 1)   }
	SubShader {  Lighting Off Color[_Color] Pass {} } 
}
