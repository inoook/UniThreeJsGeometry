using UnityEngine;
using System.Collections;
using THREE;

public class CreateCylinder : CreateBase {

    [SerializeField] float radiusTop = 25;
    [SerializeField] float radiusBottom = 75;
    [SerializeField] float height = 100;
    [SerializeField] int radialSegments = 40;
    [SerializeField] int heightSegments = 5;
    [SerializeField] bool openEnded = false;
    [SerializeField] float per = 1.0f;
    
    public override void Create() {
        //geo = new CylinderGeometry(10,10,10, 12,1);
        geo = new CylinderGeometry(radiusTop, radiusBottom, height, radialSegments, heightSegments, openEnded, per);
	}
}
