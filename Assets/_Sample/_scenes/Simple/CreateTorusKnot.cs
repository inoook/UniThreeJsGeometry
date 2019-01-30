using UnityEngine;
using System.Collections;
using THREE;

public class CreateTorusKnot : CreateBase {

    [SerializeField] float radius = 10;
    [SerializeField] float tube = 4;
    [SerializeField] int radialSegments = 64;
    [SerializeField] int tubularSegments = 8;
    [SerializeField] float p = 2;
    [SerializeField] float q = 3;
    [SerializeField] float heightScale = 1;

    public override void Create() {
        geo = new TorusKnotGeometry(radius, tube, radialSegments, tubularSegments, p, q, heightScale);
    }
}
