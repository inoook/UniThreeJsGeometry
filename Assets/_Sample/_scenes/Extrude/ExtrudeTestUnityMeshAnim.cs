﻿// http://threejs.org/examples/webgl_geometry_extrude_shapes.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class ExtrudeTestUnityMeshAnim : MonoBehaviour {

	public Material material;

	THREE.ClosedSplineCurve3 closedSpline;
	THREE.ExtrudeGeometry.Option extrudeSettings;
	
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
        //Shape startShape = ShapeUtils.CreateStar(10, 20, 6);
        Shape startShape = ShapeUtils.CreateStar(10, 20, 6);

		// random path
		List<Vector3> randomPoints = new List<Vector3>();
		float time = Time.time * speedAmp;
		for ( int i = 0; i < 20; i ++ ) {
			float t = 2.0f * i + time;
			randomPoints.Add( new Vector3( ( i ) * 50, 50 * Mathf.Sin(t), 0 ) );
			//randomPoints.Add( new Vector3( ( i ) * 50, 0, 0 ) );
		}

        SplineCurve3 randomSpline =  new SplineCurve3( randomPoints );
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
        extrudeSettings.uvGenerator = new ExtrudeGeometry.UVGenerator(40, 40);

        //THREE.Geometry extrudeGeo = new THREE.ExtrudeGeometry( new List<Shape>(new Shape[]{ startShape }), extrudeSettings );
        // update shape
        extrudeGeo.UpdateShape(startShape, extrudeSettings);

		UnityEngine.Mesh mesh = extrudeGeo.CreateAndGetMesh();
		meshFilter.mesh = mesh;

        //Debug.LogError("XXX");
	}

}
