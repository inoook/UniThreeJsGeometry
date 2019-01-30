using UnityEngine;
using System.Collections;
using THREE;

public class CreateTorus : CreateBase {

    [SerializeField] float radius = 10;
    [SerializeField] float tube = 4;
    [SerializeField] int radialSegments = 8;
    [SerializeField] int tubularSegments = 6;
    [SerializeField] float arcRatio = 1;
    
    public override void Create(){
		geo = new TorusGeometry(radius, tube, radialSegments, tubularSegments, arcRatio * Mathf.PI * 2);
	}
}
