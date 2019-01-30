//https://github.com/mrdoob/three.js/blob/master/src/extras/geometries/PolyhedronGeometry.js
// https://github.com/mrdoob/three.js/blob/b7279adc60d366ff33a3e662576f349720139820/src/extras/geometries/PolyhedronGeometry.js
/**
 * @author clockworkgeek / https://github.com/clockworkgeek
 * @author timothypratley / https://github.com/timothypratley
 * @author WestLangley / http://github.com/WestLangley
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace THREE
{
	public class PolyhedronGeometry : Geometry
	{

		//float radius;
		//int detail;
		public List<THREEVector3> t_vertices;
		THREEVector3 centroid;

		public void PolyhedronGeometryBuild (List<float> vertices, List<int> indices, float radius = 1, int detail = 0)
		{
			t_vertices = new List<THREEVector3> ();
		
			//this.radius = radius;
			//this.detail = detail;

			for (int i = 0, l = vertices.Count; i < l; i += 3) {
				prepare (new THREEVector3 (vertices [i], vertices [i + 1], vertices [i + 2]));
				// this.t_vertices に要素が追加される
			}
		
			//var midpoints = [];
			List<THREEVector3> p = this.t_vertices;
		
			List<Face3> faces = new List<Face3> ();
			for (int i = 0, j = 0, l = indices.Count; i < l; i += 3, j ++) {
				THREEVector3 v1 = p [indices [i]];
				THREEVector3 v2 = p [indices [i + 1]];
				THREEVector3 v3 = p [indices [i + 2]];

				faces.Add (new Face3 (v1.index, v2.index, v3.index, new Vector3[]{ v1.vec, v2.vec, v3.vec } ));
			}
		
			centroid = new THREEVector3 ();
		
			for (int i = 0, l = faces.Count; i < l; i ++) {
				subdivide (faces [i], detail);
			}
		
			// Handle case when face straddles the seam
			for (int i = 0, l = this.faces.Count; i < l; i ++) {
			
				Vector2[] uvs = this.faces [i].uvs;
			
				float x0 = uvs [0].x;
				float x1 = uvs [1].x;
				float x2 = uvs [2].x;
			
				float max = Mathf.Max (x0, Mathf.Max (x1, x2));
				float min = Mathf.Min (x0, Mathf.Min (x1, x2));
			
				if (max > 0.9f && min < 0.1f) { // 0.9 is somewhat arbitrary
				
					if (x0 < 0.2f) { 
						Vector2 v = uvs [0];
						v.x += 1;
						uvs [0] = v;
					}
					if (x1 < 0.2f) { 
						//uvs[ 1 ].x += 1;
						Vector2 v = uvs [1];
						v.x += 1;
						uvs [1] = v;
					}
					if (x2 < 0.2f) { 
						//uvs[ 2 ].x += 1;
						Vector2 v = uvs [2];
						v.x += 1;
						uvs [2] = v;
					}
				}
			}
		
			// Apply radius
			for (int i = 0, l = this.t_vertices.Count; i < l; i ++) {
				//this.t_vertices[ i ].multiplyScalar( radius );
				this.t_vertices [i].vec *= radius;
			}
		
			// Merge vertices
			//this.mergeVertices();
			//this.computeCentroids();
			//this.computeFaceNormals();
		
		
			// THREEVector3 から Vector3に変換して、描画用のverticesに格納
			for (int i = 0; i < t_vertices.Count; i++) {
				this.vertices.Add (t_vertices [i].vec);
			}

			this.mergeVertices ();
		}
		
		THREEVector3 prepare (THREEVector3 vector)
		{
			// この中でverticesに要素を追加している
			THREEVector3 vertex = new THREEVector3 ();
			vertex.vec = vector.vec.normalized;
			this.t_vertices.Add (vertex);
			int count = this.t_vertices.Count;
			vertex.index = count - 1;
		
			// Texture coords are equivalent to map coords, calculate angle and convert to fraction of a circle.
		
			float u = azimuth (vector) / 2.0f / Mathf.PI + 0.5f;
			float v = inclination (vector) / Mathf.PI + 0.5f;
			vertex.uv = new Vector2 (u, 1 - v);
		
			return vertex;
		}
	
		// Approximate a curved face with recursively sub-divided triangles.
	
		void make (THREEVector3 v1, THREEVector3 v2, THREEVector3 v3)
		{
			Face3 face = new Face3 (v1.index, v2.index, v3.index, new Vector3[]{ v1.vec, v2.vec, v3.vec } );

			// centroid.copy( v1 ).add( v2 ).add( v3 ).divideScalar( 3 );
			//THREEVector3 centroid = new THREEVector3();
			//centroid.vec = Vector3.zero;
			centroid.vec = (v1.vec + v2.vec + v3.vec) / 3.0f;
			float azi = azimuth (centroid);

			face.uvs = new Vector2[] {
				correctUV (v1.uv, v1, azi),
				correctUV (v2.uv, v2, azi),
				correctUV (v3.uv, v3, azi)
			};
			this.faces.Add (face);
		}
	
	
		// Analytically subdivide a face to the required detail level.
	
		void subdivide (Face3 face, int detail)
		{
		
			float cols = Mathf.Pow (2.0f, (float)detail);
			//float cells = Mathf.Pow (4.0f, (float)detail);
			THREEVector3 a = prepare (this.t_vertices [face.a]);
			THREEVector3 b = prepare (this.t_vertices [face.b]);
			THREEVector3 c = prepare (this.t_vertices [face.c]);

			List<List<THREEVector3>> v = new List<List<THREEVector3>> ();
		
			// Construct all of the vertices for this subdivision.
		
			for (int i = 0; i <= cols; i ++) {
			
				//v[ i ] = new List<THREEVector3>();
				List<THREEVector3> vList = new List<THREEVector3> ();
				v.Add (vList);
				//v.Add (new List<THREEVector3>() );
			
				THREEVector3 aj = prepare (a.clone ().lerp (c, (float)i / cols));
				THREEVector3 bj = prepare (b.clone ().lerp (c, (float)i / cols));
				int rows = (int)(cols - i);
			
				for (int j = 0; j <= rows; j ++) {
					THREEVector3 vv;
					if (j == 0 && i == cols) {

						//v[ i ][ j ] = aj;
						vv = aj;
					} else {
						//v[ i ][ j ] = prepare( aj.clone().lerp( bj, j / rows ) );
						vv = prepare (aj.clone ().lerp (bj, (float)j / rows));
					}
					vList.Add (vv);
				}
			
			}
		
			// Construct all of the faces.
		
			for (int i = 0; i < cols; i ++) {
			
				for (int j = 0; j < 2 * (cols - i) - 1; j ++) {
				
					int k = Mathf.FloorToInt ((float)j / 2.0f);
				
					if (j % 2 == 0) {
					
						make (
						v [i] [k + 1],
						v [i + 1] [k],
						v [i] [k]
						);
					
					} else {
					
						make (
						v [i] [k + 1],
						v [i + 1] [k + 1],
						v [i + 1] [k]
						);
					
					}
				
				}
			
			}
		
		}
	
	
		// Angle around the Y axis, counter-clockwise when looking from above.
	
		float azimuth (THREEVector3 vector)
		{
			return Mathf.Atan2 (vector.vec.z, -vector.vec.x);
		}
	
	
		// Angle above the XZ plane.
	
		float inclination (THREEVector3 vector)
		{
			return Mathf.Atan2 (-vector.vec.y, Mathf.Sqrt ((vector.vec.x * vector.vec.x) + (vector.vec.z * vector.vec.z)));
		}
	
	
		// Texture fixing helper. Spheres have some odd behaviours.
	
		Vector2 correctUV (Vector2 uv, THREEVector3 vector, float azimuth)
		{
		
			if ((azimuth < 0.0f) && (uv.x == 1.0f))
				uv = new Vector2 (uv.x - 1, uv.y);
			if ((vector.vec.x == 0.0f) && (vector.vec.z == 0.0f))
				uv = new Vector2 (azimuth / 2.0f / Mathf.PI + 0.5f, uv.y);
			//return uv.clone();
			return new Vector2 (uv.x, uv.y);
		}

		Vector2 correctUV_bk (Vector2 uv, THREEVector3 vector, float azimuth)
		{
			Vector2 t_uv = new Vector2 (uv.x, uv.y);
			if ((azimuth < 0.0f) && (uv.x == 1.0f)) { 
				t_uv = new Vector2 (uv.x - 1.0f, uv.y);
			}
			if ((vector.vec.x == 0.0f) && (vector.vec.z == 0.0f)) {
				t_uv = new Vector2 (azimuth / 2.0f / Mathf.PI + 0.5f, uv.y);
			}
			return t_uv;
		}

	}

	/// <summary>
	/// THREE vector3.
	/// </summary>
	public class THREEVector3
	{
		public Vector3 vec;
		public Vector2 uv;
		public int index;
		
		public THREEVector3 ()
		{
			vec = Vector3.zero;
		}
		
		public THREEVector3 (float x, float y, float z)
		{
			vec = new Vector3 (x, y, z);
		}
		
		public THREEVector3 clone ()
		{
			THREEVector3 v = new THREEVector3 ();
			//v.vec = new Vector3(this.vec.x, this.vec.y, this.vec.z);
			//v.uv = new Vector2(this.uv.x, this.uv.y);
			
			v.vec = this.vec;
			v.uv = this.uv;
			return v;
		}
		
		public THREEVector3 lerp (THREEVector3 toVec, float t)
		{
			this.vec = Vector3.Lerp (this.vec, toVec.vec, t);
			return this;
		}
	}
}
