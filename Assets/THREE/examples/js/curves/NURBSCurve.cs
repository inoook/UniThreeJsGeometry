using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/**
 * @author renej
 * NURBS curve object
 *
 * Derives from Curve, overriding getPoint and getTangent.
 *
 * Implementation is based on (x, y [, z=0 [, w=1]]) control points with w=weight.
 *
 **/
namespace THREE {

	public class NURBSCurve : Curve {

		int degree;
		float[] knots;
		Vector4[] controlPoints;

		public NURBSCurve(int degree, float[] knots /* array of reals */, Vector4[] controlPoints /* array of Vector(2|3|4) */ ){

			this.degree = degree;
			this.knots = knots;
			this.controlPoints = controlPoints;

			for (int i = 0; i < controlPoints.Length; ++i) { // ensure Vector4 for control points
				Vector4 point = controlPoints[i];
				this.controlPoints[i] = new Vector4(point.x, point.y, point.z, point.w);
			}
		}

		public override Vector3 getPoint (float t)
		{
			//Debug.Log("NURBSCurve getPoint: "+t);
			float u = this.knots[0] + t * (this.knots[this.knots.Length - 1] - this.knots[0]); // linear mapping t->u
			
			// following results in (wx, wy, wz, w) homogeneous point
			Vector4 hpoint = THREE.NURBSUtils.calcBSplinePoint(this.degree, this.knots, this.controlPoints, u);
			
			if (hpoint.w != 1.0f) { // project to 3D space: (wx, wy, wz, w) -> (x, y, z, 1)
				//hpoint.divideScalar(hpoint.w);
				hpoint /= hpoint.w;
			}
			
			return new Vector3(hpoint.x, hpoint.y, hpoint.z);
		}
		
		
		public override Vector3 getTangent (float t)
		{
			float u = this.knots[0] + t * (this.knots[this.knots.Length - 1] - this.knots[0]);
			Vector4[] ders = THREE.NURBSUtils.calcNURBSDerivatives(this.degree, this.knots, this.controlPoints, u, 1);
			
			Vector3 tangent = ders[1];
			tangent.Normalize();
			
			return tangent;
		}

		// add inok
		public Vector3[] getVec3ControlPoints(){
			Vector3[] points = new Vector3[controlPoints.Length];
			for(int i = 0; i < controlPoints.Length; i++){
				points[i] = (Vector3)(controlPoints[i]);
			}
			return points;
		}

	}
}
