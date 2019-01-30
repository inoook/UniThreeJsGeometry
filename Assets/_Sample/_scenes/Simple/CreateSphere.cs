using UnityEngine;
using System.Collections;
using THREE;

public class CreateSphere : CreateBase {

    [SerializeField] float radius = 4;
    [SerializeField] int widthSegments = 12;
    [SerializeField] int heightSegments = 12;
    [SerializeField] float phiStartRatio = 0;
    [SerializeField] float phiLengthRatio = 1;
    [SerializeField] float thetaStartRatio = 0;
    [SerializeField] float thetaLengthRatio = 1;

    public override void Create() {
        geo = new SphereGeometry(radius, widthSegments, heightSegments, phiStartRatio * Mathf.PI * 2, phiLengthRatio *Mathf.PI*2, thetaStartRatio * Mathf.PI, thetaLengthRatio * Mathf.PI);
	}
}
