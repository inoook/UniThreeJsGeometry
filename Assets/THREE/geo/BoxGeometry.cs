using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class BoxGeometry : Geometry {
		
		float width;
		float height;
		float depth;
		int widthSegments;
		int heightSegments;
		int depthSegments;
		
		public BoxGeometry (float width, float height, float depth, int widthSegments, int heightSegments, int depthSegments)
		{
			
			this.width = width;
			this.height = height;
			this.depth = depth;
			
			this.widthSegments = widthSegments;
			this.heightSegments = heightSegments;
			this.depthSegments = depthSegments;
			
			float width_half = this.width / 2;
			float height_half = this.height / 2;
			float depth_half = this.depth / 2;
			
			buildPlane ("z", "y", - 1, - 1, this.depth, this.height, width_half, 0); // px
			buildPlane ("z", "y", 1, - 1, this.depth, this.height, - width_half, 1); // nx
			buildPlane ("x", "z", 1, 1, this.width, this.depth, height_half, 2); // py
			buildPlane ("x", "z", 1, - 1, this.width, this.depth, - height_half, 3); // ny
			buildPlane ("x", "y", 1, - 1, this.width, this.height, depth_half, 4); // pz
			buildPlane ("x", "y", - 1, - 1, this.width, this.height, - depth_half, 5); // nz
			
			//this.computeCentroids();
			this.mergeVertices();
			
		}
		
		void buildPlane (string u, string v, int udir, int vdir, float width, float height, float depth, int materialIndex)
		{
			string w = "z"; 
			int ix, iy;
			int gridX = this.widthSegments;
			int gridY = this.heightSegments;
			float width_half = width / 2.0f;
			float height_half = height / 2.0f;
			int offset = this.vertices.Count;
			
			
			if ((u == "x" && v == "y") || (u == "y" && v == "x")) {
				w = "z";
				
			} else if ((u == "x" && v == "z") || (u == "z" && v == "x")) {
				w = "y";
				gridY = depthSegments;
				
			} else if ((u == "z" && v == "y") || (u == "y" && v == "z")) {
				w = "x";
				gridX = depthSegments;
				
			}
			
			
			int _u = IndexFromStr (u);
			int _v = IndexFromStr (v);
			int _w = IndexFromStr (w);
			
			int gridX1 = gridX + 1;
			int gridY1 = gridY + 1;
			float segment_width = width / gridX;
			float segment_height = height / gridY;
			Vector3 normal = new Vector3 ();
			
			normal [_w] = depth > 0 ? 1 : - 1;
			
			//float uvFactorX = 1.0f/widthSegments;
			//float uvFactorY = 1.0f/heightSegments;
			
			for (iy = 0; iy < gridY1; iy ++) {
				
				for (ix = 0; ix < gridX1; ix ++) {
					
					Vector3 vector = new Vector3 ();
					vector [_u] = (ix * segment_width - width_half) * udir;
					vector [_v] = (iy * segment_height - height_half) * vdir;
					vector [_w] = depth;
					
					this.vertices.Add (vector);
				}
			}
			
			for (iy = 0; iy < gridY; iy++) {
				
				for (ix = 0; ix < gridX; ix++) {
					
					int a = ix + gridX1 * iy;
					int b = ix + gridX1 * (iy + 1);
					int c = (ix + 1) + gridX1 * (iy + 1);
					int d = (ix + 1) + gridX1 * iy;

					Vector2 uva = new Vector2( (float)ix / gridX, 1 - (float)iy / gridY );
					Vector2 uvb = new Vector2( (float)ix / gridX, 1 - (float)( iy + 1 ) / gridY );
					Vector2 uvc = new Vector2( (float)( ix + 1 ) / gridX, 1 - (float)( iy + 1 ) / gridY );
					Vector2 uvd = new Vector2( (float)( ix + 1 ) / gridX, 1 - (float)iy / gridY );
					
					Face3 face = new Face3( a + offset, b + offset, d + offset );
					face.vertexNormals = new List<Vector3>( new Vector3[]{ normal, normal, normal });
					//face.materialIndex = materialIndex;
					
					this.faces.Add( face );
					this.faceVertexUvs.Add( new List<Vector2>( new Vector2[]{ uva, uvb, uvd } ) );
					
					face = new Face3( b + offset, c + offset, d + offset );
					face.vertexNormals = new List<Vector3>( new Vector3[]{ normal, normal, normal });
					//face.materialIndex = materialIndex;

					this.faces.Add( face );
					this.faceVertexUvs.Add( new List<Vector2>( new Vector2[]{ uvb, uvc, uvd } ) );

				}
				
			}

		}

	}
}
