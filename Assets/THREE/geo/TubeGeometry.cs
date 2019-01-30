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

		public List<Vector3> tangents;
//		public List<Vector3> normals;
		public List<Vector3> binormals;
		//private Curve path;

		public TubeGeometry ()
		{

		}

		public TubeGeometry (Curve path, int segments = 64, float radius = 1, int radialSegments = 8, bool closed = false)
		{
	
			//this.path = path;

			List<List<int>> grid = new List<List<int>> ();

			//Vector3 tangent;
			Vector3 normal;
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


			var frames = new THREE.TubeGeometry.FrenetFrames( path, segments, closed );
//			tangents = frames.tangents;
//			normals = frames.normals;
//			binormals = frames.binormals;

			// proxy internals
			this.tangents = frames.tangents;
			this.normals = frames.normals;
			this.binormals = frames.binormals;

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
				normal = normals [i];
				binormal = binormals [i];
			
				for (int j = 0; j < radialSegments; j++) {
				
					v = (float)j / radialSegments * 2.0f * Mathf.PI;
				
					cx = -radius * Mathf.Cos (v); // TODO: Hack: Negating it so it faces outside.
					cy = radius * Mathf.Sin (v);
				
					//pos2.copy( pos );
					pos2 = (pos);
					pos2.x += cx * normal.x + cy * binormal.x;
					pos2.y += cx * normal.y + cy * binormal.y;
					pos2.z += cx * normal.z + cy * binormal.z;
				
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

		// inner class
		public class FrenetFrames
		{
			public List<Vector3> tangents;
			public List<Vector3> normals;
			public List<Vector3> binormals;

			// For computing of Frenet frames, exposing the tangents, normals and binormals the spline
			public FrenetFrames (Curve path, int segments, bool closed)
			{
				//Vector3 tangent = new Vector3 ();
				Vector3 normal = new Vector3 ();
				//Vector3 binormal = new Vector3 ();
				
				Vector3 vec = new Vector3 ();
				//Matrix4x4 mat = new Matrix4x4 ();
			
				int numpoints = segments + 1;
				float theta;
				float epsilon = THREE.Setting.EPSILON_S;
				float smallest;
			
				float tx, ty, tz;
				float u;
				//float v;
			
				List<Vector3> tangents = new List<Vector3> (new Vector3[numpoints]);
				List<Vector3> normals = new List<Vector3> (new Vector3[numpoints]);
				List<Vector3> binormals = new List<Vector3> (new Vector3[numpoints]);
			
                Debug.LogWarning("FrenetFrames: "+numpoints + " / "+path);
				//vecs = new List<Vector3>(new Vector3[numpoints]);

				// expose internals
				this.tangents = tangents;
				this.normals = normals;
				this.binormals = binormals;
			
				// compute the tangent vectors for each segment on the path
				for (int i = 0; i < numpoints; i++) {
				
					u = (float)i / (numpoints - 1);
				
//					tangents [i] = path.getTangentAt(u);
//					tangents [i].Normalize ();
					Vector3 t_vec = path.getTangentAt(u);
					t_vec.Normalize();
					//tangents.Add( t_vec );
					tangents[i] = ( t_vec );
				}
			
				//initialNormal3();

				//void initialNormal3() {
				// select an initial normal vector perpenicular to the first tangent vector,
				// and in the direction of the smallest tangent xyz component
				
				normals.Add( new Vector3 () );
				binormals.Add( new Vector3 () );

				smallest = Mathf.Infinity;
				tx = Mathf.Abs (tangents [0].x);
				ty = Mathf.Abs (tangents [0].y);
				tz = Mathf.Abs (tangents [0].z);
				
				if (tx <= smallest) {
					smallest = tx;
					normal = new Vector3 (1, 0, 0);
				}
				
				if (ty <= smallest) {
					smallest = ty;
					normal = new Vector3 (0, 1, 0);
				}
				
				if (tz <= smallest) {
					normal = new Vector3 (0, 0, 1);
				}

				vec = Vector3.Cross (tangents [0], normal).normalized;

				//vec.crossVectors( tangents[ 0 ], normal ).normalize();
				
				//		normals[ 0 ].crossVectors( tangents[ 0 ], vec );
				//		binormals[ 0 ].crossVectors( tangents[ 0 ], normals[ 0 ] );
				
				//normals.Add( Vector3.Cross (tangents [0], vec) );
				//binormals.Add(Vector3.Cross (tangents [0], normals [0]));

				normals[0] = ( Vector3.Cross (tangents [0], vec) );
				binormals[0] = (Vector3.Cross (tangents [0], normals [0]));
				//}
			
				// compute the slowly-varying normal and binormal vectors for each segment on the path

				// js233
				for (int i = 1; i < numpoints; i++) {
				
					normals[i] = clone( normals [i - 1]);
					binormals[i] = clone(binormals [i - 1]);
				
					vec = Vector3.Cross (tangents [i - 1], tangents [i]);
				
					if (vec.magnitude > epsilon) {

						vec.Normalize ();

						//theta = Mathf.Acos( Mathf.Clamp( tangents[ i-1 ].dot( tangents[ i ] ), -1, 1 ) ); // clamp for floating pt errors
						float dot = Vector3.Dot (tangents [i - 1], tangents [i]);
						theta = Mathf.Acos (Mathf.Clamp (dot, -1, 1)); // clamp for floating pt errors
					
						//normals[ i ].applyMatrix4( mat.makeRotationAxis( vec, theta ) );
						Quaternion rot = Quaternion.AngleAxis (Mathf.Rad2Deg * (theta), vec);
						normals [i] = rot * normals [i];

						//vecs[i] = vec;
					}
					//binormals[ i ].crossVectors( tangents[ i ], normals[ i ] );
					binormals [i] = Vector3.Cross (tangents [i], normals [i]);
				}
			
			
				// if the curve is closed, postprocess the vectors so the first and last normal vectors are the same

				if (closed) {
					Debug.LogWarning("close---------");
					float dot = Vector3.Dot (normals [0], normals [numpoints - 1]);
					theta = Mathf.Acos (Mathf.Clamp (dot, -1, 1));
					theta /= (float)(numpoints - 1);

					Vector3 vec0 = Vector3.Cross (normals [0], normals [numpoints - 1]);
					float dot0 = Vector3.Dot (tangents [0], vec0);
					if (dot0 > 0) {
						//if ( tangents[ 0 ].dot( vec.crossVectors( normals[ 0 ], normals[ numpoints-1 ] ) ) > 0 ) {
						theta = -theta;
					}
				
					for (int i = 1; i < numpoints; i++) {
						// twist a little...
						//normals[ i ].applyMatrix4( mat.makeRotationAxis( tangents[ i ], theta * i ) );
						//binormals[ i ].crossVectors( tangents[ i ], normals[ i ] );

						Quaternion rot = Quaternion.AngleAxis (Mathf.Rad2Deg * (theta * i), tangents [i]);
						normals [i] = rot * normals [i];
						binormals [i] = Vector3.Cross (tangents [i], normals [i]);
					}
				}
			}

			public Vector3 clone (Vector3 vec)
			{
				return Utils.clone(vec);
			}
		}

	}
}
