using UnityEngine;
using System.Collections;
using THREE;

public class CreateTorusKnot : MonoBehaviour {

	public TorusKnotGeometry geo;
	public MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		geo = new TorusKnotGeometry();
		meshFilter.mesh = geo.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
