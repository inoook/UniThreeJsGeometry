using UnityEngine;
using System.Collections;
using THREE;

public class CreateRing : CreateBase {

    [SerializeField] float innerRadius = 2;
    [SerializeField] float outerRadius = 8;
    [SerializeField] int thetaSegments = 20;
    [SerializeField] int phiSegments = 8;
    [SerializeField] float thetaStartRatio = 0;
    [SerializeField] float thetaLengthRatio = 1;

    public override void Create() {
        geo = new RingGeometry(innerRadius, outerRadius, thetaSegments, phiSegments, thetaStartRatio* Mathf.PI*2, thetaLengthRatio * Mathf.PI * 2);
	}
}
