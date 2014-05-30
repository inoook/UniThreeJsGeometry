using UnityEngine;
using System.Collections;

namespace THREE
{
	public class QuadraticBezierCurve : Curve
	{

		Vector3 v0;
		Vector3 v1;
		Vector3 v2;

		public QuadraticBezierCurve (Vector3 v0, Vector3 v1, Vector3 v2)
		{
			this.v0 = v0;
			this.v1 = v1;
			this.v2 = v2;
		}

		public override Vector3 getPoint (float t)
		{
		
			float tx, ty;
		
			tx = Shape.Utils.b2 (t, this.v0.x, this.v1.x, this.v2.x);
			ty = Shape.Utils.b2 (t, this.v0.y, this.v1.y, this.v2.y);
		
			return new Vector3 (tx, ty);	
		}
	
		public override Vector3 getTangent (float t)
		{
		
			float tx, ty;
		
			tx = Curve.Utils.tangentQuadraticBezier (t, this.v0.x, this.v1.x, this.v2.x);
			ty = Curve.Utils.tangentQuadraticBezier (t, this.v0.y, this.v1.y, this.v2.y);
		
			// returns unit vector
		
			Vector3 tangent = new Vector3 (tx, ty);
			tangent.Normalize ();
		
			return tangent;
		
		}
	}
}
