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
meshFilter.mesh = geo.CreateAndGetMesh();
~~~
