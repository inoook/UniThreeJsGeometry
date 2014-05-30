/**
 * @author oosmoxiecode
 * @author mrdoob / http://mrdoob.com/
 * based on http://code.google.com/p/away3d/source/browse/trunk/fp10/Away3DLite/src/away3dlite/primitives/Torus.as?r=2888
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class TorusGeometry : Geometry
	{

		// Use this for initialization
		public TorusGeometry (float radius = 10, float tube = 4, int radialSegments = 8, int tubularSegments = 6, float arc = Mathf.PI * 2)
		{
			Vector3 center = new Vector3 ();
			List<Vector2> uvs = new List<Vector2> ();
			List<Vector3> normals = new List<Vector3> ();
		
			for (int j = 0; j <= radialSegments; j ++) {
			
				for (int i = 0; i <= tubularSegments; i ++) {
				
					float u = (float)i / tubularSegments * arc;
					float v = (float)j / radialSegments * Mathf.PI * 2;
				
					center.x = radius * Mathf.Cos (u);
					center.y = radius * Mathf.Sin (u);
				
					var vertex = new Vector3 ();
					vertex.x = (radius + tube * Mathf.Cos (v)) * Mathf.Cos (u);
					vertex.y = (radius + tube * Mathf.Cos (v)) * Mathf.Sin (u);
					vertex.z = tube * Mathf.Sin (v);
				
					this.vertices.Add (vertex);
				
					uvs.Add (new Vector2 ((float)i / tubularSegments, (float)j / radialSegments));

					normals.Add ((vertex - center).normalized);
				
				}
			}
		
		
			for (int j = 1; j <= radialSegments; j ++) {
			
				for (int i = 1; i <= tubularSegments; i ++) {
				
					int a = (tubularSegments + 1) * j + i - 1;
					int b = (tubularSegments + 1) * (j - 1) + i - 1;
					int c = (tubularSegments + 1) * (j - 1) + i;
					int d = (tubularSegments + 1) * j + i;

					var face = new Face3( a, b, d, new List<Vector3>(new Vector3[]{ clone(normals[ a ]), clone(normals[ b ]), clone(normals[ d ]) }) );
					this.faces.Add( face );
					this.faceVertexUvs.Add( new List<Vector2>( new Vector2[]{ uvs[ a ], uvs[ b ], uvs[ d ] }) );
					
					face = new Face3( b, c, d, new List<Vector3>(new Vector3[]{ clone(normals[ b ]), clone(normals[ c ]), clone(normals[ d ]) }) );
					this.faces.Add( face );
					this.faceVertexUvs.Add( new List<Vector2>( new Vector2[]{ uvs[ b ], uvs[ c ], uvs[ d ] }) );

				}
			
			}

			this.computeFaceNormals();
			//this.computeVertexNormals();
		}
	}
}
