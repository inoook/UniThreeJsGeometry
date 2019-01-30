using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * @author renej
 * NURBS surface object
 *
 * Implementation is based on (x, y [, z=0 [, w=1]]) control points with w=weight.
 *
 **/
namespace THREE
{

	public class NURBSSurface {

		int degree1;
		int degree2;
		float[] knots1;
		float[] knots2;
		//List<List<Vector4>> controlPoints;
		Vector4[][] controlPoints;

		public NURBSSurface( int degree1, int degree2, float[] knots1, float[] knots2 /* arrays of reals */, Vector4[][] controlPoints /* array^2 of Vector(2|3|4) */) {
			
			this.degree1 = degree1;
			this.degree2 = degree2;
			this.knots1 = knots1;
			this.knots2 = knots2;
			
			int len1 = knots1.Length - degree1 - 1;
			int len2 = knots2.Length - degree2 - 1;
			
			this.controlPoints = new Vector4[len1][];
			
			// ensure Vector4 for control points
			for (int i = 0; i < len1; ++i) {
				//this.controlPoints[i] = new List<Vector4>();
				this.controlPoints[i] = new Vector4[len2];
				for (var j = 0; j < len2; ++j) {
					Vector4 point = controlPoints[i][j];
					//this.controlPoints[i].Add( new Vector4(point.x, point.y, point.z, point.w) );
					this.controlPoints[i][j] = new Vector4(point.x, point.y, point.z, point.w);
				}
			}
		}

		public Vector3 getPoint (float t1, float t2 ) {
			float u = (this.knots1[0] + t1 * (this.knots1[this.knots1.Length - 1] - this.knots1[0])); // linear mapping t1->u
			float v = (this.knots2[0] + t2 * (this.knots2[this.knots2.Length - 1] - this.knots2[0])); // linear mapping t2->u
			
			return THREE.NURBSUtils.calcSurfacePoint(this.degree1, this.degree2, this.knots1, this.knots2, this.controlPoints, u, v);
		}

	}

}