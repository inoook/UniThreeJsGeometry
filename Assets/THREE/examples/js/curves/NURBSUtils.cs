using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * @author renej
 * NURBS utils
 *
 * See NURBSCurve and NURBSSurface.
 *
 **/

/**************************************************************
 *	NURBS Utils
 **************************************************************/
namespace THREE{
	public class NURBSUtils {

		public static int findSpan(int p, float u, float[] U ) {
			int n = U.Length - p - 1;
			
			if (u >= U[n]) {
				return n - 1;
			}
			
			if (u <= U[p]) {
				return p;
			}
			
			int low = p;
			int high = n;
			int mid = Mathf.FloorToInt((low + high) / 2);
			
			while (u < U[mid] || u >= U[mid + 1]) {
				
				if (u < U[mid]) {
					high = mid;
				} else {
					low = mid;
				}
				
				mid = Mathf.FloorToInt((low + high) / 2);
			}
			
			return mid;
		}

		/*
		Calculate basis functions. See The NURBS Book, page 70, algorithm A2.2
	   
		span : span in which u lies
		u    : parametric point
		p    : degree
		U    : knot vector
		
		returns array[p+1] with basis functions values.
		*/
		public static float[] calcBasisFunctions (int span, float u, int p, float[] U ) {
			float[] N = new float[p+1];
			float[] left = new float[p+1];
			float[] right = new float[p+1];
			N[0] = 1.0f;

			for (int j = 1; j <= p; ++j) {
				
	//			left[j] = u - U[span + 1 - j];
	//			right[j] = U[span + j] - u;

				left[j] = ( u - U[span + 1 - j] );
				right[j] = ( U[span + j] - u );
				
				float saved = 0.0f;
				
				for (int r = 0; r < j; ++r) {
					
					float rv = right[r + 1];
					float lv = left[j - r];
					float temp = N[r] / (rv + lv);
					N[r] = saved + rv * temp;
					saved = lv * temp;
				}
				
				N[j] = saved;
			}
			
			return N;
		}

		/*
		Calculate B-Spline curve points. See The NURBS Book, page 82, algorithm A3.1.
	 
		p : degree of B-Spline
		U : knot vector
		P : control points (x, y, z, w)
		u : parametric point

		returns point for given u
		*/ 
		public static Vector4 calcBSplinePoint(int p, float[] U, Vector4[] P, float u ) {
			int span = findSpan(p, u, U);
			float[] N = calcBasisFunctions(span, u, p, U);
			Vector4 C = new Vector4(0, 0, 0, 0);
			
			for (int j = 0; j <= p; ++j) {
				Vector4 point = P[span - p + j];
				float Nj = N[j];
				float wNj = point.w * Nj;
				C.x += point.x * wNj;
				C.y += point.y * wNj;
				C.z += point.z * wNj;
				C.w += point.w * Nj;
			}
			
			return C;
		}

