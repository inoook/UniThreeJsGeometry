using UnityEngine;
using System.Collections;

public class PointList
{
	public Transform[] points;

	
	public Vector3[] GetPathPoints()
	{
		Vector3[] pts = new Vector3[points.Length];
		for (int i = 0; i < points.Length; i++) {
			pts[i] = points[i].position;
		}
		return pts;
	}
	
	public ShapePath GetShapePath(Vector3[] path = null)
	{
		if (path == null) {
			path = GetPathPoints();
		}
		ShapePath sp = new ShapePath();
		sp.begin();
		
		sp.moveTo(path[0]);
		for (int n = 1; n < path.Length-1; n++) {
			Vector3 pos = Vector3.Lerp(path[n], path[n + 1], 0.5f);
			sp.curveTo(path[n], pos);
		}
		
		Vector3 posC = Vector3.Lerp(path[path.Length - 2], path[path.Length - 1], 0.5f);
		Vector3 posEnd = path[path.Length - 1];
		sp.curveTo(posC, posEnd);
		
		sp.end();
		
		return sp;
	}
	
}
