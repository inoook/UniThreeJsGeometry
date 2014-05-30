// http://threejs.org/examples/webgl_geometries.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// http://threejs.org/examples/#webgl_geometries
public class GeoTest2 : ThreeBehaviour {

	public Material material;
	
	// Use this for initialization
	protected override void Init () {
		base.Init();

		THREE.Mesh threeMesh;

		float heightScale = 1;
		float p = 2;
		float q = 3;
		float radius = 150, tube = 10;
		int segmentsR = 50, segmentsT = 20;

		
		var torus2 = new THREE.TorusKnotGeometry( radius, tube, segmentsR, segmentsT, p , q, heightScale );
		var sphere2 = new THREE.ParametricGeometries.ParaSphereGeometry( 75, 20, 10 ).geo;

		// var torus = new THREE.TorusKnotGeometry( radius, tube, segmentsR, segmentsT, p , q, heightScale );
		// var sphere = new THREE.SphereGeometry( 75, 20, 10 );

		//var GrannyKnot =  new THREE.Curves.GrannyKnot();
		// var tube = new THREE.TubeGeometry( GrannyKnot, 150, 2, 8, true, false );
		
		
		// var benchmarkCopies = 1000;
		// var benchmarkObject = tube;
		// var rand = function() { return (Math.random() - 0.5 ) * 600; };
		// for (var b=0;b<benchmarkCopies;b++) {
		//    object = THREE.SceneUtils.createMultiMaterialObject( benchmarkObject, materials );
		//   object.position.set( rand(), rand(), rand() );
		//   scene.add( object );
		// }

		threeMesh = new THREE.Mesh( torus2, material );
		threeMesh.position = new Vector3( 0, 100, 0 );
		scene.Add( threeMesh );

		THREE.Geometry geo;

		// Klein Bottle
		
		geo = new THREE.ParametricGeometry( THREE.ParametricGeometries.klein, 20, 20 );
		threeMesh = new THREE.Mesh( geo, material );
		threeMesh.position = new Vector3( 0, 0, 0 );
		threeMesh.scale = Vector3.one * 10;
		scene.Add( threeMesh );

		// Mobius Strip
		
		geo = new THREE.ParametricGeometry( THREE.ParametricGeometries.mobius, 20, 20 );
		threeMesh = new THREE.Mesh( geo, material );
		threeMesh.position = new Vector3( 10, 0, 0 );
		threeMesh.scale = Vector3.one * 100;
		scene.Add( threeMesh );

		geo = new THREE.ParametricGeometry( THREE.ParametricGeometries.plane( 200, 200 ), 10, 20 );
		//geo = new THREE.ParametricGeometry( THREE.ParametricGeometries.plane, 10, 20 );
		threeMesh = new THREE.Mesh( geo, material );
		threeMesh.position = new Vector3( 0, 0, 0 );
		scene.Add( threeMesh );


//		threeMesh = new THREE.Mesh( torus2, material );
//		threeMesh.position = new Vector3( 0, 100, 0 );
//		scene.Add( threeMesh );

		threeMesh = new THREE.Mesh( sphere2, material );
		threeMesh.position = new Vector3( 200, 0, 0 );
		scene.Add( threeMesh );

		// error
//		var tube2 = new THREE.ParametricGeometries.TubeGeometry( GrannyKnot, 150, 2, 8, true ).geo;
//		threeMesh = new THREE.Mesh( tube2, material );
//		threeMesh.position = new Vector3( 100, 0, 0 );
//		scene.Add( threeMesh );
	}
	

	protected override void Render()
	{
		float timer = -Time.time * 1.0f;
		float camRotationSpeed = 0.15f;
		float x = Mathf.Cos( timer * camRotationSpeed ) * 800;
		float z = Mathf.Sin( timer * camRotationSpeed ) * 800;

		viewCamera.transform.position = new Vector3(x, 400, z);
		
		viewCamera.transform.LookAt( Vector3.zero );

		float amp = 5.0f;
		for ( int i = 0, l = scene.children.Count; i < l; i ++ ) {
			
			THREE.RenderableObject threeMesh = scene.children[ i ];

			threeMesh.eulerAngles.x = timer * 5.0f * amp;
			threeMesh.eulerAngles.y = timer * 2.5f * amp;

		}
	}
}