		/*
		Calculate basis functions derivatives. See The NURBS Book, page 72, algorithm A2.3.

		span : span in which u lies
		u    : parametric point
		p    : degree
		n    : number of derivatives to calculate
		U    : knot vector

		returns array[n+1][p+1] with basis functions derivatives
		*/
		public static List<List<float>> calcBasisFunctionDerivatives (int span, float u, int p,  int n, float[] U ) {
			
			List<float> zeroArr = new List<float>();
			for (int i = 0; i <= p; ++i){
				//zeroArr[i] = 0.0f;
				zeroArr.Add( 0.0f );
			}
			
			List<List<float>> ders = new List<List<float>>();
			for (int i = 0; i <= n; ++i){
				//ders[i] = zeroArr.slice(0f);
				ders.Add ( zeroArr );
			}
			
			List<List<float>> ndu = new List<List<float>>();
			for (int i = 0; i <= p; ++i){
				//ndu[i] = zeroArr.slice(0);
				ndu.Add ( zeroArr );
			}
			
			ndu[0][0] = 1.0f;
			
//			var left = zeroArr.slice(0);
//			var right = zeroArr.slice(0);
			List<float> left = new List<float>();
			left.AddRange( zeroArr );
			List<float> right = new List<float>();
			right.AddRange( zeroArr );
			
			for (int j = 1; j <= p; ++j) {
				left[j] = u - U[span + 1 - j];
				right[j] = U[span + j] - u;
				
				float saved = 0.0f;
				
				for (int r = 0; r < j; ++r) {
					float rv = right[r + 1];
					float lv = left[j - r];
					ndu[j][r] = rv + lv;
					
					float temp = ndu[r][j - 1] / ndu[j][r];
					ndu[r][j] = saved + rv * temp;
					saved = lv * temp;
				}
				
				ndu[j][j] = saved;
			}
			
			for (int j = 0; j <= p; ++j) {
				ders[0][j] = ndu[j][p];
			}
			
			for (int r = 0; r <= p; ++r) {
				int s1 = 0;
				int s2 = 1;
				
				List<List<float>> a = new List<List<float>>();
				for (int i = 0; i <= p; ++i) {
					//a[i] = zeroArr.slice(0);
					a.Add( zeroArr );
				}
				a[0][0] = 1.0f;
				
				for (int k = 1; k <= n; ++k) {
					float d = 0.0f;
					var rk = r - k;
					var pk = p - k;
					
					if (r >= k) {
						a[s2][0] = a[s1][0] / ndu[pk + 1][rk];
						d = a[s2][0] * ndu[rk][pk];
					}
					
					var j1 = (rk >= -1) ? 1 : -rk;
					var j2 = (r - 1 <= pk) ? k - 1 :  p - r;
					
					for (int j = j1; j <= j2; ++j) {
						a[s2][j] = (a[s1][j] - a[s1][j - 1]) / ndu[pk + 1][rk + j];
						d += a[s2][j] * ndu[rk + j][pk];
					}
					
					if (r <= pk) {
						a[s2][k] = -a[s1][k - 1] / ndu[pk + 1][r];
						d += a[s2][k] * ndu[r][pk];
					}
					
					ders[k][r] = d;
					
					//int j = s1;
					int _j = s1;
					s1 = s2;
					s2 = _j;
				}
			}
			
			//int r = p;
			int _r = p;
			
			for (int k = 1; k <= n; ++k) {
				for (var j = 0; j <= p; ++j) {
					ders[k][j] *= _r;
				}
				_r *= p - k;
			}
			
			return ders;
		}

		/*
		Calculate derivatives of a B-Spline. See The NURBS Book, page 93, algorithm A3.2.

		p  : degree
		U  : knot vector
		P  : control points
		u  : Parametric points
		nd : number of derivatives

		returns array[d+1] with derivatives
		*/
		public static Vector4[] calcBSplineDerivatives (int p, float[] U, Vector4[] P, float u, int nd ) {
			var du = nd < p ? nd : p;
			List<Vector4> CK = new List<Vector4>();
			int span = findSpan(p, u, U);
			var nders = calcBasisFunctionDerivatives(span, u, p, du, U);
			List<Vector3> Pw = new List<Vector3>();
			
			for (int i = 0; i < P.Length; ++i) {
				//Vector3 point = P[i].clone();
				Vector4 point = P[i];
				float w = point.w;
				
				point.x *= w;
				point.y *= w;
				point.z *= w;
				
				Pw[i] = point;
			}
			for (int k = 0; k <= du; ++k) {
				//Vector3 point = Pw[span - p].clone().multiplyScalar(nders[k][0]);
				Vector3 point = Pw[span - p] * (nders[k][0]);
				
				for (int j = 1; j <= p; ++j) {
					//point.add(Pw[span - p + j].clone().multiplyScalar(nders[k][j]));
					point = point + (Pw[span - p + j] * (nders[k][j]));
				}
				
				CK[k] = point;
			}
			
			for (var k = du + 1; k <= nd + 1; ++k) {
				CK[k] = new Vector4(0, 0, 0);
			}
			
			return CK.ToArray();
		}

