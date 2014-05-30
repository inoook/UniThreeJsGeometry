using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// http://threejs.org/examples/#webgl_geometry_nurbs
public class Subdivision : ThreeBehaviour {

	public Material material;
	
	// Use this for initialization
	protected override void Init () {
		base.Init();

		THREE.Geometry boxGeo = new THREE.BoxGeometry( 200, 200, 200, 2, 2, 2 );
//		THREE.Mesh threeMesh = new THREE.Mesh( new THREE.BoxGeometry( 200, 200, 200, 2, 2, 2 ), material );
//		threeMesh.position = new Vector3( 0, 0, 0 );
//		//scene.Add( threeMesh );

		int subdivisions = 2;
		THREE.SubdivisionModifier modifier = new THREE.SubdivisionModifier( subdivisions );

		//THREE.Geometry smooth = threeMesh.geo;
		THREE.Geometry smooth = boxGeo;

		modifier.modify( smooth );


		THREE.Mesh subdivisionMesh = new THREE.Mesh( smooth, material );
		subdivisionMesh.position = new Vector3( 0, 0, 0 );
		
		scene.Add( subdivisionMesh );
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
