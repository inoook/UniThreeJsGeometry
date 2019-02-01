// https://github.com/mrdoob/three.js/blob/master/src/extras/geometries/TubeGeometry.js
/**
 * @author WestLangley / https://github.com/WestLangley
 * @author zz85 / https://github.com/zz85
 * @author miningold / https://github.com/miningold
 *
 * Modified from the TorusKnotGeometry by @oosmoxiecode
 *
 * Creates a tube which extrudes along a 3d spline
 *
 * Uses parallel transport frames as described in
 * http://www.cs.indiana.edu/pub/techreports/TR425.pdf
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class TubeGeometry : Geometry
	{
		public Vector3[] tangents;
		public Vector3[] binormals;
        
		public TubeGeometry (Curve path, int segments = 64, float radius = 1, int radialSegments = 8, bool closed = false)
		{
			List<List<int>> grid = new List<List<int>> ();

			Vector3 temp_normal;
			Vector3 binormal;
		
			int numpoints = segments + 1;
		
			//float x, y, z;
			//float tx, ty, tz;
			float u, v;
		
			float cx, cy;
			Vector3 pos, pos2 = new Vector3 ();
			int ip, jp;
			int a, b, c, d;
			Vector2 uva, uvb, uvc, uvd;
            
			var frames = new FrenetFrames( path, segments, closed );

			// proxy internals
			this.tangents = frames.tangents;
			this.binormals = frames.binormals;
            this.normals = new List<Vector3>(frames.normals);

            //		function vert( x, y, z ) {
            //			
            //			return scope.vertices.push( new THREE.Vector3( x, y, z ) ) - 1;
            //			
            //		}

            // consruct the grid

            for (int i = 0; i < numpoints; i++) {
			
				//grid[ i ] = [];
				grid.Add (new List<int> ());
			
				u = (float)i / (numpoints - 1);
			
				pos = path.getPointAt (u);
			
				//tangent = tangents [i];
				temp_normal = normals [i];
				binormal = binormals [i];
			
				for (int j = 0; j < radialSegments; j++) {
				
					v = (float)j / radialSegments * 2.0f * Mathf.PI;
				
					cx = -radius * Mathf.Cos (v); // TODO: Hack: Negating it so it faces outside.
					cy = radius * Mathf.Sin (v);
				
					//pos2.copy( pos );
					pos2 = (pos);
					pos2.x += cx * temp_normal.x + cy * binormal.x;
					pos2.y += cx * temp_normal.y + cy * binormal.y;
					pos2.z += cx * temp_normal.z + cy * binormal.z;
				
					//grid[ i ][ j ] = vert( pos2.x, pos2.y, pos2.z );
					grid [i].Add (vert (pos2.x, pos2.y, pos2.z));
				
				}
			}

			// construct the mesh
			
			for (int i = 0; i < segments; i++ ) {
				
				for (int j = 0; j < radialSegments; j++ ) {
					
					ip = ( closed ) ? (i + 1) % segments : i + 1;
					jp = (j + 1) % radialSegments;
					
					a = grid[ i ][ j ];		// *** NOT NECESSARILY PLANAR ! ***
					b = grid[ ip ][ j ];
					c = grid[ ip ][ jp ];
					d = grid[ i ][ jp ];
					
					uva = new Vector2( (float)i / segments, (float)j / radialSegments );
					uvb = new Vector2( (float)( i + 1 ) / segments, (float)j / radialSegments );
					uvc = new Vector2( (float)( i + 1 ) / segments, (float)( j + 1 ) / radialSegments );
					uvd = new Vector2( (float)i / segments, (float)( j + 1 ) / radialSegments );

					Face3 face0 = new Face3 (a, b, d);
					face0.uvs = new Vector2[]{ uva, uvb, uvd };
					this.faces.Add( face0 );
					
					Face3 face1 = new Face3 (b, c, d);
					face1.uvs = new Vector2[]{ uvb, uvc, uvd };
					this.faces.Add( face1 );
					
				}
			}

			//this.computeFaceNormals();
			//this.computeVertexNormals();
		}

		int vert (float x, float y, float z)
		{
			this.vertices.Add (new Vector3 (x, y, z));
			return this.vertices.Count - 1;
		
		}


	}
}
