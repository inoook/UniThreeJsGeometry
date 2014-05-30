using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
/**************************************************************
 *	Spline curve
 **************************************************************/
	public class SplineCurve : Curve
	{

		List<Vector3> points;

		public SplineCurve (List<Vector3> points)
		{
	
			this.points = (points == null) ? new List<Vector3> () : points;
		}

		public override Vector3 getPoint (float t)
		{
		
			Vector3 v = new Vector3 ();
			int[] c = new int[4];
			List<Vector3> points = this.points;

			int intPoint;
			float weight;
			float point = (float)(points.Count - 1) * t;
		
			intPoint = Mathf.FloorToInt (point);
			weight = point - intPoint;
		
			c [0] = intPoint == 0 ? intPoint : intPoint - 1;
			c [1] = intPoint;
			c [2] = intPoint > points.Count - 2 ? points.Count - 1 : intPoint + 1;
			c [3] = intPoint > points.Count - 3 ? points.Count - 1 : intPoint + 2;
		
			v.x = Curve.Utils.interpolate (points [c [0]].x, points [c [1]].x, points [c [2]].x, points [c [3]].x, weight);
			v.y = Curve.Utils.interpolate (points [c [0]].y, points [c [1]].y, points [c [2]].y, points [c [3]].y, weight);
		
			return v;
		
		}

	}
}
