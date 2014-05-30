// http://threejs.org/examples/webgl_geometry_extrude_shapes.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class ShapeTest2 : ThreeBehaviour
{
	public Material material;
	THREE.Geometry testGeometry;
	
	// Use this for initialization
	protected override void Init ()
	{
		base.Init ();

		// Smiley
		
		var smileyShape = new THREE.Shape()	;
		smileyShape.moveTo( 80, 40 );
		smileyShape.absarc( 40, 40, 40, 0, Mathf.PI*2, false );
		
		var smileyEye1Path = new THREE.Path();
		smileyEye1Path.moveTo( 35, 20 );
		// smileyEye1Path.absarc( 25, 20, 10, 0, Mathf.PI*2, true );
		smileyEye1Path.absellipse( 25, 20, 10, 10, 0, Mathf.PI*2, true );
		
		smileyShape.holes.Add( smileyEye1Path );
		
		var smileyEye2Path = new THREE.Path();
		smileyEye2Path.moveTo( 65, 20 );
		smileyEye2Path.absarc( 55, 20, 10, 0, Mathf.PI*2, true );

		smileyShape.holes.Add( smileyEye2Path );
		
		var smileyMouthPath = new THREE.Path();
		// ugly box mouth
//		smileyMouthPath.moveTo( 20, 40 );
//		smileyMouthPath.lineTo( 60, 40 );
//		smileyMouthPath.lineTo( 60, 60 );
//		smileyMouthPath.lineTo( 20, 60 );
//		smileyMouthPath.lineTo( 20, 40 );
		
		smileyMouthPath.moveTo( 20, 40 );
		smileyMouthPath.quadraticCurveTo( 40, 60, 60, 40 );
		smileyMouthPath.bezierCurveTo( 70, 45, 70, 50, 60, 60 );
		smileyMouthPath.quadraticCurveTo( 40, 80, 20, 60 );
		smileyMouthPath.quadraticCurveTo( 5, 50, 20, 40 );

		
		smileyShape.holes.Add( smileyMouthPath );


//		Shape arcShape = new Shape();
//		arcShape.moveTo( 20, 10 );
//		arcShape.absarc( 10, 10, 10, 0, Mathf.PI*2, false );

		THREE.ExtrudeGeometry.Option extrudeSettings = new THREE.ExtrudeGeometry.Option ();
		//extrudeSettings.steps = 100;
		//extrudeSettings.bevelEnabled = false;
		extrudeSettings.amount = 20;
		extrudeSettings.curveSegments = 3;
		extrudeSettings.bevelEnabled = true;
		extrudeSettings.bevelSegments = 2;
		extrudeSettings.steps = 1;

		addShape( smileyShape, extrudeSettings, -200, 250, 0, 0, 0, Mathf.PI, 1 );

	}

	void addShape (Shape shape, THREE.ExtrudeGeometry.Option extrudeSettings, float x, float y, float z, float rx, float ry, float rz, float s)
	{
		
		//THREE.Geometry points = shape.createPointsGeometry ();
		//THREE.Geometry spacedPoints = shape.createSpacedPointsGeometry (100);
		Debug.LogWarning("TODO: CHECK use shape.createSpacedPointsGeometry");
		shape.createSpacedPointsGeometry (100);

		// flat shape

		THREE.ShapeGeometry.Option op = new THREE.ShapeGeometry.Option ();
		op.curveSegments = 12;

		THREE.Geometry geometry;
		THREE.Mesh threeMesh;

		geometry = new THREE.ShapeGeometry (new List<Shape> (new Shape[]{shape}), op);
		threeMesh = new THREE.Mesh (geometry, material);
		threeMesh.position = new Vector3 (x, y, z - 125);
		threeMesh.eulerAngles = new Vector3 (rx, ry, rz) * Mathf.Rad2Deg;
		scene.Add (threeMesh);


//		// 3d shape
//
//		geometry = new THREE.ExtrudeGeometry (new List<Shape> (new Shape[]{shape}), extrudeSettings);
//		threeMesh = new THREE.Mesh (geometry, material);
//		threeMesh.position = new Vector3 (x, y, z);
//		threeMesh.eulerAngles = new Vector3 (rx, ry, rz) * Mathf.Rad2Deg;
//		scene.Add (threeMesh);
	}

	protected override void Render ()
	{

	}
}
