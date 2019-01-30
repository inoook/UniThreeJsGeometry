// http://threejs.org/examples/webgl_geometry_extrude_shapes.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class ExtrudeTestUnityMeshAnim : MonoBehaviour {

	public Material material;

	THREE.ClosedSplineCurve3 closedSpline;

	THREE.ExtrudeGeometry.Option extrudeSettings;
	THREE.SplineCurve3 randomSpline;

	Shape startShape;

	MeshFilter meshFilter;
	THREE.ExtrudeGeometry extrudeGeo;

	// Use this for initialization
	void Start () {

		GameObject gObj = new GameObject();
		gObj.transform.SetParent(this.transform);
		
		MeshFilter mf = gObj.AddComponent<MeshFilter>();
		MeshRenderer mr = gObj.AddComponent<MeshRenderer>();

		mr.material = material;
		
		meshFilter = mf;

		extrudeGeo = new THREE.ExtrudeGeometry();
		extrudeSettings = new THREE.ExtrudeGeometry.Option();
	}

	public int numPts = 5;
	public int step = 100;
	public float speedAmp = 1.0f;

	void Update()
	{
		// star path
		List<Vector2> pts = new List<Vector2>();
		List<Vector2> normals = new List<Vector2>();
		
		for ( int i = 0; i < numPts * 2; i ++ ) {
			int l = i % 2 == 1 ? 10 : 20;
			float a = (float)i / numPts * Mathf.PI;
			pts.Add( new Vector2 ( Mathf.Cos( a ) * l, Mathf.Sin( a ) * l ) );
		}

		for ( int i = 0; i < pts.Count; i ++ ) {
			int endI = (i == pts.Count-1) ? 0 : i+1;
			Vector2 vec = pts[endI] - pts[i];
			vec.Normalize();
			normals.Add( new Vector2 ( vec.y, -vec.x ) );
		}

		startShape = new Shape( pts, normals );

		// random path
		List<Vector3> randomPoints = new List<Vector3>();
		float time = Time.time * speedAmp;
		for ( int i = 0; i < 20; i ++ ) {
			float t = 2.0f * i + time;
			randomPoints.Add( new Vector3( ( i ) * 50, 50 * Mathf.Sin(t), 0 ) );
			//randomPoints.Add( new Vector3( ( i ) * 50, 0, 0 ) );
		}

		randomSpline =  new SplineCurve3( randomPoints );
		List<Vector3> extrudePts = randomSpline.getSpacedPoints( step );
		for(int i = 0; i < extrudePts.Count; i++){
			Vector3 pt = extrudePts[i];
			Debug.DrawRay(pt, Vector3.one* 2.0f, Color.yellow);
		}

//		extrudeSettings = new THREE.ExtrudeGeometry.Option();
		extrudeSettings.steps = step;
		extrudeSettings.bevelEnabled = false;
		extrudeSettings.extrudePath = randomSpline;
		extrudeSettings.frames = new THREE.TubeGeometry.FrenetFrames(randomSpline, step, false);

        //THREE.Geometry extrudeGeo = new THREE.ExtrudeGeometry( new List<Shape>(new Shape[]{ startShape }), extrudeSettings );
        // update shape
        ShapeAndHoleObject shapeAndHole = ExtrudeGeometry.GetShapeAndHoleObject(startShape, extrudeSettings);
		extrudeGeo.UpdateShape(shapeAndHole, extrudeSettings);

		UnityEngine.Mesh mesh = extrudeGeo.CreateAndGetMesh();
		meshFilter.mesh = mesh;
	}

}
