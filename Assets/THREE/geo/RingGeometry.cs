using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class RingGeometry : Geometry
	{

		public RingGeometry (float innerRadius = 0, float outerRadius = 50, int thetaSegments = 8, int phiSegments = 8, float thetaStart = 0, float thetaLength = Mathf.PI * 2)
		{

			List<Vector2> uvs = new List<Vector2> ();
			float radius = innerRadius;
			float radiusStep = ((outerRadius - innerRadius) / (float)phiSegments);
		
			for (int i = 0; i <= phiSegments; i ++) { // concentric circles inside ring
			
				for (int o = 0; o <= thetaSegments; o ++) { // number of segments per circle
				
					Vector3 vertex = new Vector3 ();
					float segment = thetaStart + (float)o / thetaSegments * thetaLength;
				
					vertex.x = radius * Mathf.Cos (segment);
					vertex.y = radius * Mathf.Sin (segment);
				
					this.vertices.Add (vertex);
					uvs.Add (new Vector2 ((vertex.x / outerRadius + 1) / 2.0f, (vertex.y / outerRadius + 1) / 2.0f));
				}
				radius += radiusStep;
			}
		
		
			Vector3 n = new Vector3 (0, 0, 1);
		
			for (int i = 0; i < phiSegments; i ++) { // concentric circles inside ring
			
				int thetaSegment = i * thetaSegments;
			
				for (int o = 0; o <= thetaSegments; o ++) { // number of segments per circle
				
					int segment = o + thetaSegment;
				
					int v1 = segment + i;
					int v2 = segment + thetaSegments + i;
					int v3 = segment + thetaSegments + 1 + i;
				
					this.faces.Add (new Face3 (v1, v2, v3, new List<Vector3>( new Vector3[]{ n, n, n} ) ));
					this.faceVertexUvs.Add (new List<Vector2> (new Vector2[] {
						uvs [v1],
						uvs [v2],
						uvs [v3]
					}));
				
					v1 = segment + i;
					v2 = segment + thetaSegments + 1 + i;
					v3 = segment + 1 + i;
				
					this.faces.Add (new Face3 (v1, v2, v3, new List<Vector3>( new Vector3[]{ n, n, n} ) ));
					this.faceVertexUvs.Add (new List<Vector2> (new Vector2[] {
						uvs [v1],
						uvs [v2],
						uvs [v3]
					}));
				
				}
			}

		}

	}
}
