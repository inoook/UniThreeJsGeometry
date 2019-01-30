using UnityEngine;
using System.Collections;
using THREE;

public class CreateIcosahedron : CreateBase {

    [SerializeField] float radius = 8;
    [SerializeField] int detail = 1;
    
    public override void Create() {
        geo = new IcosahedronGeometry(radius, detail);
	}
}
