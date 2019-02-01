// http://threejs.org/examples/webgl_geometries.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

// http://threejs.org/examples/#webgl_geometries
public class GeoTest3 : MonoBehaviour {

	public Material material;
	
	// Use this for initialization
	void Start () {


		float heightScale = 1;
		float p = 2;
		float q = 3;
		float radius = 150, tube = 10;
		int segmentsR = 50, segmentsT = 20;

		
		var torus2 = new TorusKnotGeometry( radius, tube, segmentsR, segmentsT, p , q, heightScale );
		
		// var torus = new TorusKnotGeometry( radius, tube, segmentsR, segmentsT, p , q, heightScale );
		// var sphere = new SphereGeometry( 75, 20, 10 );

		//var GrannyKnot =  new Curves.GrannyKnot();
		// var tube = new TubeGeometry( GrannyKnot, 150, 2, 8, true, false );
		
		
		// var benchmarkCopies = 1000;
		// var benchmarkObject = tube;
		// var rand = function() { return (Math.random() - 0.5 ) * 600; };
		// for (var b=0;b<benchmarkCopies;b++) {
		//    object = SceneUtils.createMultiMaterialObject( benchmarkObject, materials );
		//   object.position.set( rand(), rand(), rand() );
		//   scene.add( object );
		// }
		AddRenderObject( torus2, material, new Vector3( 0, 100, 0 ));

//		Geometry geo;

		// Klein Bottle
		AddRenderObject( new ParametricGeometry( ParametricGeometries.klein, 20, 20 ), material, new Vector3( 0, 0, 0 ) );

		// Mobius Strip
		AddRenderObject( new ParametricGeometry( ParametricGeometries.mobius, 20, 20 ), material, new Vector3( 10, 0, 0 ));
		
		//geo = new ParametricGeometry( ParametricGeometries.plane, 10, 20 );
		AddRenderObject( new ParametricGeometry( ParametricGeometries.plane( 200, 200 ), 10, 20 ), material, new Vector3( 0, 0, 0 ) );

		AddRenderObject( new ParametricGeometries.ParaSphereGeometry( 75, 20, 10 ).geo, material, new Vector3( 200, 0, 0 ));

	}
	
	Geometry AddRenderObject(Geometry geo, Material material, Vector3 position)
	{
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
}
