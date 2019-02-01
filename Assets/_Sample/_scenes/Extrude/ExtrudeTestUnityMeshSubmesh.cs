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

		float meshSmooth = 0;

		THREE.Geometry testGeometry;

		THREE.ClosedSplineCurve3 closedSpline = new THREE.ClosedSplineCurve3( new List<Vector3>(new Vector3[]{
		                                                  new Vector3( -60, -100,  60 ),
		                                                  new Vector3( -60,   20,  60 ),
		                                                  new Vector3( -60,  120,  60 ),
		                                                  new Vector3(  60,   20, -60 ),
		                                                  new Vector3(  60, -100, -60 )
		}));
		
		THREE.ExtrudeGeometry.Option extrudeSettings  = new THREE.ExtrudeGeometry.Option();
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

        testGeometry = new ExtrudeGeometry( shape, extrudeSettings );
		//testGeometry.computeVertexNormals();
		
		UnityEngine.Mesh mesh0 = testGeometry.GetMesh(meshSmooth);
		vertices.AddRange(mesh0.vertices);
		normals.AddRange(mesh0.normals);
		uvs.AddRange(mesh0.uv);
		
		//
		List<Vector3> randomPoints = new List<Vector3>();
		for ( int i = 0; i < 10; i ++ ) {
			randomPoints.Add( new Vector3( ( i - 4.5f ) * 50, Random.Range( - 50.0f, 50.0f ), Random.Range( - 50.0f, 50.0f ) ) );
		}
		
		SplineCurve3 randomSpline =  new SplineCurve3( randomPoints );

		THREE.ExtrudeGeometry.Option extrude0Settings  = new THREE.ExtrudeGeometry.Option();
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

		testGeometry = new ExtrudeGeometry( startShape, extrude0Settings );

		UnityEngine.Mesh mesh1 = testGeometry.GetMesh(meshSmooth);
		vertices.AddRange(mesh1.vertices);
		normals.AddRange(mesh1.normals);
		uvs.AddRange(mesh1.uv);

//		//
		THREE.ExtrudeGeometry.Option extrude1Settings  = new ExtrudeGeometry.Option();
		extrude1Settings.amount = 20;
		extrude1Settings.steps = 1;
		extrude1Settings.bevelEnabled = false;
		extrude1Settings.bevelThickness = 2;
		extrude1Settings.bevelSize = 4;
		extrude1Settings.bevelSegments = 1;

		testGeometry = new THREE.ExtrudeGeometry( startShape, extrude1Settings );
		
		UnityEngine.Mesh mesh2 = testGeometry.GetMesh(meshSmooth);
		vertices.AddRange(mesh2.vertices);
		normals.AddRange(mesh2.normals);
		uvs.AddRange(mesh2.uv);


		// unity submesh
		MeshRenderer mr = this.gameObject.AddComponent<MeshRenderer>();
		MeshFilter mf = this.gameObject.AddComponent<MeshFilter>();
		UnityEngine.Mesh mesh = new UnityEngine.Mesh();

		mesh.vertices = vertices.ToArray();
		mesh.normals = normals.ToArray();
		mesh.uv = uvs.ToArray();

		mesh.subMeshCount = 3;

		mesh.SetTriangles(mesh0.triangles, 0);

		int offsetIndex1 = mesh0.vertexCount;

		int[] orgTris = mesh1.triangles;
		int[] tris = new int[orgTris.Length];
		for(int j = 0; j < tris.Length; j++){
			tris[j] = orgTris[j] + offsetIndex1;
		}
		mesh.SetTriangles(tris, 1);

		int offsetIndex2 = offsetIndex1 + mesh1.vertexCount;
		
		int[] orgTris2 = mesh2.triangles;
		int[] tris2 = new int[orgTris2.Length];
		for(int j = 0; j < tris2.Length; j++){
			tris2[j] = orgTris2[j] + offsetIndex2;
		}
		mesh.SetTriangles(tris2, 2);

		//mesh.RecalculateNormals();

		mf.mesh = mesh;
		mr.materials = materials;
	}

}
