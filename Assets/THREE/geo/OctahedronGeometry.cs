using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
    public class OctahedronGeometry : PolyhedronGeometry
    {
        public OctahedronGeometry(float radius, int detail)
        {
            List<float> vertices = new List<float>(new float[]{
                        1, 0, 0,   -1, 0, 0,    0, 1, 0,    0,-1, 0,    0, 0, 1,    0, 0,-1});

            List<int> indices = new List<int>(new int[]{
                       0, 2, 4,    0, 4, 3,    0, 3, 5,    0, 5, 2,    1, 2, 5,    1, 5, 3,    1, 3, 4,    1, 4, 2 });

            base.PolyhedronGeometryBuild(vertices, indices, radius, detail);
        }

    }
}
