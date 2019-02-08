// http://threejs.org/examples/webgl_geometry_extrude_shapes.html
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class ExtrudeTestUnityMeshSubmesh : MonoBehaviour {

	public Material[] materials;


	// Use this for initialization
	void Start () {

		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();

		ClosedSplineCurve3 closedSpline = new ClosedSplineCurve3( new List<Vector3>(new Vector3[]{
		                                                  new Vector3( -60, -100,  60 ),
		                                                  new Vector3( -60,   20,  60 ),
		                                                  new Vector3( -60,  120,  60 ),
		                                                  new Vector3(  60,   20, -60 ),
		                                                  new Vector3(  60, -100, -60 )
		}));
		
		ExtrudeGeometry.Option extrudeSettings  = new ExtrudeGeometry.Option();
		extrudeSettings.steps = 100;
		extrudeSettings.bevelEnabled = false;
		extrudeSettings.extrudePath	= closedSpline;

        List<Vector2> pts = new List<Vector2>();
        List<Vector2> _normals = new List<Vector2>();
        int count = 7;

        float normOffset = Mathf.PI / count;
        for (int i = 0; i < count; i++)
        {
            float l = 20;
            float a = (float)i / count * Mathf.PI * 2;
            pts.Add(new Vector2(Mathf.Cos(a) * l, Mathf.Sin(a) * l));

            _normals.Add(new Vector2(Mathf.Cos(a + normOffset), Mathf.Sin(a + normOffset)));
        }

        Shape shape = new Shape(pts, _normals);

        Geometry testGeometry0 = new ExtrudeGeometry( shape, extrudeSettings );
        testGeometry0.UpdateUnityMeshInfo();
        vertices.AddRange(testGeometry0.uni_vertices);
        normals.AddRange(testGeometry0.uni_normals);
        uvs.AddRange(testGeometry0.uni_uvs);

        //
        List<Vector3> randomPoints = new List<Vector3>();
		for ( int i = 0; i < 10; i ++ ) {
			randomPoints.Add( new Vector3( ( i - 4.5f ) * 50, Random.Range( - 50.0f, 50.0f ), Random.Range( - 50.0f, 50.0f ) ) );
		}
		
		SplineCurve3 randomSpline =  new SplineCurve3( randomPoints );

		ExtrudeGeometry.Option extrude0Settings  = new ExtrudeGeometry.Option();
		extrude0Settings.steps = 200;
		extrude0Settings.bevelEnabled = false;
		extrude0Settings.extrudePath = randomSpline;


        // star path
        List<Vector2> pts0 = new List<Vector2>();
        List<Vector2> normals0 = new List<Vector2>();
        int numPts = 5;

        for (int i = 0; i < numPts * 2; i++)
        {
            int l = i % 2 == 1 ? 10 : 20;
            float a = (float)i / numPts * Mathf.PI;
            pts0.Add(new Vector2(Mathf.Cos(a) * l, Mathf.Sin(a) * l));
        }
        for (int i = 0; i < pts0.Count; i++)
        {
            int endI = (i == pts0.Count - 1) ? 0 : i + 1;
            Vector2 vec = pts0[endI] - pts0[i];
            vec.Normalize();
            normals0.Add(new Vector2(vec.y, -vec.x));
        }

        Shape startShape = new Shape(pts0, normals0);

        Geometry testGeometry1 = new ExtrudeGeometry( startShape, extrude0Settings );
        testGeometry1.UpdateUnityMeshInfo();
        vertices.AddRange(testGeometry1.uni_vertices);
        normals.AddRange(testGeometry1.uni_normals);
        uvs.AddRange(testGeometry1.uni_uvs);

        //
        ExtrudeGeometry.Option extrude1Settings  = new ExtrudeGeometry.Option();
		extrude1Settings.amount = 20;
		extrude1Settings.steps = 1;
		extrude1Settings.bevelEnabled = false;
		extrude1Settings.bevelThickness = 2;
		extrude1Settings.bevelSize = 4;
		extrude1Settings.bevelSegments = 1;

        Geometry testGeometry2 = new ExtrudeGeometry( startShape, extrude1Settings );
        testGeometry2.UpdateUnityMeshInfo();
        vertices.AddRange(testGeometry2.uni_vertices);
        normals.AddRange(testGeometry2.uni_normals);
        uvs.AddRange(testGeometry2.uni_uvs);


        // unity submesh -----
        MeshRenderer mr = this.gameObject.AddComponent<MeshRenderer>();
		MeshFilter mf = this.gameObject.AddComponent<MeshFilter>();
		UnityEngine.Mesh mesh = new UnityEngine.Mesh();

		mesh.vertices = vertices.ToArray();
		mesh.normals = normals.ToArray();
		mesh.uv = uvs.ToArray();

        // triangles
        int offsetIndex1 = testGeometry0.uni_vertices.Count;
        int offsetIndex2 = offsetIndex1 + testGeometry1.uni_vertices.Count;

        int[] orgTris0 = testGeometry0.uni_triangle.ToArray();
        int[] orgTris1 = testGeometry1.uni_triangle.ToArray();
        int[] orgTris2 = testGeometry2.uni_triangle.ToArray();

        mesh.subMeshCount = 3;

        mesh.SetTriangles(orgTris0, 0);

        AddOffset( orgTris1, offsetIndex1);
        mesh.SetTriangles(orgTris1, 1);

        AddOffset( orgTris2, offsetIndex2);
        mesh.SetTriangles(orgTris2, 2);

        mf.mesh = mesh;
		mr.materials = materials;
	}

    // 複数の submesh 用に triangle に対して offset を足していく。
    void AddOffset( int[] orgTris, int offsetIndex) {
        int[] newTris = new int[orgTris.Length];
        for (int j = 0; j < orgTris.Length; j++) {
            orgTris[j] += offsetIndex;
        }
    }

}
