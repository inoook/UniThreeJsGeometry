using UnityEngine;
using System.Collections;
using THREE;

public class CreateCircle : MonoBehaviour {

	public CircleGeometry geo;
	public MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		geo = new CircleGeometry(5, 32, 0, Mathf.PI * 2);
		meshFilter.mesh = geo.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
