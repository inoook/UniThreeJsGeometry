using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class SphereGeometry : Geometry
	{

		float radius;
		int widthSegments;
		int heightSegments;
		//float phiStart;
		//float phiLength;
		//float thetaStart;
		//float thetaLength;

		public SphereGeometry (float radius = 50, int widthSegments = 8, int heightSegments = 6, float phiStart = 0, float phiLength = Mathf.PI * 2, float thetaStart = 0, float thetaLength = Mathf.PI)
		{
			this.radius = radius;
		
			this.widthSegments = widthSegments = Mathf.Max (3, Mathf.FloorToInt (widthSegments));
			this.heightSegments = heightSegments = Mathf.Max (2, Mathf.FloorToInt (heightSegments));
		
			//this.phiStart = phiStart;
			//this.phiLength = phiLength;
		
			//this.thetaStart = thetaStart;
			//this.thetaLength = thetaLength;
		
			int x, y;
			List<List<int>> verticesIndex = new List<List<int>> ();
			List<List<Vector2>> uvs = new List<List<Vector2>> ();
		
			for (y = 0; y <= heightSegments; y ++) {
			
				List<int> verticesRow = new List<int> ();
				List<Vector2> uvsRow = new List<Vector2> ();
			
				for (x = 0; x <= widthSegments; x ++) {
				
					float u = (float)x / widthSegments;
					float v = (float)y / heightSegments;
				
					Vector3 vertex = new Vector3 ();
					vertex.x = - radius * Mathf.Cos (phiStart + u * phiLength) * Mathf.Sin (thetaStart + v * thetaLength);
					vertex.y = radius * Mathf.Cos (thetaStart + v * thetaLength);
					vertex.z = radius * Mathf.Sin (phiStart + u * phiLength) * Mathf.Sin (thetaStart + v * thetaLength);
				
					this.vertices.Add (vertex);

					//Vector2 uv = new Vector2( u, 1 - v );
					//this.uvs.Add( uv );
				
					verticesRow.Add (this.vertices.Count - 1);
					uvsRow.Add (new Vector2 (u, 1 - v));
				
				}
			
				verticesIndex.Add (verticesRow);
				uvs.Add (uvsRow);
			
			}
		
			for (y = 0; y < this.heightSegments; y ++) {
			
				for (x = 0; x < this.widthSegments; x ++) {
				
					int v1 = verticesIndex [y] [x + 1];
					int v2 = verticesIndex [y] [x];
					int v3 = verticesIndex [y + 1] [x];
					int v4 = verticesIndex [y + 1] [x + 1];

					Vector3 n1 = this.vertices[ v1 ].normalized;
					Vector3 n2 = this.vertices[ v2 ].normalized;
					Vector3 n3 = this.vertices[ v3 ].normalized;
					Vector3 n4 = this.vertices[ v4 ].normalized;

					Vector2 uv1 = uvs [y] [x + 1];
					Vector2 uv2 = uvs [y] [x];
					Vector2 uv3 = uvs [y + 1] [x];
					Vector2 uv4 = uvs [y + 1] [x + 1];

					if (Mathf.Abs (this.vertices [v1].y) == this.radius) {
					
						Face3 face = new Face3 (v1, v3, v4, new Vector3[]{ n1, n3, n4 });
						face.uvs = new Vector2[]{ uv1, uv3, uv4 };
						this.faces.Add (face);
					
					} else if (Mathf.Abs (this.vertices [v3].y) == this.radius) {
						Face3 face = new Face3 (v1, v2, v3, new Vector3[]{ n1, n2, n3 });
						face.uvs = new Vector2[]{ uv1, uv2, uv3 };
						this.faces.Add (face);
					
					} else {
						Face3 face0 = new Face3 (v1, v2, v4, new Vector3[]{ n1, n2, n4 });
						face0.uvs = new Vector2[]{ uv1, uv2, uv4 };
						this.faces.Add(face0 );

						Face3 face1 = new Face3( v2, v3, v4, new Vector3[]{ n2, n3, n4} );
						face1.uvs = new Vector2[]{ uv2, uv3, uv4 };
						this.faces.Add(face1 );
					}
				}
			}
			//this.computeFaceNormals();
			//this.mergeVertices();
		}
	}
}
