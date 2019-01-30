using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// http://threejs.org/examples/#webgl_geometry_nurbs
public class NurbsUnity : MonoBehaviour
{

	public Material material;
	public Material wireMaterial;
	
	// Use this for initialization
	void Start()
	{
		// NURBS curve
		List<Vector4> nurbsControlPoints = new List<Vector4>();
		List<float> nurbsKnots = new List<float>();
		int nurbsDegree = 3;
		
		for (int i = 0; i <= nurbsDegree; i ++) {
			nurbsKnots.Add(0);
		}
		
		for (int i = 0, j = 20; i < j; i ++) {
			
			nurbsControlPoints.Add(
				new Vector4(
					Random.value * 400 - 200,
					Random.value * 400,
					Random.value * 400 - 200,
					1 // weight of control point: higher means stronger attraction
			)
			);
			
			float knot = (float)(i + 1) / (j - nurbsDegree);
			nurbsKnots.Add(Mathf.Clamp(knot, 0, 1));
			
		}

		THREE.NURBSCurve nurbsCurve = new THREE.NURBSCurve(nurbsDegree, nurbsKnots.ToArray(), nurbsControlPoints.ToArray());
		THREE.Geometry nurbsGeometry = new THREE.Geometry();
		List<Vector3> pts = nurbsCurve.getPoints(200);

		nurbsGeometry.vertices = pts;

		AddRenderLineObject(nurbsGeometry, wireMaterial, Vector3.zero);
		
		// controlPoints
		THREE.Geometry nurbsControlPointsGeometry = new THREE.Geometry();
		nurbsControlPointsGeometry.vertices = new List<Vector3>(nurbsCurve.getVec3ControlPoints());
		AddRenderLineObject(nurbsControlPointsGeometry, wireMaterial, Vector3.zero);


		// NURBS surface
		
		Vector4[][] nsControlPoints = new Vector4[][]{
		new Vector4[]{
			new Vector4(-200, -200, 100, 1),
			new Vector4(-200, -100, -200, 1),
			new Vector4(-200, 100, 250, 1),
			new Vector4(-200, 200, -100, 1)
		},
		new Vector4[]{
		 new Vector4(0, -200, 0, 1),
		 new Vector4(0, -100, -100, 5),
		 new Vector4(0, 100, 150, 5),
		 new Vector4(0, 200, 0, 1)
			},
			new Vector4[]{
		 new Vector4(200, -200, -100, 1),
		 new Vector4(200, -100, 200, 1),
		 new Vector4(200, 100, -250, 1),
		 new Vector4(200, 200, 100, 1)
			}
		};

		int degree1 = 2;
		int degree2 = 3;
		float[] knots1 = new float[]{ 0, 0, 0, 1, 1, 1 };
		float[] knots2 = new float[]{ 0, 0, 0, 0, 1, 1, 1, 1 };
		nurbsSurface = new THREE.NURBSSurface(degree1, degree2, knots1, knots2, nsControlPoints);
		
		THREE.ParametricGeometry geometry = new THREE.ParametricGeometry(getSurfacePoint, 20, 20);
		AddRenderObject(geometry, material, Vector3.zero);
		
		t_nsControlPoints = nsControlPoints;

	}

	Vector4[][] t_nsControlPoints;
	void OnDrawGizmos()
	{
		if (Application.isPlaying) {
			for (int i = 0; i < t_nsControlPoints.Length; i++) {
				for (int n = 0; n < t_nsControlPoints[i].Length; n++) {
					Vector4 pt = t_nsControlPoints [i] [n];
					Gizmos.DrawSphere(pt * 0.01f, 10 * 0.01f);
				}
			}
		}
	}


	THREE.NURBSSurface nurbsSurface;

	Vector3 getSurfacePoint(float u, float v)
	{
		return nurbsSurface.getPoint(u, v);
	}

	THREE.Geometry AddRenderObject(THREE.Geometry geo, Material material, Vector3 position)
	{
		UnityEngine.Mesh mesh = geo.GetMesh();
		
		GameObject gObj = new GameObject();
		gObj.transform.SetParent(this.transform);
		gObj.transform.localPosition = position * 0.01f;
		gObj.transform.localScale = Vector3.one * 0.01f;
		
		MeshFilter mf = gObj.AddComponent<MeshFilter>();
		MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
		
		mf.mesh = mesh;
		mr.material = material;
		
		return geo;
	}
	THREE.Geometry AddRenderLineObject(THREE.Geometry geo, Material material, Vector3 position)
	{
		UnityEngine.Mesh mesh = geo.GetLineMesh();
		
		GameObject gObj = new GameObject();
		gObj.transform.SetParent(this.transform);
		gObj.transform.localPosition = position * 0.01f;
		gObj.transform.localScale = Vector3.one * 0.01f;
		
		MeshFilter mf = gObj.AddComponent<MeshFilter>();
		MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
		
		mf.mesh = mesh;
		mr.material = material;
		
		return geo;
	}
}
