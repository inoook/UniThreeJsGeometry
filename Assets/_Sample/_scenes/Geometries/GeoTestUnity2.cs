// http://threejs.org/examples/webgl_geometries.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// http://threejs.org/examples/#webgl_geometries
public class GeoTestUnity2 : MonoBehaviour {

	public Material material;
	public Material wireMaterial;
	
	// Use this for initialization
	void Start () {

		THREE.Geometry geometry;
		
		AddRenderObject( new THREE.SphereGeometry( 75, 20, 10 ), material, new Vector3( -400, 0, 200 ));
		
		AddRenderObject( new THREE.IcosahedronGeometry( 75, 1 ), material, new Vector3( -200, 0, 200 ) );

		AddRenderObject( new THREE.OctahedronGeometry( 75, 2 ), material, new Vector3( 0, 0, 200 ) );

		AddRenderObject( new THREE.TetrahedronGeometry( 75, 0 ), material, new Vector3( 200, 0, 200 ) );

		AddRenderObject( new THREE.PlaneGeometry( 100, 100, 4, 4 ), material, new Vector3( -400, 0, 0 ) );

		geometry = AddRenderObject( new THREE.BoxGeometry( 100, 100, 100, 4, 4, 4 ), material, new Vector3( -200, 0, 0 ) );

		AddRenderObject( new THREE.CircleGeometry( 50, 20, 0, Mathf.PI * 2 ), material, new Vector3( 0, 0, 0 ) );

		AddRenderObject( new THREE.RingGeometry( 10, 50, 20, 5, 0, Mathf.PI * 2 ), material, new Vector3( 200, 0, 0 ) );
		
		AddRenderObject( new THREE.CylinderGeometry( 25, 75, 100, 40, 5, false ), material, new Vector3( 400, 0, 0 ) );

		//
		List<Vector3> points = new List<Vector3>();
		for ( var i = 0; i < 50; i ++ ) {
			points.Add( new Vector3( Mathf.Sin( i * 0.2f ) * Mathf.Sin( i * 0.1f ) * 15 + 50, 0, ( i - 5 ) * 2 ) );
		}
		AddRenderObject( new THREE.LatheGeometry( points, 20 ), material, new Vector3( -400, 0, -200 ) );

		AddRenderObject( new THREE.TorusGeometry( 50, 20, 20, 20 ), material, new Vector3( -200, 200, -200 ) );

		AddRenderObject( new THREE.TorusKnotGeometry( 50, 10, 50, 20 ), material, new Vector3( 0, 0, -200 ) );
		

		THREE.Line lineMesh = new THREE.Line( new THREE.Geometry(), wireMaterial);
		THREE.Geometry geo = geometry;
		for(int i = 0; i < geo.faces.Count; i++){
			THREE.Face3 _face = geo.faces[i];
			int[] tri = _face.GetTriangles();

			for(int n = 0; n < tri.Length; n++){
				Vector3 normal = _face.vertexNormals[n];
				THREE.ArrowHelper arrow = new THREE.ArrowHelper(normal, geo.vertices[tri[n]] , 10, Color.green);
				lineMesh.Add( arrow );
			}
		}

		AddRenderLineObject( lineMesh.geo, wireMaterial, new Vector3( -200, 0, 0 ) );
	}

	THREE.Geometry AddRenderObject(THREE.Geometry geo, Material material, Vector3 position)
	{
//		geo.computeFaceNormals();
//		geo.copyFaceNormalToVertexNormals();

		UnityEngine.Mesh mesh = geo.GetMesh();

		GameObject gObj = new GameObject();
		gObj.transform.SetParent(this.transform);
		gObj.transform.localPosition = position * 0.01f;
		gObj.transform.localScale = Vector3.one * 0.01f;

		MeshFilter mf = gObj.AddComponent<MeshFilter>();
		MeshRenderer mr = gObj.AddComponent<MeshRenderer>();

		mf.mesh = mesh;
		mr.material = material;

		return geo;
	}

	THREE.Geometry AddRenderLineObject(THREE.Geometry geo, Material material, Vector3 position)
	{
		UnityEngine.Mesh mesh = geo.GetLineMesh();
		
		GameObject gObj = new GameObject();
		gObj.transform.SetParent(this.transform);
		gObj.transform.localPosition = position * 0.01f;
		gObj.transform.localScale = Vector3.one * 0.01f;
		
		MeshFilter mf = gObj.AddComponent<MeshFilter>();
		MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
		
		mf.mesh = mesh;
		mr.material = material;
		
		return geo;
	}
}
