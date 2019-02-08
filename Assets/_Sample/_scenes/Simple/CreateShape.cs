using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class CreateShape : CreateBase {

    public override void Create() {
        Shape shape0 = ShapeUtils.CreateSmiley();
        shape0.curveSegments = 16; // カーブの細かさ

        Shape shape1 = ShapeUtils.CreateStar();

        Shape shape2 = ShapeUtils.CreateFish();
        shape2.curveSegments = 5;

        ShapeGeometry.Option option = new ShapeGeometry.Option();
        geo = new ShapeGeometry(new List<Shape>() { shape0, shape1, shape2 }, option);

        //Shape dmyShape = new Shape();
        //dmyShape.curveSegments = 18;
        ////dmyShape.moveTo(0, 0);
        ////dmyShape.ellipse(-20, 0, 20, 10, 0, Mathf.PI * 2, true);
        
        ////dmyShape.moveTo(60, 0);
        //dmyShape.absarc(-50, 50, 20, 0, Mathf.PI * 2, true);

        ////dmyShape.moveTo(20, 0);
        ////dmyShape.ellipse(20, 0, 20, 10, 0, Mathf.PI * 2, true);

        ////dmyShape.absarc(0, 0, 20, 0, Mathf.PI * 2, true);
        ////dmyShape.moveTo(10, 0);
        ////dmyShape.absellipse(0, 0, 10, 10, 0, Mathf.PI * 2, true);
        //geo = new ShapeGeometry(dmyShape);

    }
}
