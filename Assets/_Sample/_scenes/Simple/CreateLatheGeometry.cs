using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class CreateLatheGeometry : CreateBase {

    [SerializeField] int segments = 16;
    [SerializeField] float phiStart = 0;
    [SerializeField] float phiPer = 1;

    public override void Create() {
        //List<Vector3> points = new List<Vector3>(new Vector3[]{
        //              new Vector3(3.0f,0,-4.0f),
        //              new Vector3(6.0f,0,-0.5f),
        //              new Vector3(5.8f,0,0),
        //              new Vector3(6.0f,0,0.5f),
        //              new Vector3(3.0f,0,4.0f)
        //} );
        List<Vector3> points = new List<Vector3>(new Vector3[]{
                      new Vector3(0.0f,0,-4.0f),
                      new Vector3(6.0f,0,-0.5f),
                      new Vector3(5.8f,0,0)
        });
        geo = new LatheGeometry(points, segments, phiStart * Mathf.PI * 2, phiPer * Mathf.PI*2);
	}
}
