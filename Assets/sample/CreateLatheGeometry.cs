using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using THREE;

public class CreateLatheGeometry : MonoBehaviour {

	public LatheGeometry geo;
	public MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		List<Vector3> points = new List<Vector3>(new Vector3[]{
		              new Vector3(3.0f,0,-4.0f),
		              new Vector3(6.0f,0,-0.5f),
		              new Vector3(5.8f,0,0),
		              new Vector3(6.0f,0,0.5f),
		              new Vector3(3.0f,0,4.0f)
		} );
		geo = new LatheGeometry(points, 16);
		meshFilter.mesh = geo.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
