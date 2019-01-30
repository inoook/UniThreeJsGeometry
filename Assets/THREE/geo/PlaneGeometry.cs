using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class PlaneGeometry : Geometry
	{
		public PlaneGeometry (float width, float height, int widthSegments, int heightSegments)
		{
			int ix, iz;
			float width_half = width / 2;
			float height_half = height / 2;
		
			int gridX = widthSegments;
			int gridZ = heightSegments;
		
			int gridX1 = gridX + 1;
			int gridZ1 = gridZ + 1;
		
			float segment_width = width / gridX;
			float segment_height = height / gridZ;


			Vector3 normal = new Vector3 (0, 0, 1);

			//float uvFactorX = 1.0f/widthSegments;
			//float uvFactorY = 1.0f/heightSegments;
		
			for (iz = 0; iz < gridZ1; iz ++) {
				for (ix = 0; ix < gridX1; ix ++) {
				
					var x = ix * segment_width - width_half;
					var y = iz * segment_height - height_half;

					this.vertices.Add (new Vector3 (x, - y, 0));
					//this.uvs.Add( new Vector2(ix*uvFactorX, 1.0f - iz*uvFactorY));
				}
			
			}
		
			for (iz = 0; iz < gridZ; iz ++) {
				for (ix = 0; ix < gridX; ix ++) {
				
					int a = ix + gridX1 * iz;
					int b = ix + gridX1 * (iz + 1);
					int c = (ix + 1) + gridX1 * (iz + 1);
					int d = (ix + 1) + gridX1 * iz;

					Vector2 uva = new Vector2( (float)ix / gridX, 1 - (float)iz / gridZ );
					Vector2 uvb = new Vector2( (float)ix / gridX, 1 - (float)( iz + 1 ) / gridZ );
					Vector2 uvc = new Vector2( (float)( ix + 1 ) / gridX, 1 - (float)( iz + 1 ) / gridZ );
					Vector2 uvd = new Vector2( (float)( ix + 1 ) / gridX, 1 - (float)iz / gridZ );
					
					Face3 face0 = new Face3( a, b, d, new Vector3[]{normal, normal, normal} );
					
					face0.uvs = new Vector2[]{ uva, uvb, uvd };
					this.faces.Add( face0 );
					
					Face3 face1 = new Face3( b, c, d, new Vector3[]{normal, normal, normal} );
					
					face1.uvs = new Vector2[]{ uvb, uvc, uvd };
					this.faces.Add( face1 );
				}
			}
		}
	}
}
