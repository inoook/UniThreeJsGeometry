UniThreeJsGeometry
==================

Unityでthree.jsのGeometryをなんとなくつかう。


Unity上で簡単なPrimiteveをつくりたいときなどに。

http://threejs.org/examples/#webgl_geometries

http://threejs.org/examples/#webgl_geometries2

あたりを、Unityで使えるように。

~~~cs
Geometry geo = new CylinderGeometry(radiusTop, radiusBottom, height, radialSegments, heightSegments, openEnded, per);
Geometry geo = new IcosahedronGeometry(radius, detail);
// etc...
meshFilter.mesh = geo.GetMesh();
~~~

https://threejs.org/examples/#webgl_geometry_extrude_shapes

~~~cs
[SerializeField] List<Vector3> points;
[SerializeField] int step = 10;
[SerializeField] float star_innerSize = 10;
[SerializeField] float star_outerSize = 20;
[SerializeField] int star_NumPts = 6;
[SerializeField] AnimationCurve thicknessCurve;

//
SplineCurve3 path = new SplineCurve3(points);

ExtrudeGeometry.Option extrudeSettings = new ExtrudeGeometry.Option();
extrudeSettings.steps = step;
extrudeSettings.extrudePath = path;
extrudeSettings.thicknessCurve = thicknessCurve;

extrudeSettings.uvGenerator = new ExtrudeGeometry.UVGenerator(80, 80);

Shape shape = ShapeUtils.CreateStar(star_innerSize, star_outerSize, star_NumPts);
geo = new ExtrudeGeometry(shape, extrudeSettings);

meshFilter.mesh = geo.GetMesh();
~~~