		/*
		Calculate "K over I"

		returns k!/(i!(k-i)!)
		*/
		public static float calcKoverI(int k, int i ) {
			float nom = 1.0f;
			
			for (int j = 2; j <= k; ++j) {
				nom *= j;
			}
			
			int denom = 1;
			
			for (int j = 2; j <= i; ++j) {
				denom *= j;
			}
			
			for (int j = 2; j <= k - i; ++j) {
				denom *= j;
			}
			
			return nom / denom;
		}

		/*
		Calculate derivatives (0-nd) of rational curve. See The NURBS Book, page 127, algorithm A4.2.

		Pders : result of function calcBSplineDerivatives

		returns array with derivatives for rational curve.
		*/
		public static Vector4[] calcRationalCurveDerivatives (Vector4[] Pders ) {
			int nd = Pders.Length;
			Vector4[] Aders = new Vector4[nd];
			float[] wders = new float[nd];
			
			for (var i = 0; i < nd; ++i) {
				Vector4 point = Pders[i];
				Aders[i] = new Vector3(point.x, point.y, point.z);
				wders[i] = point.w;
			}
			
			Vector4[] CK = new Vector4[nd];
			
			for (int k = 0; k < nd; ++k) {
				//Vector3 v = Aders[k].clone();
				Vector4 v = Aders[k];
				
				for (int i = 1; i <= k; ++i) {
					//v.sub(CK[k - i].clone().multiplyScalar(this.calcKoverI(k,i) * wders[i]));
					v = v - (CK[k - i] * (calcKoverI(k,i) * wders[i]));
				}
				
				//CK[k] = v.divideScalar(wders[0]);
				CK[k] = v / (wders[0]);
			}
			
			return CK;
		}

		/*
		Calculate NURBS curve derivatives. See The NURBS Book, page 127, algorithm A4.2.

		p  : degree
		U  : knot vector
		P  : control points in homogeneous space
		u  : parametric points
		nd : number of derivatives

		returns array with derivatives.
		*/
		public static Vector4[] calcNURBSDerivatives (int p, float[] U, Vector4[] P,  float u, int nd ) {
			Vector4[] Pders = calcBSplineDerivatives(p, U, P, u, nd);
			return calcRationalCurveDerivatives(Pders);
		}

		/*
		Calculate rational B-Spline surface point. See The NURBS Book, page 134, algorithm A4.3.
	 
		p1, p2 : degrees of B-Spline surface
		U1, U2 : knot vectors
		P      : control points (x, y, z, w)
		u, v   : parametric values

		returns point for given (u, v)
		*/
		public static Vector3 calcSurfacePoint (int p, int q, float[] U, float[] V, Vector4[][] P, float u, float v ) {
			int uspan = findSpan(p, u, U);
			int vspan = findSpan(q, v, V);
			float[] Nu = calcBasisFunctions(uspan, u, p, U);
			float[] Nv = calcBasisFunctions(vspan, v, q, V);
			Vector4[] temp = new Vector4[q+1];
			
			for (int l = 0; l <= q; ++l) {
				temp[l] = new Vector4(0, 0, 0, 0);
				for (var k = 0; k <= p; ++k) {
					//Vector4 point = P[uspan - p + k][vspan - q + l].clone();
					Vector4 point = P[uspan - p + k][vspan - q + l];
					float w = point.w;
					point.x *= w;
					point.y *= w;
					point.z *= w;
					//temp[l].add(point.multiplyScalar(Nu[k]));
					temp[l] = temp[l] + (point * (Nu[k]));
				}
			}
			
			Vector4 Sw = new Vector4(0, 0, 0, 0);
			for (int l = 0; l <= q; ++l) {
				//Sw.add(temp[l].multiplyScalar(Nv[l]));
				Sw = Sw + (temp[l] * (Nv[l]));
			}
			
			//Sw.divideScalar(Sw.w);
			Sw = Sw / (Sw.w);
			return new Vector3(Sw.x, Sw.y, Sw.z);
		}

	}
}
