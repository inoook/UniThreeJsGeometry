// http://threejs.org/examples/webgl_geometry_extrude_shapes.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class ShapeTest : ThreeBehaviour
{
	public Material material;
	THREE.Geometry testGeometry;
	public List<int> result;
	
	// Use this for initialization
	protected override void Init ()
	{
		base.Init ();

		List<int> ary = new List<int> (new int[]{0,1,2,3,4,5,6});
		result = ary.GetRange (2, ary.Count - 2);

		// Triangle
		
		var triangleShape = new THREE.Shape();
		triangleShape.moveTo(  80, 20 );
		triangleShape.lineTo(  40, 80 );
		triangleShape.lineTo( 120, 80 );
		triangleShape.lineTo(  80, 20 ); // close path

		// Square
		
		float sqLength = 80;
		
		THREE.Shape squareShape = new THREE.Shape();
		squareShape.moveTo( 0,0 );
		squareShape.lineTo( 0, sqLength );
		squareShape.lineTo( sqLength, sqLength );
		squareShape.lineTo( sqLength, 0 );
		squareShape.lineTo( 0, 0 );
		
		
		// Rectangle
		
		float rectLength = 120, rectWidth = 40;
		
		THREE.Shape rectShape = new THREE.Shape();
		rectShape.moveTo( 0,0 );
		rectShape.lineTo( 0, rectWidth );
		rectShape.lineTo( rectLength, rectWidth );
		rectShape.lineTo( rectLength, 0 );
		rectShape.lineTo( 0, 0 );

		// Rounded rectangle
		
		var roundedRectShape = new THREE.Shape();

		float x = 0; float y = 0; float width = 50; float height = 50; float radius = 20;
		roundedRectShape.moveTo( x, y + radius );
		roundedRectShape.lineTo( x, y + height - radius );
		roundedRectShape.quadraticCurveTo( x, y + height, x + radius, y + height );
		roundedRectShape.lineTo( x + width - radius, y + height) ;
		roundedRectShape.quadraticCurveTo( x + width, y + height, x + width, y + height - radius );
		roundedRectShape.lineTo( x + width, y + radius );
		roundedRectShape.quadraticCurveTo( x + width, y, x + width - radius, y );
		roundedRectShape.lineTo( x + radius, y );
		roundedRectShape.quadraticCurveTo( x, y, x, y + radius );

		// Heart
		
		x = 0; y = 0;
		
		Shape heartShape = new Shape (); // From http://blog.burlock.org/html5/130-paths

		heartShape.moveTo (x + 25, y + 25);
		heartShape.bezierCurveTo (x + 25, y + 25, x + 20, y, x, y);
		heartShape.bezierCurveTo (x - 30, y, x - 30, y + 35, x - 30, y + 35);
		heartShape.bezierCurveTo (x - 30, y + 55, x - 10, y + 77, x + 25, y + 95);
		heartShape.bezierCurveTo (x + 60, y + 77, x + 80, y + 55, x + 80, y + 35);
		heartShape.bezierCurveTo (x + 80, y + 35, x + 80, y, x + 50, y);
		heartShape.bezierCurveTo (x + 35, y, x + 25, y + 25, x + 25, y + 25);


		// Circle
		
		float circleRadius = 40;
		var circleShape = new THREE.Shape();
		circleShape.moveTo( 0, circleRadius );
		circleShape.quadraticCurveTo( circleRadius, circleRadius, circleRadius, 0 );
		circleShape.quadraticCurveTo( circleRadius, -circleRadius, 0, -circleRadius );
		circleShape.quadraticCurveTo( -circleRadius, -circleRadius, -circleRadius, 0 );
		circleShape.quadraticCurveTo( -circleRadius, circleRadius, 0, circleRadius );

		// Fish
		
		x = y = 0;
		
		Shape fishShape = new Shape ();
		
		fishShape.moveTo (x, y);
		fishShape.quadraticCurveTo (x + 50, y - 80, x + 90, y - 10);
		fishShape.quadraticCurveTo (x + 100, y - 10, x + 115, y - 40);
		fishShape.quadraticCurveTo (x + 115, y, x + 115, y + 40);
		fishShape.quadraticCurveTo (x + 100, y + 10, x + 90, y + 10);
		fishShape.quadraticCurveTo (x + 50, y + 80, x, y);


		//
		// Arc circle

		Shape arcShape = new Shape ();
		arcShape.moveTo (50, 10);
		arcShape.absarc (10, 10, 40, 0, Mathf.PI * 2.0f, false);
		
		Path holePath = new Path ();
		holePath.moveTo (20, 10);
		holePath.absarc (10, 10, 10, 0, Mathf.PI * 2.0f, true);

		arcShape.holes.Add (holePath);


		// Smiley
		
		var smileyShape = new THREE.Shape();
		smileyShape.moveTo( 80, 40 );
		smileyShape.absarc( 40, 40, 40, 0, Mathf.PI*2, false );
		
		var smileyEye1Path = new THREE.Path();
		smileyEye1Path.moveTo( 35, 20 );
		// smileyEye1Path.absarc( 25, 20, 10, 0, Math.PI*2, true );
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
		extrudeSettings.curveSegments = 12;
		extrudeSettings.bevelEnabled = true;
		extrudeSettings.bevelSegments = 2;
		extrudeSettings.steps = 1;

		addShape( triangleShape, extrudeSettings, -180, 0, 0, 0, 0, 0, 1 );
		addShape( roundedRectShape, extrudeSettings, -150, 150, 0, 0, 0, 0, 1 );
		//addShape( rectShape, extrudeSettings, -150, 150, 0, 0, 0, 0, 1 );
		addShape( squareShape, extrudeSettings, 150, 100, 0, 0, 0, 0, 1 );
		addShape (heartShape, extrudeSettings, 60, 100, 0, 0, 0, Mathf.PI, 1);
		addShape( circleShape, extrudeSettings, 120, 250, 0, 0, 0, 0, 1 );
		addShape (fishShape, extrudeSettings, -60, 200, 0, 0, 0, 0, 1);
		addShape( smileyShape, extrudeSettings, -200, 250, 0, 0, 0, Mathf.PI, 1 );
		addShape (arcShape, extrudeSettings, 150, 0, 0, 0, 0, 0, 1);

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

		geometry = new THREE.ShapeGeometry (shape, op);
		//geometry = new THREE.ShapeGeometry (new List<Shape> (new Shape[]{shape}), op);
		threeMesh = new THREE.Mesh (geometry, material);
		threeMesh.position = new Vector3 (x, y, z - 125);
		threeMesh.eulerAngles = new Vector3 (rx, ry, rz) * Mathf.Rad2Deg;
		scene.Add (threeMesh);


		// 3d shape

		geometry = new THREE.ExtrudeGeometry (new List<Shape> (new Shape[]{shape}), extrudeSettings);
		threeMesh = new THREE.Mesh (geometry, material);
		threeMesh.position = new Vector3 (x, y, z);
		threeMesh.eulerAngles = new Vector3 (rx, ry, rz) * Mathf.Rad2Deg;
		scene.Add (threeMesh);
	}

	protected override void Render ()
	{

	}
}
