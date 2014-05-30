/*
 * @author gyuque / http://github.com/gyuque
 *
 * Cylinder Mapping for ExtrudeGeometry
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE{
	public class UVsUtils {

		public class CylinderUVGenerator
		{
			int uRepeat = 1;
			Geometry targetGeometry = null;
			List<float> lengthCache = null;

			public List<Vector2> generateSideWallUV(Geometry geometry, Shape extrudedShape, List<Vector3> wallContour, ExtrudeGeometry.Option extrudeOptions,
			                            int indexA, int indexB, int indexC, int indexD, int stepIndex, int stepsLength,
			                             int contourIndex1, int contourIndex2 ) {
				// first call
				if (this.targetGeometry != geometry) {
					this.prepare(geometry, wallContour);
				}
				
				// generate uv
				var u_list = this.lengthCache;
				var v1 = stepIndex / stepsLength;
				var v2 = ( stepIndex + 1 ) / stepsLength;
				
				var u1 = u_list[contourIndex1];
				var u2 = u_list[contourIndex2];
				if (u1 < u2) {u1 += 1.0f;}
				
				u1 *= this.uRepeat;
				u2 *= this.uRepeat;
				return new List<Vector2>(new Vector2[]{
				        new Vector2( u1, v1 ),
				        new Vector2( u2, v1 ),
				        new Vector2( u2, v2 ),
				        new Vector2( u1, v2 )
				});
			}

			void prepare(Geometry geometry, List<Vector3> wallContour) {
				Vector3 p1, p2;
				List<float> u_list = new List<float>();
				float lengthSum = 0;
				int len = wallContour.Count;
				for (var i = 0;i < len;i++) {
					p1 = wallContour[ i ];
					p2 = wallContour[ (i+1) % len ];
					
					float dx = p1.x - p2.x;
					float dy = p1.y - p2.y;
					float segmentLength = Mathf.Sqrt(dx*dx + dy*dy);
					
					u_list.Add(lengthSum);
					lengthSum += segmentLength;
				}
				
				this.normalizeArray(u_list, lengthSum);
				this.targetGeometry = geometry;
				this.lengthCache = u_list;
			}

			void normalizeArray(List<float> ls, float v) {
				int len = ls.Count;
				for (int i = 0;i < len;i++) {
					ls[i] /= v;
				}
				//return ls;
			}

		}




	}
}
