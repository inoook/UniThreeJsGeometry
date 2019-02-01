// http://threejs.org/examples/webgl_geometry_extrude_shapes.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class ShapeTestUnity : MonoBehaviour
{
    public Material material;
    THREE.Geometry testGeometry;

    // Use this for initialization
    void Start()
    {
        // Triangle
        var triangleShape = ShapeUtils.CreateTriangle();
        //var triangleShape = ShapeUtils.CreateStar();

        // Square
        float sqLength = 80;
        THREE.Shape squareShape = ShapeUtils.CreateSquare(sqLength);

        // Rectangle
        float rectLength = 120, rectWidth = 40;
        THREE.Shape rectShape = ShapeUtils.CreateRectangle(rectLength, rectWidth);

        // Rounded rectangle
        var roundedRectShape = new THREE.Shape();
        float width = 50, height = 50, radius = 20;
        roundedRectShape = ShapeUtils.CreateRoundedRectangle(width, height, radius);

        // Heart
        Shape heartShape = ShapeUtils.CreateHeart();

        // Circle
        float circleRadius = 40;
        var circleShape = ShapeUtils.CreateCircle(circleRadius);

        // Fish
        Shape fishShape = ShapeUtils.CreateFish();

        // Arc circle
        Shape arcShape = ShapeUtils.CreateArcCircle();

        // Smiley
        var smileyShape = ShapeUtils.CreateSmiley();

        // 
        THREE.ExtrudeGeometry.Option extrudeSettings = new THREE.ExtrudeGeometry.Option();

        extrudeSettings.amount = 40;
        extrudeSettings.bevelEnabled = true;
        extrudeSettings.bevelSegments = 3;
        extrudeSettings.bevelSize = 6.0f;
        extrudeSettings.steps = 2;

        addShape(triangleShape, extrudeSettings, -180, 0, 0, 0, 0, 0, 1);
        addShape(roundedRectShape, extrudeSettings, -150, 150, 0, 0, 0, 0, 1);
        addShape(rectShape, extrudeSettings, -150, 250, 0, 0, 0, 0, 1 );
        addShape(squareShape, extrudeSettings, 150, 100, 0, 0, 0, 0, 1);
        addShape(heartShape, extrudeSettings, 60, 100, 0, 0, 0, Mathf.PI, 1);
        addShape(circleShape, extrudeSettings, 120, 250, 0, 0, 0, 0, 1);
        addShape(fishShape, extrudeSettings, -60, 200, 0, 0, 0, 0, 1);
        addShape(smileyShape, extrudeSettings, -200, 250, 0, 0, 0, Mathf.PI, 1);
        addShape(arcShape, extrudeSettings, 150, 0, 0, 0, 0, 0, 1);
    }

    void addShape(Shape shape, THREE.ExtrudeGeometry.Option extrudeSettings, float x, float y, float z, float rx, float ry, float rz, float s)
    {
        //THREE.Geometry points = shape.createPointsGeometry ();
        //THREE.Geometry spacedPoints = shape.createSpacedPointsGeometry (100);
        //Debug.LogWarning("TODO: CHECK use shape.createSpacedPointsGeometry");
        //shape.createSpacedPointsGeometry (100);

        // 2d shape
        ShapeGeometry.Option op = new ShapeGeometry.Option();

        Geometry shapeGeometry = new ShapeGeometry(shape, op);
        shapeGeometry.SetFaceNormals();
        AddRenderObject(shapeGeometry, material, new Vector3(x, y, z - 125), new Vector3(rx, ry, rz) * Mathf.Rad2Deg);

        // 3d shape
        Geometry geometry = new ExtrudeGeometry( shape, extrudeSettings);

        //threeMesh = new THREE.Mesh (geometry, material);
        //threeMesh.eulerAngles = new Vector3 (rx, ry, rz) * Mathf.Rad2Deg;
        AddRenderObject(geometry, material, new Vector3(x, y, z), new Vector3(rx, ry, rz) * Mathf.Rad2Deg);
    }

    THREE.Geometry AddRenderObject(THREE.Geometry geo, Material material, Vector3 position, Vector3 angle)
    {
        // TODO: GetMesh(30.0f)でスムージングではなく、各面ごと（側面と蓋面を分けて）にcomputeFaceNormals, computeVertexNormals でスムージングできないか？検討行う。
        //geo.computeFaceNormals();
        //geo.computeVertexNormals();

        // ここでのSmoothingの処理が重い。Unityのfbxインポートのsmoothingに似せている。綺麗になるが重い
        //float time = Time.realtimeSinceStartup;
        //geo.SetFaceNormals();
        //UnityEngine.Mesh mesh = geo.GetMesh(30.0f);
        UnityEngine.Mesh mesh = geo.GetMesh();
        UnityMeshUtils.RecalculateNormals(mesh, 30); // 速い！

        //Debug.Log(Time.realtimeSinceStartup - time);
        //UnityMeshUtils.MeshSmoothNormals2(mesh);

        position.y += -95;

        GameObject gObj = new GameObject();
        gObj.transform.SetParent(this.transform);

        gObj.transform.localPosition = position * 0.01f;
        gObj.transform.localEulerAngles = angle;
        gObj.transform.localScale = Vector3.one * 0.01f;

        MeshFilter mf = gObj.AddComponent<MeshFilter>();
        MeshRenderer mr = gObj.AddComponent<MeshRenderer>();

        mf.mesh = mesh;
        mr.material = material;

        //UnityMeshUtils.checkVertices(mesh);

        return geo;
    }

}
