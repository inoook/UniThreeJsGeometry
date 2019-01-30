using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class ShapePath2 : MonoBehaviour
{

	CurvePath curve;
	Curve spline;

	List<Vector3> randomPoints;
	
	ShapePath shapePath;

	// Use this for initialization
	void Start()
	{
		
		randomPoints = new List<Vector3>();
		for (int i = 0; i < 5; i ++) {
			randomPoints.Add(new Vector3((i) * 10, Random.Range(- 50.0f, 50.0f), Random.Range(- 50.0f, 50.0f)));
			//randomPoints.Add(new Vector3(Random.Range(- 50.0f, 50.0f), Random.Range(- 50.0f, 50.0f), Random.Range(- 50.0f, 50.0f)) * 0.4f);
		}

        // ----------
		// BezierCurve3 - white
		curve = new THREE.CurvePath();

		//Vector3 c = (randomPoints[1] - randomPoints[2]) * -1 + randomPoints[2];
		//for (int i = 2; i < randomPoints.Count-1; i++) {
		//	c = (c - randomPoints[i]) * -1 + randomPoints[i];
		//	THREE.QuadraticBezierCurve3 curveSegment1 = new THREE.QuadraticBezierCurve3(randomPoints[i], c, randomPoints[i + 1]);
		//	curve.add(curveSegment1);
		//}

		Vector3 centerPosPre;
		Vector3 centerPosNext = Vector3.Lerp(randomPoints[1], randomPoints[2], 0.5f);

		THREE.QuadraticBezierCurve3 curveSegment = new THREE.QuadraticBezierCurve3(randomPoints[0], randomPoints[1], centerPosNext);
		curve.add(curveSegment);

		for (int i = 2; i < randomPoints.Count-1; i++) {
			centerPosPre = Vector3.Lerp(randomPoints[i - 1], randomPoints[i], 0.5f);
			centerPosNext = Vector3.Lerp(randomPoints[i], randomPoints[i + 1], 0.5f);
			THREE.QuadraticBezierCurve3 curveSegment1 = new THREE.QuadraticBezierCurve3(centerPosPre, randomPoints[i], centerPosNext);
			curve.add(curveSegment1);
		}

        // ----------
        // Spline3 - green // randomPoints を通る線を作成
        spline = new THREE.SplineCurve3(randomPoints);

        // ----------
        // ShapePath - blue
        PointList pointList = new PointList();
		shapePath = pointList.GetShapePath(randomPoints.ToArray());
	}

	public int num = 10;
	public bool useGetPointAt = false;
	// Update is called once per frame
	void OnDrawGizmos()
	{
        // point
		if (randomPoints != null) {
			Gizmos.color = Color.cyan;
			for (int i = 0; i < randomPoints.Count; i++) {
				Gizmos.DrawSphere(randomPoints[i], 1.5f);
			}
            // 2点の中間点
			Gizmos.color = Color.magenta;
			for (int i = 0; i < randomPoints.Count-1; i++) {
				Gizmos.DrawWireSphere(Vector3.Lerp(randomPoints[i + 1], randomPoints[i], 0.5f), 0.75f);
			}
		}

        // line
        // ----
		if (curve != null) {
			Gizmos.color = Color.white;
			for (int i = 0; i < num; i++) {
				float v = (float)i / num;
				Vector3 pt;
				if (useGetPointAt) {
					pt = curve.getPointAt(v);
				} else {
					pt = curve.getPoint(v);
				}
				Gizmos.DrawWireSphere(pt, 0.25f);
			}
		}

        // ----
        if (spline != null) {
			Gizmos.color = Color.green;
			for (int i = 0; i < num; i++) {
				float v = (float)i / num;
				Vector3 pt;
				if (useGetPointAt) {
					pt = spline.getPointAt(v); // 等間隔のポイントを取得
				} else {
					pt = spline.getPoint(v);
				}
				Gizmos.DrawWireSphere(pt, 0.25f);
			}
		}

        // ----
        if (shapePath != null) {
			Gizmos.color = Color.blue;
			for (int i = 0; i < num; i++) {
				float v = (float)i / num;
				Vector3 pt;
				pt = shapePath.getPointInfoVec3Percent(v);
				Gizmos.DrawWireSphere(pt, 0.25f);
			}
		}
	}
}