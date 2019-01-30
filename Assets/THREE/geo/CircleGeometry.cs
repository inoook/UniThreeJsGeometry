// https://github.com/mrdoob/three.js/blob/b7279adc60d366ff33a3e662576f349720139820/src/extras/geometries/CircleGeometry.js
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class CircleGeometry : Geometry
	{

		public CircleGeometry (float radius_, int segments_, float thetaStart_, float thetaLength_)
		{
			List<Vector2> uvs = new List<Vector2> ();

			float radius = radius_;
		
//		float thetaStart = thetaStart_ != undefined ? thetaStart_ : 0;
//		float thetaLength = thetaLength_ != undefined ? thetaLength_ : Mathf.PI * 2;
//		int segments = segments_ != undefined ? Mathf.max( 3, segments_ ) : 8;
			float thetaStart = thetaStart_;
			float thetaLength = thetaLength_;
			int segments = segments_;

			Vector3 center = new Vector3 ();
			Vector2 centerUV = new Vector2 (0.5f, 0.5f);
		
			this.vertices.Add (center);
			uvs.Add (centerUV);
		
			for (int i = 0; i <= segments; i ++) {
			
				Vector3 vertex = new Vector3 ();
				float segment = thetaStart + ((float)i / segments) * thetaLength;
			
				vertex.x = radius * Mathf.Cos (segment);
				vertex.y = radius * Mathf.Sin (segment);
			
				this.vertices.Add (vertex);
				uvs.Add (new Vector2 ((vertex.x / radius + 1) / 2, (vertex.y / radius + 1) / 2));
			}
		
			Vector3 n = new Vector3 (0, 0, 1);
		
			for (int i = 1; i <= segments; i ++) {
			
				int v1 = i;
				int v2 = i + 1;
				int v3 = 0;

				Face3 face = new Face3 (v1, v2, v3, new Vector3[]{ n, n, n });
				face.uvs = new Vector2[] {
					uvs [i],
					uvs [i + 1],
					centerUV
				};
				this.faces.Add (face );
			}
		}
	}
}
