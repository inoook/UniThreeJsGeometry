// http://forum.unity3d.com/threads/8814-Wireframe-3D/page3
using UnityEngine;
using System.Collections.Generic;

public class WireframeRenderer : MonoBehaviour
{
	Mesh LastMesh;

	Material LastMaterial;
//	string lineshader = "Shader \"Unlit/Color\" { Properties { _Color(\"Color\", Color) = (0, 1, 1, 1)   } " +
//	                       "SubShader {  Lighting Off Color[_Color] Pass {} } }";

	public void OnEnable ()
	{

		var mesh = gameObject.GetComponent<MeshFilter> ().sharedMesh;
		var renderer = gameObject.GetComponent<MeshRenderer> ();
		
		LastMaterial = renderer.material;
		LastMesh = mesh;

		Vector3[] vertices = mesh.vertices;
		var triangles = mesh.triangles;
		var lines = new Vector3[triangles.Length];

		int[] indexBuffer;

		var GeneratedMesh = new Mesh ();
		
		for (var t = 0; t < triangles.Length; t++) {
			lines [t] = (vertices [triangles [t]]);
		}

		GeneratedMesh.vertices = lines;
		GeneratedMesh.name = "Generated Wireframe";

		var LinesLength = lines.Length;
		indexBuffer = new int[LinesLength];
		var uvs = new Vector2[LinesLength];
		var normals = new Vector3[LinesLength];
		

		for (var m = 0; m < LinesLength; m++) {
			indexBuffer [m] = m;
			uvs [m] = new Vector2 (0.0f, 1.0f); // sets a fake UV (FAST)
			normals [m] = new Vector3 (1, 1, 1);// sets a fake normal
		}
		
		// 頂点のカラーをランダムに。shaderを GUI/Text Shader などに設定する必要あり
		Color[] colors = new Color[LinesLength];
		int i = 0;
		while (i < LinesLength) {
			colors [i] = Color.Lerp (Color.red, Color.green, Random.value);
			i++;
		}
		GeneratedMesh.colors = colors;
		//
		GeneratedMesh.uv = uvs;
		GeneratedMesh.normals = normals;
		GeneratedMesh.SetIndices (indexBuffer, MeshTopology.LineStrip, 0);
		
		
		gameObject.GetComponent<MeshFilter> ().mesh = GeneratedMesh;

//		Material tempmaterial = new Material (lineshader);
		Shader shader = Shader.Find("Unlit/Line");
		Material tempmaterial = new Material (shader);

		renderer.material = tempmaterial;
	}

	void OnDisable ()
	{
		gameObject.GetComponent<MeshFilter> ().mesh = LastMesh;
		gameObject.GetComponent<MeshRenderer> ().material = LastMaterial;
		
	}
	
}