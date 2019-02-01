using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using THREE;

public class CreateTube : CreateBase {

    [SerializeField] int segments = 64;
    [SerializeField] float radius = 1;
    [SerializeField] int radialSegments = 8;
    [SerializeField] bool closed = false;

    [SerializeField] Transform[] pointTransList;

    public override void Create()
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < pointTransList.Length; i++) {
            points.Add(pointTransList[i].position);
        }
        SplineCurve3 path = new SplineCurve3(points);

        geo = new TubeGeometry(path, segments, radius, radialSegments, closed);
    }

}
