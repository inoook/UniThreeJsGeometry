/**
 * @author oosmoxiecode
 * based on http://code.google.com/p/away3d/source/browse/trunk/fp10/Away3D/src/away3d/primitives/TorusKnot.as?spec=svn2473&r=2473
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class TorusKnotGeometry : Geometry
	{

		public TorusKnotGeometry (float radius = 10, float tube = 4, int radialSegments = 64, int tubularSegments = 8, float  p = 2, float  q = 3, float heightScale = 1)
		{
	
			//List<List<int>> grid = new Array( this.radialSegments );
			List<List<int>> grid = new List<List<int>> (radialSegments);
		
			Vector3 tang = new Vector3 ();
			Vector3 n = new Vector3 ();
			Vector3 bitan = new Vector3 ();
		
			for (int i = 0; i < radialSegments; ++ i) {
			
				grid.Add (new List<int> (tubularSegments));
				float u = (float)i / radialSegments * 2 * p * Mathf.PI;
				Vector3 p1 = getPos (u, q, p, radius, heightScale);
				Vector3 p2 = getPos (u + 0.01f, q, p, radius, heightScale);
				//tang.subVectors( p2, p1 );
				//n.addVectors( p2, p1 );
				tang = p2 - p1;
				n = p2 + p1;

				//bitan.crossVectors( tang, n );
				//n.crossVectors( bitan, tang );
			
				bitan = Vector3.Cross (tang, n);
				n = Vector3.Cross (bitan, tang);

				bitan.Normalize ();
				n.Normalize ();
			
				for (int j = 0; j < tubularSegments; ++ j) {
				
					float v = (float)j / tubularSegments * 2 * Mathf.PI;
					float cx = - tube * Mathf.Cos (v); // TODO: Hack: Negating it so it faces outside.
					float cy = tube * Mathf.Sin (v);
				
					Vector3 pos = new Vector3 ();
					pos.x = p1.x + cx * n.x + cy * bitan.x;
					pos.y = p1.y + cx * n.y + cy * bitan.y;
					pos.z = p1.z + cx * n.z + cy * bitan.z;

					this.vertices.Add (pos);
					//grid[ i ][ j ] = this.vertices.Count - 1;
					grid [i].Add (this.vertices.Count - 1);
				}
			
			}
		
			for (int i = 0; i < radialSegments; ++ i) {
			
				for (int j = 0; j < tubularSegments; ++ j) {
				
					int ip = (i + 1) % radialSegments;
					int jp = (j + 1) % tubularSegments;
				
					int a = grid [i] [j];
					int b = grid [ip] [j];
					int c = grid [ip] [jp];
					int d = grid [i] [jp];
				
					Vector2 uva = new Vector2 ((float)i / radialSegments, (float)j / tubularSegments);
					Vector2 uvb = new Vector2 ((float)(i + 1) / radialSegments, (float)j / tubularSegments);
					Vector2 uvc = new Vector2 ((float)(i + 1) / radialSegments, (float)(j + 1) / tubularSegments);
					Vector2 uvd = new Vector2 ((float)i / radialSegments, (float)(j + 1) / tubularSegments);
				
//					this.faces.Add (new Face4 (a, b, c, d));
//					this.faceVertexUvs.Add (new List<Vector2> (new Vector2[] {
//						uva,
//						uvb,
//						uvc,
//						uvd
//					}));

					this.faces.Add( new Face3( a, b, d ) );
					this.faceVertexUvs.Add( new List<Vector2> (new Vector2[] { uva, uvb, uvd } ));
					
					this.faces.Add( new Face3( b, c, d ) );
					this.faceVertexUvs.Add( new List<Vector2> (new Vector2[] { clone(uvb), uvc, clone(uvd) } ));
				}
			}

			this.computeFaceNormals();
			this.computeVertexNormals();
		}
	
		Vector3 getPos (float u, float in_q, float in_p, float radius, float heightScale)
		{
			float cu = Mathf.Cos (u);
			float su = Mathf.Sin (u);
			float quOverP = in_q / in_p * u;
			float cs = Mathf.Cos (quOverP);
		
			float tx = radius * (2 + cs) * 0.5f * cu;
			float ty = radius * (2 + cs) * su * 0.5f;
			float tz = heightScale * radius * Mathf.Sin (quOverP) * 0.5f;
		
			return new Vector3 (tx, ty, tz);
		}

//		Vector2 clone(Vector2 vec)
//		{
//			return Utils.clone(vec);
//		}

	}
}
