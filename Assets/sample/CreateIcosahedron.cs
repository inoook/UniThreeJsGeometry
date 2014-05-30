using UnityEngine;
using System.Collections;
using THREE;

public class CreateIcosahedron : MonoBehaviour {

	public IcosahedronGeometry geo;
	public MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		geo = new IcosahedronGeometry(8, 1);
		meshFilter.mesh = geo.CreateMesh();
		Debug.Log(meshFilter.mesh.vertexCount);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
