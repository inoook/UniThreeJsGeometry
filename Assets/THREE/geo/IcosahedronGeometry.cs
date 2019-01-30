using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class IcosahedronGeometry : PolyhedronGeometry
	{

		public IcosahedronGeometry (float radius, int detail)
		{

			float t = ( 1 + Mathf.Sqrt( 5.0f ) ) / 2.0f;
			
			List<float> vertices = new List<float>( new float[]{
			                -1,  t,  0,    1,  t,  0,   -1, -t,  0,    1, -t,  0,
			                0, -1,  t,    0,  1,  t,    0, -1, -t,    0,  1, -t,
			                t,  0, -1,    t,  0,  1,   -t,  0, -1,   -t,  0,  1
			});
			
			List<int> indices = new List<int>(new int[]{
			               0, 11,  5,    0,  5,  1,    0,  1,  7,    0,  7, 10,    0, 10, 11,
			               1,  5,  9,    5, 11,  4,   11, 10,  2,   10,  7,  6,    7,  1,  8,
			               3,  9,  4,    3,  4,  2,    3,  2,  6,    3,  6,  8,    3,  8,  9,
			               4,  9,  5,    2,  4, 11,    6,  2, 10,    8,  6,  7,    9,  8,  1
			});

			base.PolyhedronGeometryBuild (vertices, indices, radius, detail);
		}

	}
}
