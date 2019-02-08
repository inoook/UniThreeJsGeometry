﻿// http://threejs.org/examples/webgl_geometry_extrude_shapes.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class ExtrudeTestUnityMesh : MonoBehaviour {

	public Material[] materials;

	ExtrudeGeometry.Option extrudeSettings;
	ClosedSplineCurve3 closedSpline;

    ExtrudeGeometry testGeometry0;
    ExtrudeGeometry.Option extrude0Settings;
	SplineCurve3 randomSpline;
    
	// Use this for initialization
	void Start () {

		Geometry testGeometry;

		// ---------------
		// closed path
		closedSpline = new ClosedSplineCurve3( new List<Vector3>(new Vector3[]{
		                                                  new Vector3( -60, -100,  60 ),
		                                                  new Vector3( -60,   20,  60 ),
		                                                  new Vector3( -60,  120,  60 ),
		                                                  new Vector3(  60,   20, -60 ),
		                                                  new Vector3(  60, -100, -60 )
		}));

//		SplineCurve3 closedSpline = new SplineCurve3( new List<Vector3>(new Vector3[]{
//			new Vector3( 0, 0, 0 ),
//			new Vector3( 150, 0, 0 ),
//			new Vector3( 0, 150, 0 ),
//		}));
		
		extrudeSettings  = new THREE.ExtrudeGeometry.Option();
		extrudeSettings.steps = 30; // 30, 100
		extrudeSettings.bevelEnabled = false;
		extrudeSettings.extrudePath	= closedSpline;
        extrudeSettings.uvGenerator = new UVGenerator();
        //extrudeSettings.curveSegments = 4;

        List<Vector2> pts = new List<Vector2>();
		List<Vector2> normals = new List<Vector2>();
		int count = 7;

		float normOffset = Mathf.PI / count;
		for ( int i = 0; i < count; i ++ ) {
			float l = 20;
			float a = (float)i / count * Mathf.PI * 2;
			pts.Add( new Vector2 ( Mathf.Cos( a ) * l, Mathf.Sin( a ) * l ) );

			normals.Add( new Vector2 ( Mathf.Cos( a + normOffset ), Mathf.Sin( a + normOffset ) ) );
		}

		Shape shape = new Shape( pts, normals );
		ShapeGeometry.Option op = new ShapeGeometry.Option ();

		// test
		Geometry shapeGeo = new ShapeGeometry (shape, op);
		AddRenderObject(shapeGeo, materials[0], Vector3.zero);
        
		testGeometry = new ExtrudeGeometry( shape, extrudeSettings );
		//testGeometry.computeVertexNormals();

		//ShapeGeometry.Option op0 = new ShapeGeometry.Option();
		//op0.curveSegments = 12;
		//Geometry geometry = new ShapeGeometry(new List<Shape>( new Shape[]{shape} ), op0);

		AddRenderObject(testGeometry, materials[0], Vector3.zero);


		// random path
		List<Vector3> randomPoints = new List<Vector3>();
		for ( int i = 0; i < 10; i ++ ) {
			//randomPoints.Add( new Vector3( ( i - 4.5f ) * 50, Random.Range( - 50.0f, 50.0f ), Random.Range( - 50.0f, 50.0f ) ) );
			randomPoints.Add( new Vector3( ( i ) * 50, Random.Range( - 50.0f, 50.0f ), Random.Range( - 50.0f, 50.0f ) ) );
			//randomPoints.Add( new Vector3( i * 50, i, i*2 ) );
		}
//		for ( int i = 0; i < 3; i ++ ) {
//			randomPoints.Add( new Vector3( 0, i * 30, i * 40 ) );
//		}
		randomSpline =  new SplineCurve3( randomPoints );

		extrude0Settings  = new ExtrudeGeometry.Option();
		extrude0Settings.steps = 80;// 80
		extrude0Settings.bevelEnabled = false;
		extrude0Settings.extrudePath = randomSpline;


        // star path
        Shape startShape = ShapeUtils.CreateStar();
        testGeometry0 = new ExtrudeGeometry( startShape , extrude0Settings );
		//testGeometry.computeVertexNormals();
		
		AddRenderObject(testGeometry0, materials[1], Vector3.zero);


		// star Extrude
		ExtrudeGeometry.Option extrude1Settings  = new THREE.ExtrudeGeometry.Option();
		extrude1Settings.amount = 20;
		extrude1Settings.steps = 2;
		extrude1Settings.bevelEnabled = true;
		extrude1Settings.bevelThickness = 2;
		extrude1Settings.bevelSize = 1;
		extrude1Settings.bevelSegments = 1;

		testGeometry = new ExtrudeGeometry( startShape, extrude1Settings );
		AddRenderObject(testGeometry, materials[2], new Vector3(50, 100, 50 ));

		// test
		Geometry startShapeGeo;
		startShapeGeo = new ShapeGeometry (startShape, op);
		AddRenderObject(startShapeGeo, materials[0], Vector3.zero);
	}

	Geometry AddRenderObject(Geometry geo, Material material, Vector3 position)
	{
		UnityEngine.Mesh mesh = geo.GetMesh();

		GameObject gObj = new GameObject();
		gObj.transform.SetParent(this.transform);
		gObj.transform.localPosition = position;
		
		MeshFilter mf = gObj.AddComponent<MeshFilter>();
		MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
		
		mf.mesh = mesh;
		mr.material = material;
		
		return geo;
	}

	void Update()
	{
//		ShapeAndHoleObject shapePoints = shape.extractPoints( options.curveSegments );
//		List<Vector3> vertices = shapePoints.shape;
//		Vector3 vert = vertices[contourIndex1];

		Curve spline = randomSpline;//closedSpline;

        //		TubeGeometry.FrenetFrames splineTube = extrudeSettings.frames;
        //
        //		int step = extrudeSettings.steps;
        //		List<Vector3> extrudePts = spline.getSpacedPoints( step );	

        //TubeGeometry.FrenetFrames splineTube = extrude0Settings.frames;
        FrenetFrames splineTube = testGeometry0.splineTube;
		
		int step = extrude0Settings.steps;
		List<Vector3> extrudePts = spline.getSpacedPoints( step );

		for(int i = 0; i < step; i++){
			Vector3 normal = splineTube.normals[i]; // spline上の法線
			normal.Normalize();

			//Vector3 vec = splineTube.vecs[i]; // tangent
			float u = (float)i / (step);
			Vector3 getTan = spline.getTangentAt(u);// tangent
			Vector3 tangent = splineTube.tangents[i];
			
			//float u = (float)i / (step - 1);
			//Vector3 t_vec = closedSpline.getPointAt(u);
			Vector3 t_vec = extrudePts[i];

			Debug.DrawRay(t_vec, normal * 10, Color.yellow);
			//Debug.DrawRay(t_vec, vec * 10, Color.green);
			Debug.DrawRay(t_vec, tangent * 30, Color.cyan);
			Debug.DrawRay(t_vec, getTan * 30, Color.magenta);
		}
	}

}
