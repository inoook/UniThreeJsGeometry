using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class ShapePathTest : MonoBehaviour
{

	Curve spline;
	//SplineCurve3 spline;
	List<Vector3> randomPoints;

	// Use this for initialization
	void Start()
	{
//		spline = new THREE.Path ();
//		
//		spline.moveTo (20, 40);
//		spline.quadraticCurveTo (40, 60, 60, 40);
//		spline.bezierCurveTo (70, 45, 70, 50, 60, 60);
//		spline.quadraticCurveTo (40, 80, 20, 60);
//		spline.quadraticCurveTo (5, 50, 20, 40);



		randomPoints = new List<Vector3>();
		for (int i = 0; i < 10; i ++) {
			//randomPoints.Add (new Vector3 ((i - 4.5f) * 50, Random.Range (- 50.0f, 50.0f), Random.Range (- 50.0f, 50.0f)));
			randomPoints.Add(new Vector3(Random.Range(- 50.0f, 50.0f), Random.Range(- 50.0f, 50.0f), Random.Range(- 50.0f, 50.0f)) * 0.4f);
		}
		spline = new THREE.SplineCurve3(randomPoints);

	}

	public int num = 10;
	public bool useGetPointAt = false;
	// Update is called once per frame
	void OnDrawGizmos()
	{
		if (randomPoints != null) {
			Gizmos.color = Color.red;
			for (int i = 0; i < randomPoints.Count; i++) {
				Gizmos.DrawWireSphere(randomPoints[i], 0.5f);
			}
		}

		if (spline != null) {
			Gizmos.color = Color.white;
			for (int i = 0; i < num; i++) {
				float v = (float)i / num;
				Vector3 pt;
				if (useGetPointAt) {
					pt = spline.getPointAt(v);
				} else {
					pt = spline.getPoint(v);
				}
				Gizmos.DrawWireSphere(pt, 0.25f);
			}
		}
	}
}