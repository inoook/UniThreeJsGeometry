using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using THREE;

public class TriangulateShapeSample : CreateBase
{

    [SerializeField] int numPts = 5;
    [SerializeField] float outerSize = 1f;
    [SerializeField] int innter_numPts = 5;
    [SerializeField] float innerSize = 1f;

    [SerializeField] Vector2 holePos;
    public override void Create() {

        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < numPts; i++) {
            float a = (float)i / numPts * Mathf.PI * 2;
            points.Add(new Vector2(Mathf.Cos(a) * outerSize, Mathf.Sin(a) * outerSize));
        }

        List<Vector2> hole_points = new List<Vector2>();
        for (int i = 0; i < innter_numPts; i++) {
            float a = (float)i / innter_numPts * Mathf.PI * 2;
            hole_points.Add(new Vector2(-Mathf.Cos(a) * innerSize + holePos.x, Mathf.Sin(a) * innerSize + holePos.y));
        }

        List<List<Vector2>> holesList = new List<List<Vector2>>() { hole_points };

        ShapeGeometry.Option option = new ShapeGeometry.Option();
        geo = new ShapeGeometry(points, holesList);
    }
}
