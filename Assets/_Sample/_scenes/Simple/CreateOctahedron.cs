using UnityEngine;
using System.Collections;
using THREE;

public class CreateOctahedron : CreateBase {

    [SerializeField] float radius = 10;
    [SerializeField] int detail = 3;

    public override void Create() {
        geo = new OctahedronGeometry(radius, detail);
	}
}
