using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using THREE;

public class ShapeUtils
{
    public static Shape CreateTriangle(Vector2 a, Vector2 b, Vector2 c)
    {
        var triangleShape = new THREE.Shape();
        triangleShape.moveTo(a.x, a.y);
        triangleShape.lineTo(b.x, b.y);
        triangleShape.lineTo(c.x, c.y);
        triangleShape.lineTo(a.x, a.y); // close path

        return triangleShape;
    }

    public static Shape CreateTriangle()
    {
        return CreateTriangle(new Vector2(80, 20), new Vector2(40, 80), new Vector2(120, 80));
    }

    public static Shape CreateSquare(float sqLength)
    {
        var squareShape = new THREE.Shape();
        squareShape.moveTo(0, 0);
        squareShape.lineTo(0, sqLength);
        squareShape.lineTo(sqLength, sqLength);
        squareShape.lineTo(sqLength, 0);
        squareShape.lineTo(0, 0);

        return squareShape;
    }

    public static Shape CreateRectangle(float rectLength = 120, float rectWidth = 40)
    {
        var rectShape = new THREE.Shape();
        rectShape.moveTo(0, 0);
        rectShape.lineTo(0, rectWidth);
        rectShape.lineTo(rectLength, rectWidth);
        rectShape.lineTo(rectLength, 0);
        rectShape.lineTo(0, 0);

        return rectShape;
    }

    public static Shape CreateRoundedRectangle(float width = 50, float height = 50, float radius = 20)
    {
        var x = 0;
        var y = 0;
        var roundedRectShape = new THREE.Shape();

        roundedRectShape.moveTo(x, y + radius );
        roundedRectShape.lineTo(x, y + height - radius );
        roundedRectShape.quadraticCurveTo(x, y + height, x + radius, y + height );
        roundedRectShape.lineTo(x + width - radius, y + height) ;
        roundedRectShape.quadraticCurveTo(x + width, y + height, x + width, y + height - radius );
        roundedRectShape.lineTo(x + width, y + radius );
        roundedRectShape.quadraticCurveTo(x + width, y, x + width - radius, y );
        roundedRectShape.lineTo(x + radius, y );
        roundedRectShape.quadraticCurveTo(x, y, x, y + radius );

        return roundedRectShape;
    }

    public static Shape CreateHeart()
    {
        float x = 0; float y = 0;

        Shape heartShape = new Shape(); // From http://blog.burlock.org/html5/130-paths

        heartShape.moveTo(x + 25, y + 25);
        heartShape.bezierCurveTo(x + 25, y + 25, x + 20, y, x, y);
        heartShape.bezierCurveTo(x - 30, y, x - 30, y + 35, x - 30, y + 35);
        heartShape.bezierCurveTo(x - 30, y + 55, x - 10, y + 77, x + 25, y + 95);
        heartShape.bezierCurveTo(x + 60, y + 77, x + 80, y + 55, x + 80, y + 35);
        heartShape.bezierCurveTo(x + 80, y + 35, x + 80, y, x + 50, y);
        heartShape.bezierCurveTo(x + 35, y, x + 25, y + 25, x + 25, y + 25);

        return heartShape;
    }

    public static Shape CreateCircle(float circleRadius)
    {
        var circleShape = new THREE.Shape();
        circleShape.moveTo(0, circleRadius);
        circleShape.quadraticCurveTo(circleRadius, circleRadius, circleRadius, 0);
        circleShape.quadraticCurveTo(circleRadius, -circleRadius, 0, -circleRadius);
        circleShape.quadraticCurveTo(-circleRadius, -circleRadius, -circleRadius, 0);
        circleShape.quadraticCurveTo(-circleRadius, circleRadius, 0, circleRadius);

        return circleShape;
    }

    public static Shape CreateFish()
    {
        Shape fishShape = new Shape();
        float x = 0;
        float y = 0;
        fishShape.moveTo(x, y);
        fishShape.quadraticCurveTo(x + 50, y - 80, x + 90, y - 10);
        fishShape.quadraticCurveTo(x + 100, y - 10, x + 115, y - 40);
        fishShape.quadraticCurveTo(x + 115, y, x + 115, y + 40);
        fishShape.quadraticCurveTo(x + 100, y + 10, x + 90, y + 10);
        fishShape.quadraticCurveTo(x + 50, y + 80, x, y);

        return fishShape;
    }

    public static Shape CreateArcCircle(float outRadius = 40, float inRadius = 10)
    {
        // TODO: check パラメータの内容
        float a = 10;
        Shape arcShape = new Shape();
        arcShape.moveTo(outRadius + a, a);
        arcShape.absarc(a, a, outRadius, 0, Mathf.PI * 2.0f, false);

        Path holePath = new Path();
        holePath.moveTo(inRadius + a, a);
        holePath.absarc(a, a, inRadius, 0, Mathf.PI * 2.0f, true);


        //arcShape.moveTo(50, 10);
        //arcShape.absarc(10, 10, 40, 0, Mathf.PI * 2.0f, false);

        //Path holePath = new Path();
        //holePath.moveTo(20, 10);
        //holePath.absarc(10, 10, 10, 0, Mathf.PI * 2.0f, true);

        arcShape.holes.Add(holePath);

        return arcShape;
    }

    public static Shape CreateASmiley()
    {
        var smileyShape = new THREE.Shape();
        smileyShape.moveTo(80, 40);
        smileyShape.absarc(40, 40, 40, 0, Mathf.PI * 2, false);

        var smileyEye1Path = new THREE.Path();
        smileyEye1Path.moveTo(35, 20);
        // smileyEye1Path.absarc( 25, 20, 10, 0, Math.PI*2, true );
        smileyEye1Path.absellipse(25, 20, 10, 10, 0, Mathf.PI * 2, true);

        smileyShape.holes.Add(smileyEye1Path);

        var smileyEye2Path = new THREE.Path();
        smileyEye2Path.moveTo(65, 20);
        smileyEye2Path.absarc(55, 20, 10, 0, Mathf.PI * 2, true);

        smileyShape.holes.Add(smileyEye2Path);

        var smileyMouthPath = new THREE.Path();
        // ugly box mouth
        //      smileyMouthPath.moveTo( 20, 40 );
        //      smileyMouthPath.lineTo( 60, 40 );
        //      smileyMouthPath.lineTo( 60, 60 );
        //      smileyMouthPath.lineTo( 20, 60 );
        //      smileyMouthPath.lineTo( 20, 40 );

        smileyMouthPath.moveTo(20, 40);
        smileyMouthPath.quadraticCurveTo(40, 60, 60, 40);
        smileyMouthPath.bezierCurveTo(70, 45, 70, 50, 60, 60);
        smileyMouthPath.quadraticCurveTo(40, 80, 20, 60);
        smileyMouthPath.quadraticCurveTo(5, 50, 20, 40);


        smileyShape.holes.Add(smileyMouthPath);

        return smileyShape;
    }
}
