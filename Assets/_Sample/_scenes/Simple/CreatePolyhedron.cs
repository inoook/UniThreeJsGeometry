using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class CreatePolyhedron : CreateBase {

    [SerializeField] float radius = 10;
    [SerializeField] int detail = 3;

    public override void Create() {
        List<float> vertices = new List<float>(new float[]{
                        1, 0, 0,   -1, 0, 0,    0, 1, 0,    0,-1, 0,    0, 0, 1,    0, 0,-1});

        List<int> indices = new List<int>(new int[]{
                       0, 2, 4,    0, 4, 3,    0, 3, 5,    0, 5, 2,    1, 2, 5,    1, 5, 3,    1, 3, 4,    1, 4, 2 });

        geo = new PolyhedronGeometry(vertices, indices, radius, detail);
	}
}
