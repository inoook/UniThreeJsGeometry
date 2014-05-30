using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class TetrahedronGeometry : PolyhedronGeometry
	{
	
		public TetrahedronGeometry (float radius, int detail)
		{
	
			List<float> vertices = new List<float> (new float[]{
		                1,  1,  1,   -1, -1,  1,   -1,  1, -1,    1, -1, -1
			});
		
			List<int> indices = new List<int> (new int[]{
		               2,  1,  0,    0,  3,  2,    1,  3,  0,    2,  3,  1
			});
		
			PolyhedronGeometryBuild (vertices, indices, radius, detail);
		}
	}
}
