// http://threejs.org/examples/webgl_geometries.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// http://threejs.org/examples/#webgl_geometries
public class GeoTest : ThreeBehaviour {

	public Material material;
	public Material wireMaterial;
	
	// Use this for initialization
	protected override void Init () {
		base.Init();

		THREE.Mesh drawNormalMesh;

		THREE.Mesh threeMesh;
		threeMesh = new THREE.Mesh( new THREE.SphereGeometry( 75, 20, 10 ), material );
		threeMesh.position = new Vector3( -400, 0, 200 );
		scene.Add( threeMesh );

		threeMesh = new THREE.Mesh( new THREE.IcosahedronGeometry( 75, 1 ), material );
		threeMesh.position = new Vector3( -200, 0, 200 );
		scene.Add( threeMesh );
		
		threeMesh = new THREE.Mesh( new THREE.OctahedronGeometry( 75, 2 ), material );
		threeMesh.position = new Vector3( 0, 0, 200 );
		scene.Add( threeMesh );
		
		threeMesh = new THREE.Mesh( new THREE.TetrahedronGeometry( 75, 0 ), material );
		threeMesh.position = new Vector3( 200, 0, 200 );
		scene.Add( threeMesh );

		threeMesh = new THREE.Mesh( new THREE.PlaneGeometry( 100, 100, 4, 4 ), material );
		threeMesh.position = new Vector3( -400, 0, 0 );
		scene.Add( threeMesh );
		
		threeMesh = new THREE.Mesh( new THREE.BoxGeometry( 100, 100, 100, 4, 4, 4 ), material );
		threeMesh.position = new Vector3( -200, 0, 0 );
		scene.Add( threeMesh );

		drawNormalMesh = threeMesh; // Draw Normal Mesh
		
		threeMesh = new THREE.Mesh( new THREE.CircleGeometry( 50, 20, 0, Mathf.PI * 2 ), material );
		threeMesh.position = new Vector3( 0, 0, 0 );
		scene.Add( threeMesh );
		
		threeMesh = new THREE.Mesh( new THREE.RingGeometry( 10, 50, 20, 5, 0, Mathf.PI * 2 ), material );
		threeMesh.position = new Vector3( 200, 0, 0 );
		scene.Add( threeMesh );
		
		threeMesh = new THREE.Mesh( new THREE.CylinderGeometry( 25, 75, 100, 40, 5, false ), material );
		threeMesh.position = new Vector3( 400, 0, 0 );
		scene.Add( threeMesh );


		//
		List<Vector3> points = new List<Vector3>();
		
		for ( var i = 0; i < 50; i ++ ) {
			points.Add( new Vector3( Mathf.Sin( i * 0.2f ) * Mathf.Sin( i * 0.1f ) * 15 + 50, 0, ( i - 5 ) * 2 ) );
		}
		
		threeMesh = new THREE.Mesh( new THREE.LatheGeometry( points, 20 ), material );
		threeMesh.position = new Vector3( -400, 0, -200 );
		scene.Add( threeMesh );
		
		threeMesh = new THREE.Mesh( new THREE.TorusGeometry( 50, 20, 20, 20 ), material );
		threeMesh.position = new Vector3( -200, 0, -200 );
		scene.Add( threeMesh );
		
		threeMesh = new THREE.Mesh( new THREE.TorusKnotGeometry( 50, 10, 50, 20 ), material );
		threeMesh.position = new Vector3( 0, 0, -200 );
		scene.Add( threeMesh );


		/*
		threeMesh = new THREE.AxisHelper( 50 );
		threeMesh.position.set( 200, 0, -200 );
		scene.add( threeMesh );
		
		threeMesh = new THREE.ArrowHelper( new THREE.Vector3( 0, 1, 0 ), new THREE.Vector3( 0, 0, 0 ), 50 );
		threeMesh.position.set( 400, 0, -200 );
		scene.add( threeMesh );
		*/

		THREE.Line lineMesh = new THREE.Line( new THREE.Geometry(), wireMaterial);
		lineMesh.position = drawNormalMesh.position;

		THREE.Geometry geo = drawNormalMesh.geo;
		for(int i = 0; i < geo.faces.Count; i++){
			THREE.Face3 _face = geo.faces[i];
			List<int> tri = _face.GetTriangles();

			for(int n = 0; n < tri.Count; n++){
				Vector3 normal = _face.vertexNormals[n];
				THREE.ArrowHelper arrow = new THREE.ArrowHelper(normal, geo.vertices[tri[n]] , 10, Color.green);
				lineMesh.Add( arrow );
			}
		}

		scene.Add( lineMesh );
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
