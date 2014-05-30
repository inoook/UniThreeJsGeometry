using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class LatheGeometry : Geometry
	{

		public LatheGeometry (List<Vector3> points, int segments = 20, float phiStart = 0, float phiLength = Mathf.PI * 2)
		{
			float inversePointLength = 1.0f / (points.Count - 1);
			float inverseSegments = 1.0f / segments;
		
			for (int i = 0, il = segments; i <= il; i ++) {
			
				float phi = phiStart + i * inverseSegments * phiLength;
			
				float c = Mathf.Cos (phi),
				s = Mathf.Sin (phi);
			
				for (int j = 0, jl = points.Count; j < jl; j ++) {
				
					var pt = points [j];
				
					Vector3 vertex = new Vector3 ();
				
					vertex.x = c * pt.x - s * pt.y;
					vertex.y = s * pt.x + c * pt.y;
					vertex.z = pt.z;
				
					this.vertices.Add (vertex);
				
				}
			
			}
		
			int np = points.Count;
		
			for (int i = 0, il = segments; i < il; i ++) {
			
				for (int j = 0, jl = points.Count - 1; j < jl; j ++) {
				
					int _base = j + np * i;
					int a = _base;
					int b = _base + np;
					int c = _base + 1 + np;
					int d = _base + 1;

					float u0 = i * inverseSegments;
					float v0 = j * inversePointLength;
					float u1 = u0 + inverseSegments;
					float v1 = v0 + inversePointLength;

					this.faces.Add( new Face3( a, b, d ) );

					this.faceVertexUvs.Add( new List<Vector2>( new Vector2[]{
											new Vector2( u0, v0 ),
											new Vector2( u1, v0 ),
											new Vector2( u0, v1 )
					}) );

					this.faces.Add( new Face3( b, c, d ) );

					this.faceVertexUvs.Add( new List<Vector2>( new Vector2[]{
											new Vector2( u1, v0 ),
											new Vector2( u1, v1 ),
											new Vector2( u0, v1 )
					}) );


				}
			
			}

			this.mergeVertices();
			this.computeFaceNormals();
			this.computeVertexNormals();
		}
	}
}
