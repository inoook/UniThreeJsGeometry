// https://github.com/mrdoob/three.js/blob/master/examples/js/CurveExtras.js

/*
 * A bunch of parametric curves
 * @author zz85
 *
 * Formulas collected from various sources
 *	http://mathworld.wolfram.com/HeartCurve.html
 *	http://mathdl.maa.org/images/upload_library/23/stemkoski/knots/page6.html
 *	http://en.wikipedia.org/wiki/Viviani%27s_curve
 *	http://mathdl.maa.org/images/upload_library/23/stemkoski/knots/page4.html
 *	http://www.mi.sanu.ac.rs/vismath/taylorapril2011/Taylor.pdf
 *	http://prideout.net/blog/?p=44
 */


using UnityEngine;
using System.Collections;

namespace THREE.Curves
{
	//public class GrannyKnot : Curve {
	public class GrannyKnot : Path {

		public override Vector3 getPoint(float t ) {
			t = 2 * Mathf.PI * t;
			
			float x = -0.22f * Mathf.Cos(t) - 1.28f * Mathf.Sin(t) - 0.44f * Mathf.Cos(3 * t) - 0.78f * Mathf.Sin(3 * t);
			float y = -0.1f * Mathf.Cos(2 * t) - 0.27f * Mathf.Sin(2 * t) + 0.38f * Mathf.Cos(4 * t) + 0.46f * Mathf.Sin(4 * t);
			float z = 0.7f * Mathf.Cos(3 * t) - 0.4f * Mathf.Sin(3 * t);
			return new Vector3(x, y, z) * 20;
		}

	}
}
