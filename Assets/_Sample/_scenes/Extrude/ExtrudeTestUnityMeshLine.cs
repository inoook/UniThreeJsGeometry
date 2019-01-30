// http://threejs.org/examples/webgl_geometry_extrude_shapes.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class ExtrudeTestUnityMeshLine : MonoBehaviour {

	public Material material;

	THREE.ExtrudeGeometry.Option extrudeSettings;
    THREE.SplineCurve3 randomSpline;
    THREE.Curve debug_line;

    List<Vector3> randomPoints;

    [SerializeField] AnimationCurve curveX;
    [SerializeField] AnimationCurve curveY;
    [SerializeField] AnimationCurve curveZ;

	// Use this for initialization
	void Start () {

		THREE.Geometry testGeometry;

		// random path
		randomPoints = new List<Vector3>();
        int count = 100;
        float delta = 1f / count;
        for ( int i = 0; i < count; i ++ ) {
			//randomPoints.Add( new Vector3( ( i - 4.5f ) * 50, Random.Range( - 50.0f, 50.0f ), Random.Range( - 50.0f, 50.0f ) ) );
			//randomPoints.Add( new Vector3( ( i ) * 50, Random.Range( - 50.0f, 50.0f ), Random.Range( - 50.0f, 50.0f ) ) );
			//randomPoints.Add( new Vector3( i * 50, i, i*2 ) );

            //randomPoints.Add(new Vector3((delta * i) * 500, 100 * curveY.Evaluate(delta * i), 100 * curveZ.Evaluate(delta * i)));
            randomPoints.Add(new Vector3( 500 * curveX.Evaluate(delta * i), 500 * curveY.Evaluate(delta * i), 100 * curveZ.Evaluate(delta * i) ));

		}

		randomSpline =  new SplineCurve3( randomPoints );
        SegmentLine segmentLine = new SegmentLine(randomPoints);
        LineCurve line = new LineCurve(Vector3.zero, Vector3.one * 15);

        debug_line = segmentLine;

        //
		extrudeSettings  = new THREE.ExtrudeGeometry.Option();
		
		extrudeSettings.bevelEnabled = false;

        //extrudeSettings.steps = 80;// 80
        //extrudeSettings.steps = 20;
        //extrudeSettings.extrudePath = randomSpline;

        extrudeSettings.steps = randomPoints.Count;
        extrudeSettings.extrudePath = segmentLine;
        //extrudeSettings.frames = new THREE.TubeGeometry.FrenetFrames(segmentLine, extrudeSettings.steps, false);

        //extrudeSettings.extrudePath = line;

		// star path
		//List<Vector2> pts0 = new List<Vector2>();
		//List<Vector2> normals0 = new List<Vector2>();
		//int numPts = 5;
        
		//for ( int i = 0; i < numPts * 2; i ++ ) {
		//	int l = i % 2 == 1 ? 10 : 20;
		//	float a = (float)i / numPts * Mathf.PI;
		//	pts0.Add( new Vector2 ( Mathf.Cos( a ) * l, Mathf.Sin( a ) * l ) );
		//}
		//for ( int i = 0; i < pts0.Count; i ++ ) {
		//	int endI = (i == pts0.Count-1) ? 0 : i+1;
		//	Vector2 vec = pts0[endI] - pts0[i];
		//	vec.Normalize();
		//	normals0.Add( new Vector2 ( vec.y, -vec.x ) );
		//}

        List<Vector2> pts0 = new List<Vector2>();
        List<Vector2> normals0 = new List<Vector2>();
        int numPts = 20;

        int l = 20;
        for (int i = 0; i < numPts; i++)
        {
            float a = (float)i / numPts * Mathf.PI*2;
            pts0.Add(new Vector2(Mathf.Cos(a) * l, Mathf.Sin(a) * l));
        }
        for (int i = 0; i < pts0.Count; i++)
        {
            int endI = (i == pts0.Count - 1) ? 0 : i + 1;
            Vector2 vec = pts0[endI] - pts0[i];
            vec.Normalize();
            normals0.Add(new Vector2(vec.y, -vec.x));
        }

		
        Shape shape = new Shape( pts0, normals0 );
		testGeometry = new THREE.ExtrudeGeometry( new List<Shape>(new Shape[]{ shape }), extrudeSettings );
		
		AddRenderObject(testGeometry, material, Vector3.zero);
	}

	THREE.Geometry AddRenderObject(THREE.Geometry geo, Material material, Vector3 position, float smooth = 0.0f) // 60
	{
		UnityEngine.Mesh mesh = geo.GetMesh(smooth);

		GameObject gObj = new GameObject();
		gObj.transform.SetParent(this.transform);
		gObj.transform.localPosition = position;
		
		MeshFilter mf = gObj.AddComponent<MeshFilter>();
		MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
		
		mf.mesh = mesh;
		mr.material = material;
		
		return geo;
	}

    [SerializeField] bool gizmo = false;

	void OnDrawGizmos()
	{
        if (!gizmo) { return; }

        if(debug_line == null){
            return;
        }

        Curve spline = debug_line;

		TubeGeometry.FrenetFrames splineTube = extrudeSettings.frames;
		
		int step = extrudeSettings.steps;
		List<Vector3> extrudePts = spline.getSpacedPoints( step );
        
		for(int i = 0; i < step; i++){
			Vector3 normal = splineTube.normals[i]; // spline上の法線
			normal.Normalize();

            Vector3 tangent = splineTube.tangents[i];
            Vector3 binormals = splineTube.binormals[i];
            Debug.Log(tangent);
			Vector3 t_vec = extrudePts[i];

			Debug.DrawRay(t_vec, normal * 10, Color.yellow);
            Debug.DrawRay(t_vec, tangent * 30, Color.cyan);
            Debug.DrawRay(t_vec, binormals * 30, Color.red);

            Gizmos.DrawWireSphere(t_vec, 10);
		}

        Gizmos.color = Color.red;
        for (int i = 0; i < randomPoints.Count; i++){
            Gizmos.DrawWireSphere(randomPoints[i], 5);

        }
	}

}
