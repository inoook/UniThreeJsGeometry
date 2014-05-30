// http://threejs.org/examples/webgl_geometry_extrude_shapes.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class ExtrudeTest : ThreeBehaviour {

	public Material material;

	THREE.Geometry testGeometry;

	List<Vector2> pts;
	THREE.ClosedSplineCurve3 closedSpline;
	// Use this for initialization
	protected override void Init () {
		base.Init();
		
		closedSpline = new THREE.ClosedSplineCurve3( new List<Vector3>(new Vector3[]{
		                                                  new Vector3( -60, -100,  60 ),
		                                                  new Vector3( -60,   20,  60 ),
		                                                  new Vector3( -60,  120,  60 ),
		                                                  new Vector3(  60,   20, -60 ),
		                                                  new Vector3(  60, -100, -60 )
		}));
		
		THREE.ExtrudeGeometry.Option extrudeSettings  = new THREE.ExtrudeGeometry.Option();
		extrudeSettings.steps = 100;
		extrudeSettings.bevelEnabled = false;
		extrudeSettings.extrudePath	= closedSpline;

		pts = new List<Vector2>();
		int count = 3;
		
		for ( int i = 0; i < count; i ++ ) {
			float l = 20;
			float a = 2 * (float)i / count * Mathf.PI;
			pts.Add( new Vector2 ( Mathf.Cos( a ) * l, Mathf.Sin( a ) * l ) );
		}
		
		THREE.Shape shape = new THREE.Shape( pts );

		THREE.Mesh threeMesh;

		testGeometry = new THREE.ExtrudeGeometry(new List<THREE.Shape>(new THREE.Shape[]{ shape }), extrudeSettings );
		//THREE.ShapeGeometry.Option op0 = new THREE.ShapeGeometry.Option();
		//op0.curveSegments = 12;
		//THREE.Geometry geometry = new THREE.ShapeGeometry(new List<Shape>( new Shape[]{shape} ), op0);
		threeMesh = new THREE.Mesh( testGeometry, material);
		scene.Add( threeMesh );

		/////
		List<Vector3> randomPoints = new List<Vector3>();
		for ( int i = 0; i < 10; i ++ ) {
			randomPoints.Add( new Vector3( ( i - 4.5f ) * 50, Random.Range( - 50.0f, 50.0f ), Random.Range( - 50.0f, 50.0f ) ) );
		}
		
		SplineCurve3 randomSpline =  new SplineCurve3( randomPoints );

		THREE.ExtrudeGeometry.Option extrude0Settings  = new THREE.ExtrudeGeometry.Option();
		extrude0Settings.steps = 200;
		extrude0Settings.bevelEnabled = false;
		extrude0Settings.extrudePath = randomSpline;


		List<Vector2> pts0 = new List<Vector2>();
		int numPts = 5;
		
		for ( int i = 0; i < numPts * 2; i ++ ) {
			int l = i % 2 == 1 ? 10 : 20;
			float a = (float)i / numPts * Mathf.PI;
			pts0.Add( new Vector2 ( Mathf.Cos( a ) * l, Mathf.Sin( a ) * l ) );
		}
		
		Shape shape0 = new Shape( pts0 );
		testGeometry = new THREE.ExtrudeGeometry( new List<Shape>(new Shape[]{ shape0 }), extrude0Settings );
		threeMesh = new THREE.Mesh( testGeometry, material);
		scene.Add( threeMesh );

		////////////
		THREE.ExtrudeGeometry.Option extrude1Settings  = new THREE.ExtrudeGeometry.Option();
		extrude1Settings.amount = 20;
		extrude1Settings.steps = 1;
		extrude1Settings.bevelEnabled = false;
		extrude1Settings.bevelThickness = 2;
		extrude1Settings.bevelSize = 4;
		extrude1Settings.bevelSegments = 1;

		testGeometry = new THREE.ExtrudeGeometry( new List<Shape>(new Shape[]{ shape0 }), extrude1Settings );
		threeMesh = new THREE.Mesh( testGeometry, material);
		threeMesh.position = new Vector3(50, 100, 50 );
		scene.Add( threeMesh );

		//
		// Sphere
//		threeMesh = new THREE.Mesh( new THREE.SphereGeometry( 75, 20, 10 ), material );
//		threeMesh.position = new Vector3( -400, 0, 200 );
//		scene.Add( threeMesh );

//		//
//		Shape shapeTri = new Shape();
//		shapeTri.moveTo(  0, 100 );
//		shapeTri.lineTo(  100, -50 );
//		shapeTri.lineTo( -100, -50 );
//		shapeTri.lineTo(  0, 100 );
//
//		THREE.ShapeGeometry.Option op = new THREE.ShapeGeometry.Option();
//		op.curveSegments = 12;
//		THREE.Geometry geometryShape = new THREE.ShapeGeometry(new List<Shape>( new Shape[]{shapeTri} ), op);
//		threeMesh = new THREE.Mesh( geometryShape, material);
//		scene.Add( threeMesh );



	}
	/*
	// Update is called once per frame
	void Update () {
		for(int i = 0; i< pts.Count; i++){
			Vector3 pt0 = pts[i];
			int endIndex = i+1;
			if(endIndex >= pts.Count){
				endIndex = 0;
			}
			Vector3 pt1 = pts[endIndex];
			Debug.DrawLine(pt0, pt1, Color.red);
		}
	}
	*/
	protected override void Render()
	{
//		int step = 60;
//		float offset = 1.0f / step;
//
//		for(int i = 0; i < step; i++){
//			Vector3 pt0 = closedSpline.getPoint((float)i * offset);
//			float end = (float)i+1;
//			if(end >= step){ end = 0; }
//			Vector3 pt1 = closedSpline.getPoint((float)end * offset);
//
//			Debug.DrawLine(pt0, pt1, Color.green);
//		}
//
//		for(int i = 0; i < pts.Count; i++){
//			Vector3 pt0 = pts[i];
//			int end = i+1;
//			if(end >= pts.Count){ end = 0; }
//			Vector3 pt1 = pts[end];
//			
//			Debug.DrawLine(pt0, pt1, Color.red);
//		}
//
//		//Debug.Log("testGeometry.vertices.Count: "+testGeometry.vertices.Count);
//		for(int i = 0; i < testGeometry.vertices.Count; i++){
//			Debug.DrawRay(Vector3.zero,  testGeometry.vertices[i], Color.cyan);
//		}
	}
}
