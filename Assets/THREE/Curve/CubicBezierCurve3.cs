using UnityEngine;
using System.Collections;

namespace THREE
{
	public class CubicBezierCurve3 : Curve
	{

		Vector3 v0;
		Vector3 v1;
		Vector3 v2;
		Vector3 v3;

		public CubicBezierCurve3 (Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
		{
			this.v0 = v0;
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		public override Vector3 getPoint (float t)
		{
		
			float tx, ty, tz;
		
			tx = Shape.UtilsShape.b3 (t, this.v0.x, this.v1.x, this.v2.x, this.v3.x);
			ty = Shape.UtilsShape.b3 (t, this.v0.y, this.v1.y, this.v2.y, this.v3.y);
			tz = Shape.UtilsShape.b3 (t, this.v0.z, this.v1.z, this.v2.z, this.v3.z);
		
			return new Vector3 (tx, ty, tz);
		
		}
	}
}
