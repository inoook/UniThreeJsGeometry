using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using THREE;

public class CreateExtrude : CreateBase {

    [SerializeField] int step = 10;
    [SerializeField] float star_innerSize = 10;
    [SerializeField] float star_outerSize = 20;
    [SerializeField] int star_NumPts = 6;

    [SerializeField] Transform[] pointTransList;

    [SerializeField] AnimationCurve thicknessCurve;

    public override void Create()
    {
        Shape shape = ShapeUtils.CreateStar(star_innerSize, star_outerSize, star_NumPts);
        //Shape shape = ShapeUtils.CreateFish();
        //Shape shape = ShapeUtils.CreateSmiley();

        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < pointTransList.Length; i++) {
            points.Add(pointTransList[i].position);
        }
        SplineCurve3 path = new SplineCurve3(points);

        ExtrudeGeometry.Option extrudeSettings = new ExtrudeGeometry.Option();
        extrudeSettings.steps = step;
        extrudeSettings.extrudePath = path;
        extrudeSettings.thicknessCurve = thicknessCurve;
        //extrudeSettings.bevelEnabled = true;

        extrudeSettings.uvGenerator = new ExtrudeGeometry.UVGenerator(80, 80);
        //extrudeSettings.uvGenerator = new UVsUtils.CylinderUVGenerator();

        //Shape startShape = ShapeUtils.CreateStar(star_innerSize, star_outerSize, star_NumPts);
        //Shape shape = Shape.Combine(startShape, ShapeUtils.CreateFish());
        geo = new ExtrudeGeometry(shape, extrudeSettings);
   }

}
