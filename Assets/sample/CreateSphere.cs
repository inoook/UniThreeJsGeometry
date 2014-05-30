using UnityEngine;
using System.Collections;
using THREE;

public class CreateSphere : MonoBehaviour {

	public SphereGeometry geo;
	public MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		geo = new SphereGeometry(4, 12, 12, 0, Mathf.PI*2, 0, Mathf.PI);
		meshFilter.mesh = geo.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
