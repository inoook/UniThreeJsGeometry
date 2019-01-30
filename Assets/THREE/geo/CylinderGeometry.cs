using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class CylinderGeometry : Geometry
	{

		public CylinderGeometry (float radiusTop = 5, float radiusBottom = 5, float height = 10, int radialSegments = 8, int heightSegments = 1, bool openEnded = false, float per = 1.0f)
		{
	
			float heightHalf = height / 2.0f;
		
			int x, y;
			List<List<int>> vertices = new List<List<int>> ();
			List<List<Vector2>> uvs = new List<List<Vector2>> ();
		
			for (y = 0; y <= heightSegments; y ++) {
			
				List<int> verticesRow = new List<int> ();
				List<Vector2> uvsRow = new List<Vector2> ();
			
				float v = (float)y / heightSegments;
				float radius = v * (radiusBottom - radiusTop) + radiusTop;
			
				for (x = 0; x <= radialSegments; x ++) {
				
					float u = (float)x / radialSegments;
				
					Vector3 vertex = new Vector3 ();
					vertex.x = radius * Mathf.Sin (u * Mathf.PI * 2 * per);
					vertex.y = - v * height + heightHalf;
					vertex.z = radius * Mathf.Cos (u * Mathf.PI * 2 * per);
				
					this.vertices.Add (vertex);

					verticesRow.Add (this.vertices.Count - 1);
					uvsRow.Add (new Vector2 (u, 1 - v));
				
				}
			
				vertices.Add (verticesRow);
				uvs.Add (uvsRow);
			
			}
		
			float tanTheta = (radiusBottom - radiusTop) / height;
			Vector3 na, nb;
		
			for (x = 0; x < radialSegments; x ++) {
				if (radiusTop != 0) {
					na =  (this.vertices [vertices [0] [x]]);
					nb =  (this.vertices [vertices [0] [x + 1]]);
				} else {
					na =  (this.vertices [vertices [1] [x]]);
					nb =  (this.vertices [vertices [1] [x + 1]]);
				}
			
				na.y = (Mathf.Sqrt (na.x * na.x + na.z * na.z) * tanTheta);
				nb.y = (Mathf.Sqrt (nb.x * nb.x + nb.z * nb.z) * tanTheta);
				na.Normalize ();
				nb.Normalize ();
			
				for (y = 0; y < heightSegments; y ++) {
				
					int v1 = vertices [y] [x];
					int v2 = vertices [y + 1] [x];
					int v3 = vertices [y + 1] [x + 1];
					int v4 = vertices [y] [x + 1];
				
					Vector3 n1 =  (na);
					Vector3 n2 =  (na);
					Vector3 n3 =  (nb);
					Vector3 n4 =  (nb);
				
					Vector2 uv1 =  (uvs [y] [x]);
					Vector2 uv2 =  (uvs [y + 1] [x]);
					Vector2 uv3 =  (uvs [y + 1] [x + 1]);
					Vector2 uv4 =  (uvs [y] [x + 1]);

					Face3 face0 = new Face3 (v1, v2, v4, new Vector3[]{ n1, n2, n4 });
					face0.uvs = new Vector2[]{ uv1, uv2, uv4 };
					this.faces.Add (face0);
				
					Face3 face1 = new Face3 (v2, v3, v4, new Vector3[]{ (n2), n3, (n4) } );
					face1.uvs = new Vector2[] {
						 (uv2),
						uv3,
						 (uv4)
					};
					this.faces.Add (face1);
				}
			
			}
		
			// top cap
		
			if (openEnded == false && radiusTop > 0) {
			
				this.vertices.Add (new Vector3 (0, heightHalf, 0));
			
				for (x = 0; x < radialSegments; x ++) {
				
					int v1 = vertices [0] [x];
					int v2 = vertices [0] [x + 1];
					int v3 = this.vertices.Count - 1;
				
					Vector3 n1 = new Vector3( 0, 1, 0 );
					Vector3 n2 = new Vector3( 0, 1, 0 );
					Vector3 n3 = new Vector3( 0, 1, 0 );
				
					Vector2 uv1 =  (uvs [0] [x]);
					Vector2 uv2 =  (uvs [0] [x + 1]);
					Vector2 uv3 = new Vector2 (uv2.x, 0);
				
					Face3 face = new Face3 (v1, v2, v3, new Vector3[]{ n1, n2, n3 });
					face.uvs = new Vector2[]{ uv1, uv2, uv3 };
					this.faces.Add (face);
				}
			
			}
		
			// bottom cap
		
			if (openEnded == false && radiusBottom > 0) {
			
				this.vertices.Add (new Vector3 (0, - heightHalf, 0));
			
				for (x = 0; x < radialSegments; x ++) {
				
					int v1 = vertices [y] [x + 1];
					int v2 = vertices [y] [x];
					int v3 = this.vertices.Count - 1;
				
					Vector3 n1 = new Vector3( 0, - 1, 0 );
					Vector3 n2 = new Vector3( 0, - 1, 0 );
					Vector3 n3 = new Vector3( 0, - 1, 0 );

					Vector2 uv1 =  (uvs [y] [x + 1]);
					Vector2 uv2 =  (uvs [y] [x]);
					//Vector2 uv3 = new Vector2( uv2.u, 1 );
					Vector2 uv3 = new Vector2 (uv2.x, 1);
				
					Face3 face = new Face3 (v1, v2, v3, new Vector3[]{ n1, n2, n3 });
					face.uvs = new Vector2[]{ uv1, uv2, uv3 };
					this.faces.Add (face);
				}
			
			}

			//this.mergeVertices ();
			
			//this.computeVertexNormals();
			//this.computeFaceNormals();
		}


	}
}
