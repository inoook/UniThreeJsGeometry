using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// http://threejs.org/examples/#webgl_geometry_nurbs
public class Nurbs : ThreeBehaviour {

	public Material material;
	public Material wireMaterial;
	
	// Use this for initialization
	protected override void Init () {
		base.Init();

		// NURBS curve
		List<Vector4> nurbsControlPoints = new List<Vector4>();
		List<float> nurbsKnots = new List<float>();
		int nurbsDegree = 3;
		
		for ( int i = 0; i <= nurbsDegree; i ++ ) {
			nurbsKnots.Add( 0 );
		}
		
		for ( int i = 0, j = 20; i < j; i ++ ) {
			
			nurbsControlPoints.Add(
				new Vector4(
					Random.value * 400 - 200,
					Random.value * 400,
					Random.value * 400 - 200,
					1 // weight of control point: higher means stronger attraction
				)
			);
			
			float knot = (float)( i + 1 ) / ( j - nurbsDegree );
			nurbsKnots.Add( Mathf.Clamp( knot, 0, 1 ) );
			
		}

		THREE.NURBSCurve nurbsCurve = new THREE.NURBSCurve(nurbsDegree, nurbsKnots.ToArray(), nurbsControlPoints.ToArray());
		THREE.Geometry nurbsGeometry = new THREE.Geometry();
		List<Vector3> pts = nurbsCurve.getPoints(200);

		nurbsGeometry.vertices = pts;

		THREE.Line nurbsLine = new THREE.Line( nurbsGeometry, wireMaterial );
		nurbsLine.position = Vector3.zero;
		scene.Add( nurbsLine );
		
		// controlPoints
		THREE.Geometry nurbsControlPointsGeometry = new THREE.Geometry();
		nurbsControlPointsGeometry.vertices = new List<Vector3>( nurbsCurve.getVec3ControlPoints() );

		THREE.Line nurbsControlPointsLine = new THREE.Line( nurbsControlPointsGeometry, wireMaterial );
		nurbsControlPointsLine.position = nurbsLine.position;
		scene.Add( nurbsControlPointsLine );


		// NURBS surface
		
		Vector4[][] nsControlPoints = new Vector4[][]{
		new Vector4[]{
			new Vector4 ( -200, -200, 100, 1 ),
			new Vector4 ( -200, -100, -200, 1 ),
			new Vector4 ( -200, 100, 250, 1 ),
			new Vector4 ( -200, 200, -100, 1 )
		},
		new Vector4[]{
		 new Vector4 ( 0, -200, 0, 1 ),
		 new Vector4 ( 0, -100, -100, 5 ),
		 new Vector4 ( 0, 100, 150, 5 ),
		 new Vector4 ( 0, 200, 0, 1 )
			},
			new Vector4[]{
		 new Vector4 ( 200, -200, -100, 1 ),
		 new Vector4 ( 200, -100, 200, 1 ),
		 new Vector4 ( 200, 100, -250, 1 ),
		 new Vector4 ( 200, 200, 100, 1 )
			}
		};

		int degree1 = 2;
		int degree2 = 3;
		float[] knots1 = new float[]{ 0, 0, 0, 1, 1, 1 };
		float[] knots2 = new float[]{ 0, 0, 0, 0, 1, 1, 1, 1 };
		nurbsSurface = new THREE.NURBSSurface(degree1, degree2, knots1, knots2, nsControlPoints);
		
		THREE.ParametricGeometry geometry = new THREE.ParametricGeometry( getSurfacePoint, 20, 20 );
		THREE.Mesh mesh = new THREE.Mesh( geometry, material );
		mesh.position = new Vector3(0,0,0);
		scene.Add( mesh );


		t_nsControlPoints = nsControlPoints;

	}

	Vector4[][] t_nsControlPoints;
	void OnDrawGizmos()
	{
		if(Application.isPlaying){
		for(int i = 0; i < t_nsControlPoints.Length; i++){
			for(int n = 0;  n < t_nsControlPoints[i].Length; n++){
				Vector4 pt = t_nsControlPoints[i][n];
				Gizmos.DrawSphere(pt, 10);
			}
		}
		}

	}


	THREE.NURBSSurface nurbsSurface;

	Vector3 getSurfacePoint(float u, float v){
		return nurbsSurface.getPoint(u, v);
	}

	protected override void Render()
	{
		float timer = -Time.time * 1.0f;
		float camRotationSpeed = 0.15f;
		float x = Mathf.Cos( timer * camRotationSpeed ) * 800;
		float z = Mathf.Sin( timer * camRotationSpeed ) * 800;

		viewCamera.transform.position = new Vector3(x, 400, z);
		
		viewCamera.transform.LookAt( Vector3.zero );
//
//		float amp = 5.0f;
//		for ( int i = 0, l = scene.children.Count; i < l; i ++ ) {
//			
//			THREE.RenderableObject threeMesh = scene.children[ i ];
//
//			threeMesh.eulerAngles.x = timer * 5.0f * amp;
//			threeMesh.eulerAngles.y = timer * 2.5f * amp;
//
//		}
	}
}
