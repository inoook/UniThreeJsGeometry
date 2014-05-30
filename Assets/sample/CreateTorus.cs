using UnityEngine;
using System.Collections;
using THREE;

public class CreateTorus : MonoBehaviour {

	public TorusGeometry geo;
	public MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		geo = new TorusGeometry();
		meshFilter.mesh = geo.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
