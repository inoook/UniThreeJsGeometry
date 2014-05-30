using UnityEngine;
using System.Collections;
using THREE;

public class CreateMeshCube : MonoBehaviour {

	public BoxGeometry geo;
	public MeshFilter meshFilter;
	// Use this for initialization
	void Start () {
		geo = new BoxGeometry(10,10,10,3,3,3);
		meshFilter.mesh = geo.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
